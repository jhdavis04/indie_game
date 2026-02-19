using UnityEngine;
using UnityEngine.UI;

public class SkillTreeConnector : MonoBehaviour
{
    [Header("Visual Settings")]
    public Image lineImage;
    public Color lockedColor = new Color(0.2f, 0.2f, 0.2f, 1f);
    public Color unlockedColor = Color.cyan;

    [Header("Connection Targets")]
    public RectTransform startNode;
    public RectTransform endNode;

    public void UpdateLine(CharacterSaveData character)
    {
        if (startNode == null || endNode == null || lineImage == null) return;

        var startButton = startNode.GetComponent<SkillNodeButton>();
        bool isPathActive = false;

        if (startButton != null && character != null)
        {
            isPathActive = character.purchasedSkillNodeIDs.Contains(startButton.nodeData.name);
        }

        lineImage.color = isPathActive ? unlockedColor : lockedColor;

        PositionLine();
    }

    private void PositionLine()
    {
        Vector2 startPos = startNode.anchoredPosition;
        Vector2 endPos = endNode.anchoredPosition;

        Vector2 midPoint = (startPos + endPos) / 2f;
        transform.localPosition = midPoint;

        Vector2 direction = endPos - startPos;
        float distance = direction.magnitude;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        
        RectTransform rect = GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(distance, 5f);
        rect.localRotation = Quaternion.Euler(0, 0, angle);
    }
}