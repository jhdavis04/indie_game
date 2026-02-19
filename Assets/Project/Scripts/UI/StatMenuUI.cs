using UnityEngine;
using UnityEngine.UI;

public class StatMenuUI : MonoBehaviour
{
    [Header("Global Connection")]
    public PlayerState globalState;

    [Header("Active Context")]
    public CharacterSaveData activeCharacter;

    [Header("UI Text Fields")]
    public Text nameText;
    public Text levelText;
    public Text hpText;
    public Text mpText;
    public Text pointsAvailableText;

    [Header("Stat Value Texts")]
    public Text powerText;
    public Text enduranceText;
    public Text agilityText;
    public Text chanceText;

    void OnEnable() 
    {
        if (activeCharacter == null && globalState.allCharacters.Count > 0)
            activeCharacter = globalState.allCharacters[0];
            
        RefreshUI();
    }

    public void SelectCharacter(int index)
    {
        if (index >= 0 && index < globalState.allCharacters.Count)
        {
            activeCharacter = globalState.allCharacters[index];
            RefreshUI();
        }
    }

    public void RefreshUI()
    {
        if (activeCharacter == null) return;

        nameText.text = $"{activeCharacter.characterName} ({activeCharacter.codename})";
        levelText.text = $"Level: {activeCharacter.level}";
        pointsAvailableText.text = $"Stat Points: {activeCharacter.statPointsAvailable}";

        hpText.text = $"HP: {activeCharacter.currentHL} / {activeCharacter.MaxHL}";
        mpText.text = $"MP: {activeCharacter.currentCL} / {activeCharacter.MaxCL}";

        powerText.text = activeCharacter.power.ToString();
        enduranceText.text = activeCharacter.endurance.ToString();
        agilityText.text = activeCharacter.agility.ToString();
        chanceText.text = activeCharacter.chance.ToString();
    }

    public void IncreaseStat(string statName)
    {
        if (activeCharacter == null || activeCharacter.statPointsAvailable <= 0) return;

        bool statChanged = false;

        switch (statName.ToLower())
        {
            case "power": 
                if (activeCharacter.power < 99) { activeCharacter.power++; statChanged = true; } 
                break;
            case "endurance": 
                if (activeCharacter.endurance < 99) { activeCharacter.endurance++; statChanged = true; } 
                break;
            case "agility": 
                if (activeCharacter.agility < 99) { activeCharacter.agility++; statChanged = true; } 
                break;
            case "chance": 
                if (activeCharacter.chance < 99) { activeCharacter.chance++; statChanged = true; } 
                break;
        }

        if (statChanged)
        {
            activeCharacter.statPointsAvailable--;
            RefreshUI();
        }
    }
}