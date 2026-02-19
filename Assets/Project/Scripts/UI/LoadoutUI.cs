using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class LoadoutUI : MonoBehaviour
{
    public PlayerState playerState;

    [Header("UI Containers")]
    public Transform equippedSlotsParent;
    public Transform unlockedPoolParent;
    public GameObject moveButtonPrefab;

    private int selectedSlotIndex = -1;
    void Start() => RefreshUI();

    public void RefreshUI()
    {
        foreach (Transform child in equippedSlotsParent) Destroy(child.gameObject);
        foreach (Transform child in unlockedPoolParent) Destroy(child.gameObject);

        for (int i = 0; i < 6; i++)
        {
            int index = i;
            GameObject go = Instantiate(moveButtonPrefab, equippedSlotsParent);
            MoveData move = (index < playerState.equippedMoves.Count) ? playerState.equippedMoves[index] : null;

            go.GetComponentInChildren<Text>().text = (move != null) ? move.moveName : "---";
            go.GetComponent<Button>().onClick.AddListener(() => selectedSlotIndex = index);
        }

        foreach (MoveData move in playerState.unlockedPool)
        {
            GameObject go = Instantiate(moveButtonPrefab, unlockedPoolParent);
            go.GetComponentInChildren<Text>().text = move.moveName;

            go.GetComponent<TooltipTrigger>().moveData = move;

            go.GetComponent<Button>().onClick.AddListener(() => {
                if (selectedSlotIndex != -1)
                {
                    playerState.EquipMove(move, selectedSlotIndex);
                    selectedSlotIndex = -1;
                    RefreshUI();
                }
            });
        }
    }
}