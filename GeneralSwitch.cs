using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GeneralSwitch : MonoBehaviour, IDoEStuff
{
    [SerializeField] bool isOn = false;
    [SerializeField] bool canOnlyTurnOn = false;

    void Start()
    {
        // Ensure the developer remembers to turn off the light by refreshing the switch state during initialization
        ToggleState();
    }

    public void DoE()
    {
        // If the switch is already on and can only turn on, terminate the function
        if (isOn == true && canOnlyTurnOn == true)
            return;

        isOn = !isOn;
        ToggleState();
    }

    [SerializeField] UnityEvent onTurnOn = null;
    [SerializeField] UnityEvent onTurnOff = null;
    [SerializeField] Animator animator = null;

    void ToggleState()
    {
        if (isOn == true)
        {
            onTurnOn.Invoke();
        }

        if (isOn == false)
        {
            onTurnOff.Invoke();
        }
        animator.SetBool("isOn", isOn);
    }
}
