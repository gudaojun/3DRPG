using UnityEngine;
using UnityEngine.EventSystems;
public class ShowTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private ItemUI currentItemUI;

    private void Awake()
    {
        currentItemUI = GetComponent<ItemUI>();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        QuestUI.Instance.toolTip.gameObject.SetActive(true);
        QuestUI.Instance.toolTip.SetUpTooltip(currentItemUI.currentItemData);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        QuestUI.Instance.toolTip.gameObject.SetActive(false);

    }
}
