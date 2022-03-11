using UnityEngine;
using UnityEngine.UI;
public class ItemTooltip : MonoBehaviour
{
    public Text itemName;
    public Text iteamInfo;

    private RectTransform rect;
    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }
    public void SetUpTooltip(ItemData_SO itemData_SO)
    {
        iteamInfo.text = itemData_SO.description;
        itemName.text = itemData_SO.itemName;
    }

    private void OnEnable()
    {
        UpdatePosition();
    }
    private void Update()
    {
        UpdatePosition();
    }

    /// <summary>
    /// 跟随鼠标并判断在屏幕的位置
    /// </summary>
    public void UpdatePosition()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3[] corners = new Vector3[4];

        rect.GetWorldCorners(corners);
        float width = corners[3].x - corners[0].x;
        float hight = corners[1].y - corners[0].y;
        if (mousePos.y < hight)
        {
            rect.position = mousePos + Vector3.up * hight * 0.6f;
        }
        else if (Screen.width - mousePos.x > width)
            rect.position = mousePos + Vector3.right * width * 0.6f;
        else
            rect.position = mousePos + Vector3.left * width * 0.6f;
    }
}
