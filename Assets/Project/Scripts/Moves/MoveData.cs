using UnityEngine;

public abstract class MoveData : ScriptableObject
{
    public string moveName;
    public ElementType moveElement;
    public int chargeCost;
    public StatType scalingStat;
    public float multiplier = 1.0f;
    [TextArea] public string description;

    public abstract void Execute(BaseCharacter user, BaseCharacter target, TypeChart chart, int rank);
}