using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// ScriptableObject = Custom file format

[CreateAssetMenu(fileName = "ItemName", menuName = "Create Item")]
public class ItemData : ScriptableObject
{
    public int id;
    public string description;
    public Sprite icon;
}
