using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "Spell_InstantGoldSO", menuName = "SOs/Spell_InstantGoldSO", order = 3)]
public class InstantGoldSO : BaseSpellSO {

	public override void Execute (Spell thisSpell, GameObject target)
	{
		KingdomManager.instance.Gold += thisSpell.Value1;
	}

	public override void Revert (Spell thisSpell, GameObject target)
	{
		
	}
}
