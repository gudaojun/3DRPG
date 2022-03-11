using UnityEngine;

public class ActionButtion : MonoBehaviour
{
    public KeyCode customKey;

    private SlotHolder currentHolder;
    private void Awake()
    {
        currentHolder = GetComponent<SlotHolder>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(customKey) && currentHolder.itemUI.GetItem())
        {
            currentHolder.UseItem();
        }
    }
}
