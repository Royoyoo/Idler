using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LocationUIManager : MonoBehaviour {

	public Text LocationNameText;
	public Text IncomeText;
	public Text UpgradeText;
	public Text MastersText;
	public Transform HeroPanelTransform;

	public Location thisLocation;

	void Awake () {
		thisLocation = GetComponent<Location>();
	}

	public void UpdateUI ()
	{
			LocationNameText.text = thisLocation.LocationName + " lvl " + thisLocation.LocationLevel;
			IncomeText.text = "Income: " + thisLocation.CurrentIncome * KingdomManager.instance.GlobalMultiplier + "g";
			UpgradeText.text = "Upgrade +" + thisLocation.UpgradeBonus * thisLocation.LocationMultiplier * KingdomManager.instance.GlobalMultiplier + "g\nCost: " + thisLocation.UpgradeCost + "g";
			MastersText.text = "M.Boost: " + thisLocation.MastersBonus * KingdomManager.instance.GlobalMultiplier + "  Masters: " + thisLocation.MastersCount;
	}
}
