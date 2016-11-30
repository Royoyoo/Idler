using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

[System.Serializable]
public class Upgrade
{
	public int UnlockLevel;
	public int Price;
	public LocationUpgradeSO UpgradeSO;
	public float UpgradeValue;
	public string UpgradeButtonText;

	[HideInInspector]public Location targetLocation;
	[HideInInspector]public bool Purchased;
	[HideInInspector]public Button UpgradeButton;

	public void ExecuteUpgrade()
	{
		if (KingdomManager.instance.Gold >= Price)
		{
			KingdomManager.instance.Gold -= Price;
			UpgradeSO.Execute (this, targetLocation);
			this.Purchased = true;
			UpgradeButton.interactable = false;
			UpgradeButton.onClick.RemoveListener (ExecuteUpgrade);
		}
	}
}

public class LocationUpgradesManager : MonoBehaviour {
	
	public List<Upgrade> LocationUpgrades = new List<Upgrade>();
	public GameObject UpgradeButtonGO;
	public Transform UpgradeButtonsParent;

	[HideInInspector]public Location ThisLocation;
	int offset = 0;

	// Use this for initialization
	void Start () {
		ThisLocation = GetComponent<Location> ();
		CheckUpgrades ();
	}

	public void CheckUpgrades ()
	{
		foreach (var u in LocationUpgrades)
		{
			if (ThisLocation.LocationLevel == u.UnlockLevel)
			{			
				u.targetLocation = ThisLocation;

				var upgradeButtonGO = (GameObject)Instantiate (UpgradeButtonGO, UpgradeButtonsParent, false);
				upgradeButtonGO.transform.position += new Vector3 (offset, 0, 0);
				upgradeButtonGO.GetComponentInChildren<Text> ().text = u.UpgradeButtonText;
				u.UpgradeButton = (Button)upgradeButtonGO.GetComponent<Button> ();		
				u.UpgradeButton.onClick.AddListener (u.ExecuteUpgrade);

				offset += 35;
			}
		}
	}
}
