using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HeroUIManager : MonoBehaviour {

	public Transform HeroTransform;
	public Transform ParentTransform;
	public Text NameText;
	public Text EffectText;

	public Hero thisHero;

	void Awake()
	{
		thisHero = GetComponent<Hero> ();
	}

	void Start()
	{
		this.transform.SetParent (UnlocksManager.instance.HeroesPanel);
		this.transform.position = KingdomManager.instance.ActiveLocations.Find(x => x.LocationUID == thisHero.DefaultLocation.LocationUID).locationUIManager.HeroPanelTransform.position;
	}

	public void UpdateUI()
	{
		NameText.text = thisHero.Name + " lvl" + thisHero.Level;

		if (thisHero.CurrentState == HeroState.WORKING)
		{			
			if(thisHero.Stamina > 0)
				EffectText.text = thisHero.LocationAssigned.CurrentIncome * thisHero.Prospecting * 
					KingdomManager.instance.ProspectingMultiplier * KingdomManager.instance.GlobalMultiplier + "g";
			else
				EffectText.text = "0g";
		}

		if (thisHero.CurrentState == HeroState.GUARD)
		{
			if(thisHero.Stamina > 0)
				EffectText.text = thisHero.Strenght + " str";
			else
				EffectText.text = "powerless";
		}	

		if(thisHero.CurrentState == HeroState.IDLE)
		{
			EffectText.text = "Resting...";
		}

		if(thisHero.CurrentState == HeroState.FIGHTING)
		{
			EffectText.text = "In a fight!";
		}
	}
}
