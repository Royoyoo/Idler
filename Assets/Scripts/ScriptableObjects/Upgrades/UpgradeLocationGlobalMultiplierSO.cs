using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "Location_GlobalMultiplierSO", menuName = "SOs/Location_GlobalMultiplierSO", order = 2)]
public class UpgradeLocationGlobalMultiplierSO : LocationUpgradeSO {

	public override void Execute (Upgrade upgrade, Location thisLocation)
	{
		KingdomManager.instance.GlobalMultiplier *= upgrade.UpgradeValue;

		foreach (var l in KingdomManager.instance.ActiveLocations)
			l.UpdateStats ();
	}
}
