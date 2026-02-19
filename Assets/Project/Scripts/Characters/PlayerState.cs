using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "PlayerState", menuName = "RPG/Player State")]
public class PlayerState : ScriptableObject
{
    [Header("Shared Economy")]
    public int gold;
    public int tokens;

    [Header("The Party")]
    public List<CharacterSaveData> allCharacters;
    public List<CharacterSaveData> activeParty;

    [Header("Shared Inventory")]
    public List<ItemData> inventory;

    [Header("Global Progression")]
    public int storyChapter;
    public List<string> completedQuestIDs;

    #region Helper Methods
    public CharacterSaveData GetCharacter(string name)
    {
        return allCharacters.Find(c => c.characterName == name);
    }

    public void RecruitCharacter(CharacterSaveData newMember)
    {
        if (!allCharacters.Contains(newMember))
        {
            allCharacters.Add(newMember);
        }
    }
    #endregion

}