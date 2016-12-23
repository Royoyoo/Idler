using UnityEngine;
using System.Collections;

public class EnemiesManager : MonoBehaviour {

	public float SpawnRate;
	public GameObject EnemyGO;
	public Transform EnemiesParent;

	public static EnemiesManager instance;

	void Start ()
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
				Destroy (this);

		Ticker.OnTickEvent += CheckSpawns;
	}

	void CheckSpawns(float interval)
	{
		foreach(var l in KingdomManager.instance.ActiveLocations)
			if(l.Threat > 100 && l.LastEnemyTime + SpawnRate < Time.time && l.EnemiesPresent.Count < l.MaxEnemiesCount)
				SpawnEnemy (l);
	}

	public void SpawnEnemy(Location targetLocation)
	{
		GameObject thisEnemyGO = (GameObject)Instantiate (EnemyGO, EnemiesParent);
		thisEnemyGO.transform.position = targetLocation.transform.position;	//!!!!!
		var thisEnemy = thisEnemyGO.GetComponent<Enemy> ();
		thisEnemy.LocationAssigned = targetLocation;
		targetLocation.EnemiesPresent.Add (thisEnemy);
		targetLocation.LastEnemyTime = Time.time;
	}
}
