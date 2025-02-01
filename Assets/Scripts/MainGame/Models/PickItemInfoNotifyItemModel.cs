using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PickItemInfoNotifyItemModel : MonoBehaviour
{
    public ItemData ItemData;
    public TMP_Text Info;
    public Image Icon;

    public void SetItemData(ItemData itemData) 
    {
        this.ItemData = itemData;
        Info.text = String.Empty;
        if (itemData.Value > 0)
        {
            Info.color = Color.green;
            Info.text = "+";
        } 
        else
        {
            Info.color = Color.red;
        }
        Info.text += this.ItemData.Value;
        Icon.sprite = this.ItemData.Icon;
    }
}
