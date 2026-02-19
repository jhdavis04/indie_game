using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SkillTreeUI : MonoBehaviour
{
    [Header("Global Data")]
    public PlayerState globalState;
    
    [Header("Active Context")]
    public CharacterSaveData activeCharacter; 

    [Header("UI References")]
    public Text characterNameText;
    public Text tokenCountText;
    public Transform nodeContainer; 
    
    void OnEnable()
    {
        if (activeCharacter == null && globalState.allCharacters.Count > 0)
            activeCharacter = globalState.allCharacters[0];
            
        RefreshUI();
    }

    public void RefreshUI()
    {
        if (activeCharacter == null) return;

        characterNameText.text = activeCharacter.characterName;
        tokenCountText.text = $"Tokens: {activeCharacter.skillTokens}";

        foreach (SkillNodeButton btn in nodeContainer.GetComponentsInChildren<SkillNodeButton>())
        {
            btn.UpdateVisuals(activeCharacter);
        }
    }

    #region Character Selection
    public void SelectCharacter(int index)
    {
        if (index >= 0 && index < globalState.allCharacters.Count)
        {
            activeCharacter = globalState.allCharacters[index];
            RefreshUI();
        }
    }
    #endregion

    #region Purchase Logic
    public void TryPurchaseNode(SkillNode node)
    {
        if (activeCharacter == null) return;
        if (activeCharacter.purchasedSkillNodeIDs.Contains(node.name)) return;
        if (!CanUnlock(node)) return;

        if (activeCharacter.skillTokens >= node.tokenCost)
        {
            activeCharacter.skillTokens -= node.tokenCost;
            activeCharacter.purchasedSkillNodeIDs.Add(node.name);
            if (node.moveReward != null && !activeCharacter.unlockedPool.Contains(node.moveReward))
            {
                activeCharacter.unlockedPool.Add(node.moveReward);
            }

            RefreshUI();
        }
    }
    private bool CanUnlock(SkillNode node)
    {
        if (activeCharacter.level < node.requiredLevel) return false;
        foreach (SkillNode req in node.requiredNodes)
        {
            if (!activeCharacter.purchasedSkillNodeIDs.Contains(req.name))
                return false;
        }
        return true;
    }
    #endregion

    
}