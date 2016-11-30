using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UnlocksManager : MonoBehaviour {

	//Update it manually!!
	public List<Location> LocationPrefabs = new List<Location> ();
	public List<Hero> HeroesPrefabs = new List<Hero> ();

	public Transform LocationsParent;
	public Transform HeroesIdleParent;

	private List<Location> LocationToRemove = new List<Location> ();
	private List<Hero> HeroToRemove = new List<Hero> ();

	public static UnlocksManager instance;

	// Use this for initialization
	void Start () {
		if (instance == null)
			instance = this;
		else
		{
			if (instance != this)
				Destroy (this);
		}

		Ticker.OnTickEvent += CheckLocationsUnlock;
		Ticker.OnTickEvent += CheckHeroesUnlock;
	}

	public void CheckLocationsUnlock(float interval)
	{
		LocationToRemove.Clear ();

		foreach(var l in LocationPrefabs)
			if(KingdomManager.instance.Gold > l.GoldToUnlock)
			{
				Instantiate (l, LocationsParent, false);
				LocationToRemove.Add (l);
			}

		if(LocationToRemove.Count > 0)
			foreach(var l in LocationToRemove)
				LocationPrefabs.Remove (l);
	}

	public void CheckHeroesUnlock(float interval)
	{
		HeroToRemove.Clear ();

		foreach(var h in HeroesPrefabs)
			if(KingdomManager.instance.Gold > h.GoldToUnlock)
			{
				Instantiate (h);
				HeroToRemove.Add (h);
			}

		if(HeroToRemove.Count > 0)
			foreach(var h in HeroToRemove)
				HeroesPrefabs.Remove (h);
	}
}
