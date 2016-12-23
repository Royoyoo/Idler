using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InfoPanelManager : MonoBehaviour {

	public Text NameText;
	public Text ProspectingText;
	public Text StrengthText;
	public Text StaminaText;
	public Text SpeedText;

	public Button StateButton;
	public Text StateText;

	public GameObject InfoPanelGO;

	public Hero SelectedHero;

	public static InfoPanelManager instance;

	void Start ()
	{
		if (instance == null)
			instance = this;
		else
			if (instance != this)
				Destroy (this);

		Ticker.OnTickEvent += UpdateUI;
	}

	public void Update()
	{
		if (Input.GetMouseButtonDown (1))
		{
			SelectedHero = null;
			InfoPanelGO.SetActive (false);
		}
	}

	public void UpdateUI(float interval)
	{
		if (SelectedHero != null)
			UpdateHeroInfo (SelectedHero);
	}

	public void UpdateHeroInfo(Hero thisHero)
	{
		if (!InfoPanelGO.activeSelf)
			InfoPanelGO.SetActive (true);

		SelectedHero = thisHero;

		NameText.text = thisHero.Name + " lvl" + thisHero.Level.ToString();
		ProspectingText.text = "Prospect: " + thisHero.Prospecting.ToString();
		StrengthText.text = "Strength: " + thisHero.Strenght.ToString();
		StaminaText.text = "Stamina: " + thisHero.Stamina.ToString();
		SpeedText.text = "Speed: " + thisHero.Speed.ToString();

		if(thisHero.CurrentState == HeroState.IDLE || thisHero.CurrentState == HeroState.MOVING)
			StateText.text = thisHero.CurrentState.ToString();
		else
			StateText.text = thisHero.CurrentState.ToString() + " at " + thisHero.LocationAssigned.LocationName;
	}

	public void ChangeHeroState()
	{
		SelectedHero.ChangeState ();
	}
}
