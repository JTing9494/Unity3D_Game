using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IDoEStuff
{
    [SerializeField] Animator animator = null;
    [SerializeField] SayStuff noKeyMessage = null;
    [SerializeField] int keyID = 0;

    public void DoE()
    {
        if (SaveManager.instance.playerData.HasItem(keyID) == false)
        {
            SaySystem.instance.StartSay(noKeyMessage);
            // Stop the function
            return;
        }

        // Get the current state of the door's animation
        bool isOpen = animator.GetBool("isOpen");
        isOpen = !isOpen;
        // Set the state back to the animator
        animator.SetBool("isOpen", isOpen);
    }
}
