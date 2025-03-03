using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LookAndSay : MonoBehaviour, IDoEStuff
{
    [SerializeField] SayStuff descriptionText = null;
    [SerializeField] UnityEvent onEvent = null;

    public void DoE()
    {
        SaySystem.instance.StartSay(descriptionText);
        onEvent.Invoke();
    }
}
