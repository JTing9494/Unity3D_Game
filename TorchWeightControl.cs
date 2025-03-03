using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchWeightControl : MonoBehaviour
{
    [SerializeField] IKController ikController = null;
    public bool shouldRaiseHand = false;

    public void SetShouldRaiseHand(bool value) 
    {
        shouldRaiseHand = value;
    }

    float weightTransition = 0f;
    private void Update()
    {
        if (shouldRaiseHand)
        {
            weightTransition = Mathf.Lerp(weightTransition, 1f, Time.deltaTime * 2f);
        }
        else
        {
            weightTransition = Mathf.Lerp(weightTransition, 0f, Time.deltaTime * 2f);
        }

        ikController.rightHandWeight = weightTransition;
    }
}
