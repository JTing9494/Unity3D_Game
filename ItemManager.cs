using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager
{
    private readonly static object padlock = new object();
    private static ItemManager _instance = null;
    public static ItemManager Instance
    {
        get
        {
            lock (padlock)
            {
                if (_instance == null)
                    _instance = new ItemManager();
                return _instance;
            }
        }
    }


    List<ItemData> itemList = new List<ItemData>();
    // Load files from the folder during initialization
    public void Load() 
    {
        // Load item data from the Resources folder
        // Create a new List array and store the loaded data in it.
        itemList = new List<ItemData>(Resources.LoadAll<ItemData>(""));
    }

    public ItemData GetItemByID(int id)
    {
        // Check each item
        for (int i = 0; i < itemList.Count; i++)
        {
            // If the library has an item with the same ID as the customer, give it to them
            if (itemList[i].id == id)
            {
                // Return the found book
                return itemList[i];
            }
        }
        // If the loop ends without returning, it means there is no item available, log an error
        Debug.LogError("No item found with ID: " + id);
        return new ItemData();
    }
}
