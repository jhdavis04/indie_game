using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewCharacterData", menuName = "RPG/Character Save Data")]
public class CharacterSaveData : ScriptableObject
{
    public string characterName;
    public string codename;

    [Header("Vitality (Live State)")]
    public int currentHL;
    public int currentCL;

    public int MaxHL => 100 + (endurance * 10) + (level * 5);
    public int MaxCL => 20 + (level * 2) + (chance * 5);

    [Header("Leveling & Stats")]
    public int level = 1;
    public int currentXP;
    
    [Range(1, 99)] public int power = 10;
    [Range(1, 99)] public int endurance = 10;
    [Range(1, 99)] public int agility = 10;
    [Range(1, 99)] public int chance = 10;

    [Header("Currencies")]
    public int statPointsAvailable;
    public int skillTokens;
    
    [Header("Move Collection (Personal)")]
    public List<MoveData> equippedMoves = new List<MoveData>(new MoveData[6]);
    public List<MoveData> unlockedPool = new List<MoveData>();

    [Header("Progression")]
    public List<string> purchasedSkillNodeIDs = new List<string>();

    #region Helper Methods
    public void EquipMove(MoveData move, int slotIndex)
    {
        if (slotIndex >= 0 && slotIndex < 6)
        {
            equippedMoves[slotIndex] = move;
        }
    }

    public void FullHeal()
    {
        currentHL = MaxHL;
        currentCL = MaxCL;
    }

    public void ValidateLimits()
    {
        currentHL = Mathf.Clamp(currentHL, 0, MaxHL);
        currentCL = Mathf.Clamp(currentCL, 0, MaxCL);
    }
    #endregion
}