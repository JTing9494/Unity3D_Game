using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemDisplay : MonoBehaviour
{
    [SerializeField] Image icon = null;
    [SerializeField] Text nameText = null;

    /// <summary>Set the content to be displayed/// </summary>
    public void SetContent(Goods item)
    {
        // Get detailed information from the database using ID
        ItemData details = ItemManager.Instance.GetItemByID(item.id);
        // Display the icon
        icon.sprite = details.icon;
        // If there is only one item, display only the name
        if (item.count == 1)
        {
            nameText.text = details.name;
        }
        else
        {
            nameText.text = details.name + " x " + item.count;
        }
    }
}
