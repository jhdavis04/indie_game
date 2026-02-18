using UnityEngine;
using System.Collections.Generic;

public class BaseCharacter : MonoBehaviour 
{
    [Header("Data")]
    public EnemyData data;
    public PlayerState saveState;
    public bool isPlayer;

    [Header("Battle Stats")]
    public int level;
    public int currentHL;
    public int Power, Endurance, Agility, Chance;

    [Header("Battle Stages")]
    public int attackStage, defenseStage, speedStage, luckStage;
    public List<StatusEffect> activeStatuses = new List<StatusEffect>();

    public void Initialize(EnemyData info, bool playerSide)
    {
        data = info; isPlayer = playerSide;
        if (isPlayer && saveState != null)
        {
            Power = saveState.power;
            Endurance = saveState.endurance;
            Agility = saveState.agility;
            Chance = saveState.chance;
            currentHL = saveState.currentHL;
            if (currentHL <= 0) currentHL = GetMaxHL();
        } else
        {
            Power = data.basePower;
            Endurance = data.baseEndurance;
            Agility = data.baseAgility;
            Chance = data.baseChance;
            currentHL = GetMaxHL();
        }
        ResetStages();
    }

    #region Health & Mitigation Logic
    public int GetMaxHL() => 100 + (level * 10);

    public void TakeDamage(int rawDmg)
    {
        int mitigation = GetEffectiveDefense() / 2;
        int finalDmg = Mathf.Max(1, rawDmg - mitigation);

        currentHL = Mathf.Max(0, currentHL - finalDmg);
        Debug.Log($"{data.characterName} took {finalDmg} damage! (Mitigated {mitigation})");
    }
    #endregion

    #region BattleCalculationMath
    public void ResetStages()
    {
        attackStage = 0;
        defenseStage = 0;
        speedStage = 0;
        luckStage = 0;
    }

    private float GetStageMult(int stage)
    {
        int index = Mathf.Clamp(stage, -2, 2) + 2;
        float[] mults = { .5f, .75f, 1.0f, 1.5f, 2.0f };
        return mults[index];
    }

    public int GetEffectiveAttack() => Mathf.RoundToInt(Power * GetStageMult(attackStage));
    public int GetEffectiveDefense() => Mathf.RoundToInt(Endurance * GetStageMult(defenseStage));
    public int GetEffectiveSpeed() => Mathf.RoundToInt(Agility * GetStageMult(speedStage));
    public int GetEffectiveLuck() => Mathf.RoundToInt(Chance * GetStageMult(luckStage));
    #endregion

    public void SyncToSave()
    {
        if (isPlayer && saveState != null)
        {
            saveState.currentHL = currentHL;
        }
    }
}
