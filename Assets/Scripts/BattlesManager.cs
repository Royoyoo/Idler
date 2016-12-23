using UnityEngine;
using System.Collections;

public class BattlesManager : MonoBehaviour {

	public float RoundDuration;
	public float RoundTimer;

	public static BattlesManager instance;

	void Start ()
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
				Destroy (this);

		RoundTimer = 0f;
	}

	void Update () {
		RoundTimer += Time.deltaTime;

		if(RoundTimer > RoundDuration)
		{
			RoundTimer -= RoundDuration;
			StartBattleRound ();
		}
	}

	public void StartBattleRound()
	{
		foreach (var l in KingdomManager.instance.ActiveLocations)
		{
			if (l.EnemiesPresent.Count == 0 || l.HeroesGuarding.Count == 0)
				return;

			foreach (var e in l.EnemiesPresent)
				if(l.HeroesGuarding.Count > 0)
					l.HeroesGuarding [0].Stamina -= e.AtackPower;

			foreach (var h in l.HeroesGuarding) 
			{
				if (l.EnemiesPresent.Count > 0) 
				{
					h.CurrentState = HeroState.FIGHTING;
					l.EnemiesPresent [0].Health -= h.Strenght * 0.5f;
				}
			}
		}
	}
}
