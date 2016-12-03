using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public enum SpellType {GLOBAL, HERO, LOCATION};

[System.Serializable]
public class Spell
{
	public bool IsActive;
	public string Name;
	public BaseSpellSO ThisSpellSO;
	public SpellType ThisSpellType;
	public float Duration;
	public float ManaCost;
	public float Value1;
	public float Value2;
	public float Timer;
	public Button SpellButton;
	public GameObject TargetGO;

	public void ExecuteSpell()
	{		
		if (IsActive)
			return;

		if (ThisSpellType != SpellType.GLOBAL) 
		{
			SpellsManager.instance.spellInAction = this;
			SpellsManager.instance.StartCoroutine ("GetTarget", this);
		}
		else
		{
			ContinueExecution (null);
		}
	}

	public void ContinueExecution (GameObject targetGO)
	{
		if(KingdomManager.instance.Mana >= ManaCost)
		{
			KingdomManager.instance.Mana -= ManaCost;

			if (Duration != 0)
			{
				TargetGO = targetGO;
				IsActive = true;
				SpellButton.interactable = false;
				Timer = Time.time;
			}

			ThisSpellSO.Execute (this, targetGO);
		}
	}
}

public class SpellsManager : MonoBehaviour {

	public static SpellsManager instance;
	public GameObject SpellButtonGO;
	public Transform SpellsParentTransform;
	int offset = 0;

	public GraphicRaycaster thisGraphicRaycaster;

	public List<Spell> AvailableSpells = new List<Spell> ();

	public Spell spellInAction;

	void Start () {
		if (instance == null)
			instance = this;
		else
			if (instance != this)
				Destroy (this);

		SetSpellButtons ();

		Ticker.OnTickEvent += CheckSpellsTimer;
	}

	public void SetSpellButtons()
	{
		foreach (var s in AvailableSpells)
		{
			var upgradeButtonGO = (GameObject)Instantiate (SpellButtonGO, SpellsParentTransform, false);
			upgradeButtonGO.transform.position += new Vector3 (0, offset, 0);
			upgradeButtonGO.GetComponentInChildren<Text> ().text = s.Name;
			s.SpellButton = (Button)upgradeButtonGO.GetComponent<Button> ();
			s.SpellButton.onClick.AddListener (s.ExecuteSpell);

			offset -= 35;
		}
	}

	public IEnumerator GetTarget (Spell thisSpell)
	{
		if (thisSpell.ThisSpellType != SpellType.GLOBAL) 
		{
			while (!Input.GetMouseButtonDown (0))
			{
				if (Input.GetMouseButtonDown (1))
				{
					spellInAction = null;
					yield break;
				}
				yield return null;
			}

			GameObject targetGO = null;
			PointerEventData ped = new PointerEventData(null);
			ped.position = Input.mousePosition;
			List<RaycastResult> results = new List<RaycastResult>();

			thisGraphicRaycaster.Raycast (ped, results);

			//Cast on Location
			if (thisSpell.ThisSpellType == SpellType.LOCATION)
			{
				foreach (var r in results)
				{
					if (r.gameObject.GetComponent<Location> () != null)
						targetGO = r.gameObject;
					break;
				}
			}
			//Cast on Hero
			else
			{
				foreach (var r in results)
				{
					if (r.gameObject.GetComponent<Hero> () != null)
						targetGO = r.gameObject;
					break;
				}
			}

			if (targetGO != null)
				thisSpell.ContinueExecution (targetGO);
		}
		//If this Spell is GLOBAL
		else
		{
			thisSpell.ContinueExecution (null);
		}
	}

	//Check Duration Timer & Revert Spell Effect
	public void CheckSpellsTimer(float interval)
	{
		foreach (var s in AvailableSpells)
		{
			if (s.IsActive && (s.Timer + s.Duration < Time.time))
			{
				s.ThisSpellSO.Revert (s, s.TargetGO);
				s.IsActive = false;
				s.SpellButton.interactable = true;
			}
		}
	}
}
