using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "Spell_TimedGlobalMultiplierSO", menuName = "SOs/Spell_TimedGlobalMultiplierSO", order = 4)]
public class TimedGlobalMultiplierSO : BaseSpellSO {

	public override void Execute (Spell thisSpell, GameObject target)
	{
		KingdomManager.instance.GlobalMultiplier *= thisSpell.Value1;
	}

	public override void Revert (Spell thisSpell, GameObject target)
	{
		KingdomManager.instance.GlobalMultiplier /= thisSpell.Value1;
	}
}
