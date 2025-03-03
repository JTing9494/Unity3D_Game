using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public void Start()
    {
        Refresh();
        SaveManager.instance.OnItemChangeEvent += Refresh; 
    }

    private void OnDisable() // Before being deleted or disabled
    {
        SaveManager.instance.OnItemChangeEvent -= Refresh;
    }

    [SerializeField] GameObject itemTemplate = null;
    [SerializeField] RectTransform background = null;
    List<GameObject> garbageBin = new List<GameObject>();

    void Refresh() 
    {
        // Clear previously generated items
        for (int i = 0; i < garbageBin.Count; i++)
        {
            // Destroy objects in the list
            Destroy(garbageBin[i]);
        }
        // Clear the list
        garbageBin.Clear();
        // Enable the template before copying
        itemTemplate.SetActive(true);

        // Duplicate the template based on the number of items the player has
        // The length of the item list in the save system's player data
        // Count represents the length of the list in List arrays
        for (int i = 0; i < SaveManager.instance.playerData.itemList.Count; i++)
        {
            // Instantiate an item template and add it to the background as a child object
            GameObject currentItem = Instantiate(itemTemplate, background);
            // Write data to the duplicated item
            currentItem.GetComponent<ItemDisplay>().SetContent(SaveManager.instance.playerData.itemList[i]);
            // Add it to the garbage bin for future disposal
            garbageBin.Add(currentItem);
        }
        // Disable the template to avoid displaying it
        itemTemplate.SetActive(false);
    }
}
