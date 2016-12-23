using UnityEngine;
using System.Collections;

public class EnemiesManager : MonoBehaviour {

	public float SpawnRate;
	public float Timer;
	public bool CanSpawn;
	public GameObject EnemyGO;
	public Transform EnemiesParent;

	public static EnemiesManager instance;

	void Start ()
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
				Destroy (this);

		CanSpawn = true;
	}

	void Update()
	{
		if (CanSpawn && Timer + SpawnRate < Time.time)
			SpawnEnemies ();
	}

	public void SpawnEnemies()
	{
		foreach (var l in KingdomManager.instance.ActiveLocations)
		{
			if (l.Threat > 100f && l.EnemiesPresent.Count == 0)
			{
				GameObject thisEnemyGO = (GameObject)Instantiate (EnemyGO, EnemiesParent);
				thisEnemyGO.transform.position = l.transform.position;	//!!!!!
				var thisEnemy = thisEnemyGO.GetComponent<Enemy> ();
				thisEnemy.LocationAssigned = l;
				l.EnemiesPresent.Add (thisEnemy);
				CanSpawn = false;
			}
		}
	}
}
