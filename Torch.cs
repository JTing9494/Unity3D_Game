using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour, IDoEStuff
{
    [SerializeField] SayStuff initialDialogue = null;
    [SerializeField] SayStuff insufficientQuantityDialogue = null;
    [SerializeField] SayStuff activationSuccessDialogue = null;
    [SerializeField] int batteryID = 1;
    [SerializeField] int flashlightID = 2;
    [SerializeField] GameObject light = null;
    int interactionCount = 0;
    
    public void DoE()
    {
        if (light.activeSelf == true)
        {
            SaveManager.instance.playerData.AddItem(flashlightID);
            Destroy(this.gameObject);
            return;
        }

        if (interactionCount == 0)
        {
            SaySystem.instance.StartSay(initialDialogue);
            interactionCount++;
            return;
        }

        if (interactionCount >= 1)
        {
            // Insufficient battery quantity
            if (SaveManager.instance.playerData.GetItemQuantity(batteryID) < 3)
            {
                SaySystem.instance.StartSay(insufficientQuantityDialogue);
                return;
            }
            else
            {
                SaySystem.instance.StartSay(activationSuccessDialogue);
                SaveManager.instance.playerData.ConsumeItem(batteryID, 3);
                light.SetActive(true);
                return;
            }
        }
    }
}
