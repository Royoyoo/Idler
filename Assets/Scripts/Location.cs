using UnityEngine;
using System.Collections.Generic;

public class Location : MonoBehaviour {

	public string LocationName;
	public int LocationUID;
	public int LocationLevel;
	public int UpgradeCost;
	public int UpgradeBonus;
	public int CurrentIncome;
	public int MastersBonus;

	public float UpgradeCostMultiplier;
	public float UpgradeBonusMultiplier;

	public int GoldToUnlock;

	public List<Hero> HeroesWorking = new List<Hero>();
	public List<Hero> HeroesGuarding = new List<Hero>();

	LocationUpgradesManager locationUpgradeMamager;
	public LocationUIManager locationUIManager;

	private int mastersCount;
	public int MastersCount {	get	{ return mastersCount; }
								set	{ mastersCount = value;	UpdateStats (1f);	}	}

	private int baseIncome;
	public int BaseIncome {	get	{ return baseIncome; }
							set	{ baseIncome = value;	UpdateStats (1f);	}	}

	private float locationMultiplier;
	public float LocationMultiplier {	get	{ return locationMultiplier; }
										set	{ locationMultiplier = value;	UpdateStats (1f);	}	}

	void Start () {

		locationUpgradeMamager = GetComponent<LocationUpgradesManager> ();
		locationUIManager = GetComponent<LocationUIManager> ();

		BaseIncome = CurrentIncome;
		LocationLevel = 1;
		LocationMultiplier = 1;
		MastersCount = 0;

		locationUIManager.UpdateUI ();

		KingdomManager.instance.ActiveLocations.Add (this);
		MapNavigator.instance.BuildPaths ();

		Ticker.OnTickEvent += UpdateStats;
	}
			

	public void BuyUpgrade()
	{
		if (KingdomManager.instance.Gold < UpgradeCost)
			return;

		LocationLevel++;
		BaseIncome += UpgradeBonus;
		KingdomManager.instance.Gold -= UpgradeCost;

		UpgradeCost  = Mathf.RoundToInt (UpgradeCost  + UpgradeCost  * UpgradeCostMultiplier);
		UpgradeBonus = Mathf.RoundToInt (UpgradeBonus + UpgradeBonus * UpgradeBonusMultiplier);

		locationUpgradeMamager.CheckUpgrades ();

		locationUIManager.UpdateUI ();
	}


	//Called when buying upgrade & adding/removing workers
	public void UpdateStats(float interval)
	{
		CurrentIncome = Mathf.RoundToInt (LocationMultiplier * BaseIncome);
		MastersBonus = Mathf.RoundToInt (CurrentIncome * MastersCount * 0.2f);
		locationUIManager.UpdateUI ();
	}
}
