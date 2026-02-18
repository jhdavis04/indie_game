using UnityEngine;

public abstract class StatusEffect : ScriptableObject
{
    public string effectName;
    public StatusEffectType type;
    public int duration;

    public abstract void OnTurnEnd(BaseCharacter character);
}