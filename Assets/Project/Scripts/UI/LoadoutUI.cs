using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class LoadoutUI : MonoBehaviour
{
    [Header("Global Connection")]
    public PlayerState globalState;

    [Header("Active Context")]
    public CharacterSaveData activeCharacter;

    [Header("UI Containers")]
    public Transform equippedSlotsParent;
    public Transform unlockedPoolParent;
    public GameObject moveButtonPrefab;

    [Header("Character Info")]
    public Text activeHeroNameText;

    private int selectedSlotIndex = -1;

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
            selectedSlotIndex = -1; 
            RefreshUI();
        }
    }

    public void RefreshUI()
    {
        if (activeCharacter == null) return;
        activeHeroNameText.text = $"{activeCharacter.characterName}'s Loadout";

        foreach (Transform child in equippedSlotsParent) Destroy(child.gameObject);
        foreach (Transform child in unlockedPoolParent) Destroy(child.gameObject);

        for (int i = 0; i < 6; i++)
        {
            int index = i;
            GameObject go = Instantiate(moveButtonPrefab, equippedSlotsParent);
            
            MoveData move = (index < activeCharacter.equippedMoves.Count) ? activeCharacter.equippedMoves[index] : null;

            go.GetComponentInChildren<Text>().text = (move != null) ? move.moveName : "---";
            
            if (selectedSlotIndex == index) go.GetComponent<Image>().color = Color.yellow;

            go.GetComponent<Button>().onClick.AddListener(() => {
                selectedSlotIndex = index;
                RefreshUI();
            });
        }

        foreach (MoveData move in activeCharacter.unlockedPool)
        {
            GameObject go = Instantiate(moveButtonPrefab, unlockedPoolParent);
            go.GetComponentInChildren<Text>().text = move.moveName;

            if (go.TryGetComponent<TooltipTrigger>(out var trigger))
                trigger.moveData = move;

            go.GetComponent<Button>().onClick.AddListener(() => {
                if (selectedSlotIndex != -1)
                {
                    activeCharacter.EquipMove(move, selectedSlotIndex);
                    selectedSlotIndex = -1;
                    RefreshUI();
                }
            });
        }
    }
}