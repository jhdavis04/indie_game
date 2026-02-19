using UnityEngine; 
using UnityEngine.UI;
public class MoveTooltip : MonoBehaviour
{
    public static MoveTooltip Instance;
    public Text nameText, detailText, flavorText;

    private void Awake() => Instance = this;

    public void Show(MoveData move)
    {
        gameObject.SetActive(true);
        nameText.text = move.moveName;
        detailText.text = $"Type: {move.moveElement}\nScales with: {move.scalingStat}";
        flavorText.text = move.description;
    }
    public void Hide() => gameObject.SetActive(false);
    private void Update() => transform.position = Input.mousePosition + new Vector3(15, -15, 0);
}