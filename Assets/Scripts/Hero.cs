using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public enum HeroState {IDLE, WORKING, GUARD, MOVING}

public class Hero : MonoBehaviour {

	public string Name;
	public int Level;
	public float Experience;
	public int Prospecting;
	public int ProspectingGain;
	public int Strenght;
	public int StrenghtGain;
	public float Speed;

	public float Stamina;
	public float MaxStamina;
	public float StaminaRecovery;

	public int GoldToUnlock;

	public Location LocationAssigned;
	public Location DefaultLocation;

	public HeroState CurrentState;

	public Transform HeroIdleTransform;
	public Transform ParentTransform;
	public Text NameText;
	public Text EffectText;

	void Start ()
	{
		Stamina = MaxStamina;
		NameText.text = Name;
		HeroIdleTransform = UnlocksManager.instance.HeroesIdleParent;
		ParentTransform = HeroIdleTransform;
		this.transform.SetParent (ParentTransform);
		transform.localPosition = new Vector3 (0f, 50 - HeroIdleTransform.childCount * 50f, 0f);
		Level = 1;
		Experience = 0;
		CurrentState = HeroState.IDLE;

		KingdomManager.instance.ActiveHeroes.Add (this);

		Ticker.OnTickEvent += UpdateHeroStats;
	}

	//When Dropped Hero On Location
	public void AssignHero (Location targetLocation) 
	{
		switch (CurrentState) 
		{
		case HeroState.IDLE:

			if (!targetLocation)
				return;

			CurrentState = HeroState.WORKING;

			LocationAssigned = targetLocation;
				ParentTransform = LocationAssigned.locationUIManager.HeroPanelTransform;
			this.transform.SetParent (ParentTransform);
			this.transform.localPosition = new Vector3 (0f, 0f, 0f);

			LocationAssigned.HeroesWorking.Add (this);
			break;

		case HeroState.WORKING:

			if (!targetLocation)
			{
				CurrentState = HeroState.IDLE;

				ParentTransform = HeroIdleTransform;
				this.transform.SetParent (ParentTransform);
				transform.localPosition = new Vector3 (0f, 50 - HeroIdleTransform.childCount * 50f, 0f);

				LocationAssigned.HeroesWorking.Remove (this);
				LocationAssigned = null;
				EffectText.text = "";
			}
			else
			{
				LocationAssigned.HeroesWorking.Remove (this);

				LocationAssigned = targetLocation;
				ParentTransform = LocationAssigned.locationUIManager.HeroPanelTransform;
				this.transform.SetParent (ParentTransform);
				this.transform.localPosition = new Vector3 (0f, 0f, 0f);

				LocationAssigned.HeroesWorking.Add (this);
			}
			break;

		case HeroState.GUARD:
			break;

		case HeroState.MOVING:
			break;

		default:
			break;
		}
	}
		
	public IEnumerator MoveHero(Location finalLocation)
	{
		if (finalLocation != LocationAssigned)
		{
			Location currentLocation = LocationAssigned;

			if (!LocationAssigned)
				currentLocation = DefaultLocation;				

			Location nextLocation;
			Vector3 nextLocationPosition;
			Vector3 currentPosition = ParentTransform.position;
			HeroState previousState;

			previousState = CurrentState;
			CurrentState = HeroState.MOVING;

			do {
				float dist;
				nextLocation = MapNavigator.instance.NextDestination (currentLocation, finalLocation, out dist);
				nextLocationPosition = nextLocation.transform.position;

				dist /= Speed;

				float t = 0;
				while (t < dist) {
					transform.position = Vector3.Lerp (currentPosition, nextLocationPosition, t / dist);
					t += Time.deltaTime;
					yield return null;
				}
				currentLocation = nextLocation;
				currentPosition = nextLocationPosition;

			} while (nextLocation != finalLocation);

			CurrentState = previousState;

			AssignHero (finalLocation);
		}
	}

	public void UpdateHeroStats(float interval)
	{
		if (CurrentState == HeroState.WORKING)
		{
			//MB Property with special setter for Stamina???
			Stamina = Mathf.Max(Stamina - 5f * interval, 0f);
			EffectText.text = LocationAssigned.CurrentIncome * Prospecting * 0.03f * KingdomManager.instance.GlobalMultiplier + "g";
			Experience += 50 * interval;
			if (Experience > 100)
				LevelUp ();
		}

		if(CurrentState == HeroState.IDLE && Stamina < MaxStamina)
			Stamina = Mathf.Min(Stamina + StaminaRecovery * interval, MaxStamina);
	}

	public void LevelUp ()
	{
		Experience -= 100;
		Level++;
		Prospecting += ProspectingGain;
		Strenght += StrenghtGain;
	}

	public void UpdateInfoPanel()
	{
		InfoPanelManager.instance.UpdateHeroInfo (this);
	}
}
