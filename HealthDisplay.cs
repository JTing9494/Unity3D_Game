using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthDisplay : MonoBehaviour
{
    void Start()
    {
        // Refresh content on health change and initialization
        SaveManager.instance.OnHealthChangeEvent += RefreshContent;
        RefreshContent();
    }

    private void OnDisable()
    {
        SaveManager.instance.OnHealthChangeEvent -= RefreshContent;
    }

    [SerializeField] GameObject heart = null;
    [SerializeField] RectTransform uiBackground = null;

    void RefreshContent()
    {
        // Loop through the number of child objects
        for (int i = 0; i < uiBackground.transform.childCount; i++)
        {
            // Delete child objects
            Destroy(uiBackground.transform.GetChild(i).gameObject);
        }
        // Generate the corresponding number of hearts based on health
        for (int i = 0; i < SaveManager.instance.playerData.hp; i++) 
        {
            Instantiate(heart, uiBackground);
        }
    }
}
