using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LocationMastersManager : MonoBehaviour {
	
	public GameObject AddMasterButton;
	public GameObject RemoveMasterButton;

	Location thisLocation;

	void Start () {
		thisLocation = GetComponent<Location>();
	}

	public void AddMaster()
	{
		if (KingdomManager.instance.FreeMasters > 0) 
		{
			KingdomManager.instance.FreeMasters--;
			StartCoroutine ("TimeoutAddButton");
		}
	}

	public IEnumerator TimeoutAddButton()
	{
		var button = AddMasterButton.GetComponent<Button> ();
		var image = AddMasterButton.GetComponent<Image> ();

		button.interactable = false;
		image.fillAmount = 0;

		while (image.fillAmount < 1)
		{
			image.fillAmount += Time.deltaTime;
			yield return null;
		}

		button.interactable = true;
		thisLocation.MastersCount += 1;
	}

	public void RemoveMaster()
	{
		if(thisLocation.MastersCount > 0)
		{
			thisLocation.MastersCount -= 1;
			StartCoroutine ("TimeoutRemoveButton");
		}
	}

	public IEnumerator TimeoutRemoveButton()
	{
		var button = RemoveMasterButton.GetComponent<Button> ();
		var image = RemoveMasterButton.GetComponent<Image> ();

		button.interactable = false;
		image.fillAmount = 0;

		while (image.fillAmount < 1)
		{
			image.fillAmount += Time.deltaTime;
			yield return null;
		}

		button.interactable = true;
		KingdomManager.instance.FreeMasters++;
	}

}
