using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class KingdomManager : MonoBehaviour {

	public float Gold;
	public float Mana;
	public float MaxMana;
	public float ManaRegen;
	public int MaxMasters;
	[HideInInspector]public int FreeMasters;

	public float GlobalMultiplier;

	[HideInInspector]public Text CurrentGoldText;
	[HideInInspector]public Text MastersText;

	public List<Location> ActiveLocations = new List<Location>();
	[HideInInspector]public List<Hero> ActiveHeroes = new List<Hero>();

	public static KingdomManager instance;


	void Awake () {
		if (instance == null)
			instance = this;
		else
		{
			if (instance != this)
				Destroy (this);
		}

		Ticker.OnTickEvent += GainMana;
		Ticker.OnTickEvent += CollectIncome;

		FreeMasters = MaxMasters;
		GlobalMultiplier = 1;
		Mana = MaxMana;
	}


	void UpdateUI () {
		CurrentGoldText.text = (Mathf.CeilToInt(Gold)).ToString() + "g";
		MastersText.text = "Free Masters: " + FreeMasters + " / " + MaxMasters;
	}

	//COLLECT GOLD on Tick by Ticker script
	public void CollectIncome(float interval)
	{
		foreach (var l in ActiveLocations)
			Gold += (l.CurrentIncome + l.MastersBonus) * interval * KingdomManager.instance.GlobalMultiplier;

		foreach (var h in ActiveHeroes) 
			if (h.CurrentState == HeroState.WORKING)
				Gold += h.LocationAssigned.CurrentIncome * h.Prospecting * 0.03f * KingdomManager.instance.GlobalMultiplier;

		UpdateUI ();
	}

	public void GainMana (float interval)
	{
		if(Mana < MaxMana)
			Mana = Mathf.Min (Mana + ManaRegen * interval, MaxMana);
	}
}
