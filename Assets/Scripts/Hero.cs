using UnityEngine;
using System.Collections;

public enum HeroState {REST, WORKING, GUARD, MOVING}

public class Hero : MonoBehaviour {

	public string Name;
	public int Level;
	public float Experience;
	public int Prospecting;
	public int ProspectingGain;
	public int Strenght;
	public int StrenghtGain;
	public float Speed;

	public float Health;
	public float MaxHealth;
	public float HealthRecovery;

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
		Experience = 0f;
		Health = MaxHealth;
		CurrentState = HeroState.REST;
		previousState = HeroState.REST;
		LocationAssigned = DefaultLocation = KingdomManager.instance.ActiveLocations.Find(l => l.LocationUID == DefaultLocation.LocationUID);

		KingdomManager.instance.ActiveHeroes.Add (this);

		Ticker.OnTickEvent += UpdateHeroStats;
	}
		
	public void ChangeHeroState(HeroState newState)
	{
		switch (CurrentState)
		{
			case HeroState.WORKING:
				LocationAssigned.HeroesWorking.Remove (this);
				break;

			case HeroState.GUARD:
				LocationAssigned.HeroesGuarding.Remove (this);
				break;

			default:
				break;
		}

		if(CurrentState != HeroState.REST && CurrentState != HeroState.MOVING)
			previousState = CurrentState;

		switch (newState)
		{
			case HeroState.WORKING:
				LocationAssigned.HeroesWorking.Add (this);
				break;

			case HeroState.GUARD:
				LocationAssigned.HeroesGuarding.Add (this);
				break;

			default:
				break;
		}

		CurrentState = newState;
	}

	public void UpdateHeroStats(float interval)
	{
		if (CurrentState == HeroState.WORKING || CurrentState == HeroState.GUARD)
			UpdateExp (interval);

		if (Health <= 0f)
		{
			ChangeHeroState (HeroState.REST);
			InstantMove (DefaultLocation);
			Health = 0f;
		}

		if (Health < MaxHealth && LocationAssigned.EnemiesPresent.Count == 0)
			Health = Mathf.Min (Health + HealthRecovery * interval, MaxHealth);

		heroUIManager.UpdateUI ();
	}

	void UpdateExp (float interval)
	{
		Experience += 10f * interval;
		if (Experience > 100f)
		{
			Experience -= 100f;
			Level++;
			Prospecting += ProspectingGain;
			Strenght += StrenghtGain;
		}
	}

	public void InstantMove(Location targetLocation)
	{
		transform.position = targetLocation.locationUIManager.HeroPanelTransform.position;
		LocationAssigned = targetLocation;
	}

	public IEnumerator MoveHero(Location finalLocation)
	{
		if (finalLocation == LocationAssigned)
			yield break;

		Location currentLocation = LocationAssigned;
		Location nextLocation;
		Vector3 nextLocationPosition;
		Vector3 currentPosition = LocationAssigned.locationUIManager.HeroPanelTransform.position;

		ChangeHeroState(HeroState.MOVING);

		do {
			float dist;
			nextLocation = MapNavigator.instance.NextDestination (currentLocation, finalLocation, out dist);
			nextLocationPosition = nextLocation.locationUIManager.HeroPanelTransform.position;

			dist /= Speed;

			float t = 0f;
			while (t < dist)
			{
				transform.position = Vector3.Lerp (currentPosition, nextLocationPosition, t / dist);
				t += Time.deltaTime;
				yield return null;
			}
			currentLocation = nextLocation;
			currentPosition = nextLocationPosition;

		} while (nextLocation != finalLocation);
			
		transform.position = nextLocationPosition;

		LocationAssigned = finalLocation;
		ChangeHeroState(previousState);
	}
}