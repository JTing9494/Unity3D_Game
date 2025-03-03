using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class SaveManager
{
    static public SaveManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new SaveManager();
            }
            return _instance;
        }
    }

    static SaveManager _instance = null;

    public PlayerData playerData = new PlayerData();

    public void Load()
    {
        string json = PlayerPrefs.GetString("DATA", "");
        Debug.Log("Read JSON : " + json);
        if (json == "" || json == string.Empty)
        {
            playerData = new PlayerData();
            playerData.itemList = new List<Goods>();
            playerData.maxPw = 7f;
            playerData.pw = 7f;
            playerData.hp = 3;

            playerData.saveLevelName = "";
            playerData.saveHP = 3;
            playerData.saveItemList = new List<Goods>();
            Debug.Log("No data, creating file manually");
        }
        else
        {
            playerData = JsonUtility.FromJson<PlayerData>(json);
            Debug.Log("Data found, creating file from data");
        }

    }

    public void Save()
    {
        playerData.saveHP = playerData.hp;
        playerData.saveItemList = playerData.itemList;
        playerData.saveLevelName = SceneManager.GetActiveScene().name;

        string json = JsonUtility.ToJson(playerData);
        Debug.Log("Save JSON : " + json);

        PlayerPrefs.SetString("DATA", json);
    }

    // Define Action as subscribing
    public Action itemChangeEvent = null;
    public Action hpChangeEvent = null;
    public Action staminaChangeEvent = null;
}

/// <summary>All player data</summary>
[System.Serializable]
public struct PlayerData
{
    /// <summary>Level name</summary>
    [SerializeField] public string saveLevelName;
    /// <summary>HP</summary>
    [SerializeField] public int saveHP;
    /// <summary>Items</summary>
    [SerializeField] public List<Goods> saveItemList;

    #region Item System
    /// <summary>List of items held</summary>
    [SerializeField] public List<Goods> itemList;

    public void AddItem(int id)
    {
        // Check if there are items of the same type
        // Check each current item one by one
        for (int i = 0; i < itemList.Count; i++)
        {
            // If the same ID is found, it means this item already exists
            if (itemList[i].id == id)
            {
                // Increase the quantity of the current item by one
                Goods duplicateData = itemList[i];
                duplicateData.count += 1;
                itemList[i] = duplicateData;
                if (SaveManager.instance.itemChangeEvent != null)
                {
                    SaveManager.instance.itemChangeEvent.Invoke();
                }
                // Since stacking succeeded, there's no need to add the item again. Terminate the program here.
                return;
            }
        }

        // Directly add the item only if it wasn't stacked
        // Create new Goods data
        Goods newGoods = new Goods();
        newGoods.id = id;
        newGoods.count = 1;
        // Add this data to the item list
        itemList.Add(newGoods);
        if (SaveManager.instance.itemChangeEvent != null)
        {
            SaveManager.instance.itemChangeEvent.Invoke();
        }
    }

    public bool HasItem(int id)
    {
        // Check each item one by one
        for (int i = 0; i < itemList.Count; i++)
        {
            if (itemList[i].id == id)
            {
                return true;
            }
        }
        return false;
    }

    public int ItemCount(int id)
    {
        // Loop through each item
        for (int i = 0; i < itemList.Count; i++)
        {
            // Find the desired item
            if (itemList[i].id == id)
            {
                // Return the quantity of this item and terminate the program
                return itemList[i].count;
            }
        }
        // If the loop completes without termination, it means this item is not present. Return 0.
        return 0;
    }

    /// <summary>Consume multiple items</summary>
    public void ConsumeItem(int id, int count)
    {
        for (int i = 0; i < count; i++)
        {
            ConsumeItem(id);
        }
    }

    public void ConsumeItem(int id)
    {
        // Loop to find this item
        for (int i = 0; i < itemList.Count; i++)
        {
            // Found the item
            if (itemList[i].id == id)
            {
                // If the quantity is sufficient, only reduce by one unit
                if (itemList[i].count >= 2)
                {
                    Goods duplicateData = itemList[i];
                    duplicateData.count -= 1;
                    itemList[i] = duplicateData;

                    if (SaveManager.instance.itemChangeEvent != null)
                    {
                        SaveManager.instance.itemChangeEvent.Invoke();
                    }
                    return;
                }
                // If the quantity is insufficient, remove this item
                else
                {
                    // Remove this entry
                    itemList.RemoveAt(i);

                    if (SaveManager.instance.itemChangeEvent != null)
                    {
                        SaveManager.instance.itemChangeEvent.Invoke();
                    }
                    return;
                }
            }
        }
    }
    #endregion

    #region HP and Stamina
    public int hp
    {
        get { return _hp; }
        set
        {
            _hp = value;
            if (SaveManager.instance.hpChangeEvent != null)
                SaveManager.instance.hpChangeEvent.Invoke();
        }
    }
    [SerializeField] int _hp;

    public float pw
    {
        get { return _pw; }
        set
        {
            _pw = value;
            if (SaveManager.instance.staminaChangeEvent != null)
                SaveManager.instance.staminaChangeEvent.Invoke();
        }
    }
    [SerializeField] float _pw;
    [SerializeField] public float maxPw;
    #endregion
}

[System.Serializable]
/// <summary>Item</summary>
public struct Goods
{
    [SerializeField] public int id;
    [SerializeField] public int count;
}
