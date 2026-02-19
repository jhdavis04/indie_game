using UnityEngine;
using System.Collections.Generic;

public class BaseCharacter : MonoBehaviour 
{
    [Header("Identity")]
    public string characterName;
    public bool isPlayer;

    [Header("Data Links")]
    public EnemyData enemyData;         
    public CharacterSaveData characterSave;

    [Header("Live Battle Stats")]
    public int level;
    public int currentHL; // Changed from currentHP
    public int currentCL; // Changed from currentMP
    
    public int Power, Endurance, Agility, Chance;

    [Header("Battle Modifiers (Stages)")]
    public int attackStage, defenseStage, speedStage, luckStage;
    public List<StatusEffect> activeStatuses = new List<StatusEffect>();

    public void Initialize(bool playerSide, CharacterSaveData partyData = null, EnemyData eData = null)
    {
        isPlayer = playerSide;

        if (isPlayer && partyData != null)
        {
            characterSave = partyData;
            characterName = partyData.characterName;
            
            Power = partyData.power;
            Endurance = partyData.endurance;
            Agility = partyData.agility;
            Chance = partyData.chance;
            level = partyData.level;

            // Matching names here
            currentHL = partyData.currentHL;
            currentCL = partyData.currentCL;
        } 
        else if (eData != null)
        {
            enemyData = eData;
            characterName = eData.characterName;
            
            Power = eData.basePower;
            Endurance = eData.baseEndurance;
            Agility = eData.baseAgility;
            Chance = eData.baseChance;
            level = eData.level;

            currentHL = GetMaxHL(); // Updated to HL
            currentCL = GetMaxCL(); // Updated to CL
        }

        ResetStages();
    }

    #region Health & Charge Logic
    // Renamed these to HL and CL for consistency
    public int GetMaxHL() => 100 + (Endurance * 10) + (level * 5);
    public int GetMaxCL() => 20 + (level * 2) + (Chance * 5);

    public void TakeDamage(int rawDmg)
    {
        int mitigation = GetEffectiveDefense() / 2;
        int finalDmg = Mathf.Max(1, rawDmg - mitigation);

        currentHL = Mathf.Max(0, currentHL - finalDmg); // Updated to HL
        Debug.Log($"{characterName} took {finalDmg} damage! (Mitigated {mitigation})");
    }
    #endregion

    #region Stat Stage Math
    public void ResetStages()
    {
        attackStage = 0; defenseStage = 0; speedStage = 0; luckStage = 0;
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
        if (isPlayer && characterSave != null)
        {
            // Now currentHL and currentCL are recognized!
            characterSave.currentHL = Mathf.Clamp(currentHL, 0, GetMaxHL());
            characterSave.currentCL = Mathf.Clamp(currentCL, 0, GetMaxCL());
            
            Debug.Log($"{characterSave.characterName} synced to save file.");
        }
    }
}