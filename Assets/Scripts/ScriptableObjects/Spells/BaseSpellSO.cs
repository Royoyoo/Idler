using UnityEngine;
using System.Collections;

public abstract class BaseSpellSO : ScriptableObject {


	public abstract void Execute (Spell thisSpell, GameObject target);
	public abstract void Revert (Spell thisSpell, GameObject target);
}
