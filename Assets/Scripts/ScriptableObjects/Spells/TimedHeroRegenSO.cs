using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "Spell_TimedHeroRegenSO", menuName = "SOs/Spell_TimedHeroRegenSO", order = 5)]
public class TimedHeroRegenSO : BaseSpellSO {
	
	public override void Execute (Spell thisSpell, GameObject target)
	{
		var h = target.GetComponent<Hero> ();
		h.StaminaRecovery *= thisSpell.Value1;
	}
	public override void Revert (Spell thisSpell, GameObject target)
	{
		var h2 = target.GetComponent<Hero> ();
		h2.StaminaRecovery /= thisSpell.Value1;
	}	
}
