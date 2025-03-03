using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ItemSelector : MonoBehaviour
{
    [SerializeField] int targetID = 0;
    [SerializeField] GameObject targetItem = null;
    [SerializeField] UnityEvent<bool> targetEvent;

    private void Start()
    {
        Refresh();
        SaveManager.instance.OnItemChangeEvent += Refresh;
    }

    private void OnDisable()
    {
        SaveManager.instance.OnItemChangeEvent -= Refresh;
    }

    void Refresh()
    {
        if (targetItem == null)
            return;
        // If the target ID is present, display the target item
        targetItem.SetActive(SaveManager.instance.playerData.HasItem(targetID));
        targetEvent.Invoke(SaveManager.instance.playerData.HasItem(targetID));
    }
}
