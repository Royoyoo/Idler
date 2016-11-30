using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "Location_MultiplierSO", menuName = "SOs/Location_MultiplierSO", order = 1)]
public class UpgradeLocationMultiplierSO : LocationUpgradeSO {

	public override void Execute (Upgrade thisUpgrade, Location thisLocation)
	{
		thisLocation.LocationMultiplier *= thisUpgrade.UpgradeValue;
	}
}
