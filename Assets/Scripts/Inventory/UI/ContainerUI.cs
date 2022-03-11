using System.Collections.Generic;
using UnityEngine;
public class ContainerUI : MonoBehaviour
{
    public List<SlotHolder> slotHolders;
    /// <summary>
    /// ˢ��ע������UI
    /// </summary>
    public void RefreshUI()
    {
        for (int i = 0; i < slotHolders.Count; i++)
        {
            slotHolders[i].itemUI.Index = i;
            slotHolders[i].UpdateItem();
        }
    }

}
