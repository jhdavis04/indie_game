using UnityEngine;

[CreateAssetMenu(fileName = "NewAttack", menuName = "RPG/Moves/Attack")]
public class ElementalAttackMove : MoveData
{
    public int baseDamage;
    public override void Execute(BaseCharacter attacker, BaseCharacter target, TypeChart chart, float multiplier)
    {
        Debug.Log($"{attacker.characterName} used {moveName} on {target.characterName}!");
        int power = attacker.GetEffectiveAttack();
        float typeMod = chart.GetMultiplier(moveElement, target.enemyData != null ? target.enemyData.type : ElementType.Neutral);
        int damage = Mathf.RoundToInt((baseDamage + power) * typeMod * multiplier);
        target.TakeDamage(damage);
    }
}