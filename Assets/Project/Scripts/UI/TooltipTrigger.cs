using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public MoveData moveData;
    public void OnPointerEnter(PointerEventData data) => MoveTooltip.Instance.Show(moveData);
    public void OnPointerExit(PointerEventData data) => MoveTooltip.Instance.Hide();
}