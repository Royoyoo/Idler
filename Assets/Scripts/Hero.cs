using UnityEngine;
using System.Collections;

public enum HeroState {IDLE, WORKING, GUARD, MOVING, FIGHTING}

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
	HeroState previousState;

	public HeroUIManager heroUIManager;

	void Start ()
	{
		heroUIManager = GetComponent<HeroUIManager> ();

		Level = 1;
		Experience = 0;
		Stamina = MaxStamina;
		CurrentState = HeroState.IDLE;
		previousState = HeroState.IDLE;
		LocationAssigned = KingdomManager.instance.ActiveLocations.Find(l => l.LocationUID == DefaultLocation.LocationUID);

		KingdomManager.instance.ActiveHeroes.Add (this);

		Ticker.OnTickEvent += UpdateHeroStats;
	}

	//When Dropped Hero On Location |||| REWORK STATE-CHANGING METHOD
	public void AssignHero (Location targetLocation) 
	{
		LocationAssigned = targetLocation;

		switch (CurrentState)
		{
		case HeroState.WORKING:
			LocationAssigned.HeroesWorking.Add (this);
			break;

		case HeroState.GUARD:
			LocationAssigned.HeroesGuarding.Add (this);
			break;

		case HeroState.FIGHTING:
			LocationAssigned.HeroesGuarding.Add (this);
			break;

		default:
			break;
		}
	}
		
	public void UpdateHeroStats(float interval)
	{
		if (CurrentState == HeroState.WORKING || CurrentState == HeroState.GUARD)
		{
			Stamina = Mathf.Max(Stamina - 5f * interval, 0f);
			UpdateExp (interval);
		}

		if (CurrentState == HeroState.FIGHTING && LocationAssigned.EnemiesPresent.Count == 0)
			CurrentState = HeroState.GUARD;

		if (Stamina <= 0)
		{
			if (CurrentState == HeroState.WORKING)
				LocationAssigned.HeroesWorking.Remove (this);
			else if (CurrentState == HeroState.GUARD || CurrentState == HeroState.FIGHTING)
				LocationAssigned.HeroesGuarding.Remove (this);

			previousState = CurrentState;
			CurrentState = HeroState.IDLE;
		}

		if (CurrentState == HeroState.IDLE && Stamina < MaxStamina)
		{
			Stamina = Mathf.Min (Stamina + StaminaRecovery * interval, MaxStamina);
			if (previousState != HeroState.IDLE && Stamina >= MaxStamina)
			{
				CurrentState = previousState;
				previousState = HeroState.IDLE;
				AssignHero (LocationAssigned);
			}
		}

		heroUIManager.UpdateUI ();
	}
		
	public void ChangeState()
	{
		if (CurrentState == HeroState.WORKING) 
		{
			CurrentState = HeroState.GUARD;
			LocationAssigned.HeroesWorking.Remove (this);
			AssignHero (LocationAssigned);
		} 
		else if (CurrentState == HeroState.GUARD)
		{
			CurrentState = HeroState.IDLE;
			previousState = HeroState.IDLE;
			LocationAssigned.HeroesGuarding.Remove (this);
			AssignHero (LocationAssigned);
		}
		else if (CurrentState == HeroState.IDLE)
		{
			CurrentState = HeroState.WORKING;
			AssignHero (LocationAssigned);
		}
	}

	public void UpdateExp (float interval)
	{
		Experience += 10 * interval;
		if (Experience > 100)
		{
			Experience -= 100;
			Level++;
			Prospecting += ProspectingGain;
			Strenght += StrenghtGain;
		}
	}

	public void UpdateInfoPanel()
	{
		InfoPanelManager.instance.UpdateHeroInfo (this);
	}

	public IEnumerator MoveHero(Location finalLocation)
	{
		if (finalLocation == LocationAssigned)
			yield break;

		Location currentLocation = LocationAssigned;
		Location nextLocation;
		Vector3 nextLocationPosition;
		Vector3 currentPosition = LocationAssigned.locationUIManager.HeroPanelTransform.position;

		if (CurrentState == HeroState.WORKING)
			LocationAssigned.HeroesWorking.Remove (this);
		else if (CurrentState == HeroState.GUARD)
			LocationAssigned.HeroesGuarding.Remove (this);

		HeroState previousState;
		previousState = CurrentState;
		CurrentState = HeroState.MOVING;

		do {
			float dist;
			nextLocation = MapNavigator.instance.NextDestination (currentLocation, finalLocation, out dist);
			nextLocationPosition = nextLocation.locationUIManager.HeroPanelTransform.position;

			dist /= Speed;

			float t = 0;
			while (t < dist)
			{
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