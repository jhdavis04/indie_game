using UnityEngine;

[CreateAssetMenu(fileName = "NewAttack", menuName = "RPG/Moves/Attack")]
public class ElementalAttackMove : MoveData
{
    public int baseDamage;
    public override void Execute(BaseCharacter user, BaseCharacter target, TypeChart chart, int rank)
    {
        float mult = chart.GetMultiplier(moveElement, target.data.type);
        int stat = scalingStat == StatType.Power ? user.GetEffectiveAttack() : user.GetEffectiveLuck();
        int dmg = Mathf.RoundToInt((baseDamage + (stat * multiplier)) * mult);
        target.TakeDamage(dmg);
    }
}