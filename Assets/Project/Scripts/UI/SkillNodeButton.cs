using UnityEngine;
using UnityEngine.UI;

public class SkillNodeButton : MonoBehaviour
{
    [Header("Data Connection")]
    public SkillNode nodeData;       
    public SkillTreeUI uiManager;      

    [Header("Visual Components")]
    public Image iconImage;           
    public Image backgroundImage;     
    public Text costText;              

    [Header("Color Settings")]
    public Color lockedColor = Color.gray;
    public Color availableColor = Color.yellow;
    public Color purchasedColor = Color.white;

    public void UpdateVisuals(CharacterSaveData character)
    {
        if (nodeData == null) return;

        bool isPurchased = character.purchasedSkillNodeIDs.Contains(nodeData.name);
        bool canAfford = character.skillTokens >= nodeData.tokenCost;
        bool meetsLevel = character.level >= nodeData.requiredLevel;

        bool prerequisitesMet = true;
        foreach (SkillNode req in nodeData.requiredNodes)
        {
            if (!character.purchasedSkillNodeIDs.Contains(req.name))
            {
                prerequisitesMet = false;
                break;
            }
        }
        if (isPurchased)
        {
            backgroundImage.color = purchasedColor;
            iconImage.color = Color.white;
        }
        else if (meetsLevel && prerequisitesMet)
        {
            backgroundImage.color = availableColor;
            iconImage.color = new Color(1, 1, 1, 0.6f);
        }
        else
        {
            backgroundImage.color = lockedColor;
            iconImage.color = new Color(0.2f, 0.2f, 0.2f, 0.5f);
        }

        if (costText != null) costText.text = nodeData.tokenCost.ToString();
    }

    public void OnButtonClick()
    {
        if (uiManager != null && nodeData != null)
        {
            uiManager.TryPurchaseNode(nodeData);
        }
    }
}