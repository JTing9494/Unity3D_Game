using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKControl : MonoBehaviour
{
    [SerializeField] Transform ViewPoint = null;
    [SerializeField] Animator AnimationSystem = null;

    // OnAnimatorIK is similar to Update which is for writting data into IK
    private void OnAnimatorIK(int layerIndex)
    {
        // Setting the way of view
        AnimationSystem.SetLookAtPosition(ViewPoint.position);
        // Setting each part of weight
        AnimationSystem.SetLookAtWeight(1f, 0.3f, 1f, 1f, 0.4f);
    }
}
