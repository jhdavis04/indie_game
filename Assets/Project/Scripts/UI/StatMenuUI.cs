using UnityEngine;
using UnityEngine.UI;

public class StatMenuUI : MonoBehaviour
{
    [Header("Data Connection")]
    public PlayerState playerState;

    [Header("UI Text Fields")]
    public Text levelText;
    public Text hlText;
    public Text pointsAvailableText;

    [Header("Stat Value Texts")]
    public Text powerText;
    public Text enduranceText;
    public Text agilityText;
    public Text chanceText;

    void OnEnable() => RefreshUI();

    public void RefreshUI()
    {
        levelText.text = $"Level: {playerState.level}";
        pointsAvailableText.text = $"Points: {playerState.statPointsAvailable}";

        int maxHL = 100 + (playerState.level * 10);
        hlText.text = $"HL: {playerState.currentHL} / {maxHL}";

        powerText.text = playerState.power.ToString();
        enduranceText.text = playerState.endurance.ToString();
        agilityText.text = playerState.agility.ToString();
        chanceText.text = playerState.chance.ToString();
    }

    public void IncreaseStat(string statName)
    {
        if (playerState.statPointsAvailable <= 0) return;

        switch (statName.ToLower())
        {
            case "power": if (playerState.power < 99) playerState.power++; break;
            case "endurance": if (playerState.endurance < 99) playerState.endurance++; break;
            case "agility": if (playerState.agility < 99) playerState.agility++; break;
            case "chance": if (playerState.chance < 99) playerState.chance++; break;
        }
        playerState.statPointsAvailable--;
        RefreshUI();
    }
}