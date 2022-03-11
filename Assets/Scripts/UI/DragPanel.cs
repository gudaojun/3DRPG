using UnityEngine;
using UnityEngine.EventSystems;
public class DragPanel : MonoBehaviour, IDragHandler, IPointerEnterHandler
{
    RectTransform rectTransform;
    Canvas canvas;
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = InventoryManager.Instance.GetComponent<Canvas>();
    }
    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.SetSiblingIndex(2);
    }
}
