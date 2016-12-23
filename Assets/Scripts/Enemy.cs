﻿using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	public float AtackPower;
	public float Defense;
	public float Health;
	public float MaxHealth;
	public float HealthRegen;
	public bool InBattle;

	public Location LocationAssigned;

	void Update()
	{
		if (Health < 0)
		{
			LocationAssigned.EnemiesPresent.Remove (this);
			EnemiesManager.instance.CanSpawn = true;
			EnemiesManager.instance.Timer = Time.time;
			Destroy (this.gameObject);
		}
	}
}
