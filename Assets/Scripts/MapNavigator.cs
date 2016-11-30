using UnityEngine;
using System.Collections.Generic;

public class MapNavigator : MonoBehaviour {

	#region SupportClasses

	[System.Serializable]
	class NextDirection
	{
		public int FinalLocationUID;
		public int NextLocationUID = 0;
		public float BaseDistance = 0;

		public NextDirection(Location location)
		{
			FinalLocationUID = location.LocationUID;
		}
	}

	[System.Serializable]
	class PathFromThisLocation
	{
		public int CurrentLocationUID;
		public List<NextDirection> NextDir = new List<NextDirection>();

		public PathFromThisLocation(Location location)
		{
			CurrentLocationUID = location.LocationUID;
		}
	}

	[System.Serializable]
	public class MapConnection
	{
		public Location StartLocation;
		public Location EndLocation;
		public float Distance;
	}

	[System.Serializable]
	class Neighbour
	{
		public float Distance;
		public int NeighbourUID;

		public Neighbour(int neighbourUID, float dist)
		{
			Distance = dist;
			NeighbourUID = neighbourUID;
		}
	}

	[System.Serializable]
	class MapPoint
	{
		public int LocationUID;
		public float CalcDistance;
		public MapPoint CalcPrevious;
		public List<Neighbour> Neighbours = new List<Neighbour>();
	}

	#endregion

	List<PathFromThisLocation> CurrentMap = new List<PathFromThisLocation>();
	public List<MapConnection> MapConnections = new List<MapConnection> ();
	List<MapPoint> MapGraph = new List<MapPoint> ();

	List<Location> ActiveLocations = new List<Location> ();

	public static MapNavigator instance;

	void Awake()
	{
		if (instance == null)
			instance = this;
		else
			if (instance != this)
				Destroy (this);
	}

	#region Build & Fill Initial Map

	public void BuildPaths()
	{
		ActiveLocations.Clear ();
		foreach (var p in CurrentMap)
			p.NextDir.Clear();
		CurrentMap.Clear ();
		MapGraph.Clear ();

		ActiveLocations = new List<Location>(KingdomManager.instance.ActiveLocations);

		BuildGraph ();

		int currentUID = 0;

		foreach (var l in ActiveLocations)
		{
			currentUID = l.LocationUID;
			CurrentMap.Add (new PathFromThisLocation (l));

			foreach (var l2 in ActiveLocations)
			{
				List<NextDirection> dirList = CurrentMap [CurrentMap.Count - 1].NextDir;
				var nd = new NextDirection (l2);
				CalculateDirection(nd, currentUID);
				dirList.Add (nd);
			}
		}
	}

	void BuildGraph ()
	{
		foreach (var l in ActiveLocations) 
		{
			var mp = new MapPoint ();
			mp.LocationUID = l.LocationUID;

			foreach (var mc in MapConnections)
			{
				if (mp.LocationUID == mc.StartLocation.LocationUID)
					mp.Neighbours.Add (new Neighbour(mc.EndLocation.LocationUID, mc.Distance));
				if (mp.LocationUID == mc.EndLocation.LocationUID)
					mp.Neighbours.Add (new Neighbour(mc.StartLocation.LocationUID, mc.Distance));
			}

			MapGraph.Add (mp);
		}
	}

	#endregion

	#region Calculate Next Location for each MapPoint (Dijkstra method)

	NextDirection CalculateDirection(NextDirection nextDirection, int currentLocationUID)
	{
		if (nextDirection.FinalLocationUID == currentLocationUID)
		{
			nextDirection.NextLocationUID = currentLocationUID;
			nextDirection.BaseDistance = 0;
			return nextDirection;
		}

		List<MapPoint> graphSet = new List<MapPoint> (MapGraph);
		var path = new List<MapPoint> ();

		foreach (var mp in graphSet)
		{
			mp.CalcDistance = float.MaxValue;
			mp.CalcPrevious = null;
		}

		graphSet.Find(x => x.LocationUID == currentLocationUID).CalcDistance = 0;

		while (graphSet.Count != 0)
		{
			graphSet.Sort ((x, y) => x.CalcDistance.CompareTo (y.CalcDistance));
			var smallestMP = graphSet [0];
			graphSet.RemoveAt (0);

			if (smallestMP.LocationUID == nextDirection.FinalLocationUID)
			{
				while (smallestMP.CalcPrevious != null)
				{
					path.Add (smallestMP);
					smallestMP = smallestMP.CalcPrevious;
				}
				break;
			}

			if (smallestMP.CalcDistance == float.MaxValue)
				break;

			foreach (var n in smallestMP.Neighbours)
			{
				if (graphSet.Find (x => x.LocationUID == n.NeighbourUID) != null)
				{
					var alt = smallestMP.CalcDistance + n.Distance;
					var nMP = graphSet.Find (x => x.LocationUID == n.NeighbourUID);
					if (alt < nMP.CalcDistance) {
						nMP.CalcDistance = alt;
						nMP.CalcPrevious = smallestMP;
					}
				}
			}
		}
		if (path.Count == 0)
			Debug.Log ("Error: can't build path to this point");
		nextDirection.NextLocationUID = path [path.Count - 1].LocationUID;

		foreach (var mc in MapConnections)
		{
			if (currentLocationUID == mc.StartLocation.LocationUID && nextDirection.NextLocationUID == mc.EndLocation.LocationUID ||
				currentLocationUID == mc.EndLocation.LocationUID && nextDirection.NextLocationUID == mc.StartLocation.LocationUID)
				nextDirection.BaseDistance = mc.Distance;
		}

		return nextDirection;
	}

	#endregion

	public Location NextDestination(Location currentLocation, Location finalLocation, out float baseDistance)
	{
		var row = CurrentMap.Find (x => x.CurrentLocationUID == currentLocation.LocationUID);
		var nextdir = row.NextDir.Find (x => x.FinalLocationUID == finalLocation.LocationUID);
		baseDistance = nextdir.BaseDistance;
		return KingdomManager.instance.ActiveLocations.Find(x => x.LocationUID == nextdir.NextLocationUID);
	}
}
