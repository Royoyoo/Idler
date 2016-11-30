using UnityEngine;
using System.Collections;

public abstract class LocationUpgradeSO : ScriptableObject {

	public abstract void Execute (Upgrade upgrade, Location thisLocation);
}
