using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKController : MonoBehaviour
{
    // IK for Head
    [SerializeField] Transform lookAtTarget = null;
    [SerializeField] Animator animator = null;

    [SerializeField] Transform rightHandIKTarget = null;
    [SerializeField] Transform rightHandIKHintTarget = null;
    [Range(0f, 1f)] public float rightHandWeight = 1f;
    
    // Similar to Update, dedicated to setting IK parameters
    private void OnAnimatorIK(int layerIndex)
    {
        // Control the head's look direction and weight
        animator.SetLookAtPosition(lookAtTarget.position);
        animator.SetLookAtWeight(1f, 0.3f, 1f, 1f, 0.5f);

        // Set the right hand's position and rotation
        animator.SetIKPosition(AvatarIKGoal.RightHand, rightHandIKTarget.position);
        animator.SetIKRotation(AvatarIKGoal.RightHand, rightHandIKTarget.rotation);
        // Set the right hand's hint position
        animator.SetIKHintPosition(AvatarIKHint.RightElbow, rightHandIKHintTarget.position);
        // Set right hand weights
        animator.SetIKPositionWeight(AvatarIKGoal.RightHand, rightHandWeight);
        animator.SetIKRotationWeight(AvatarIKGoal.RightHand, rightHandWeight);
        animator.SetIKHintPositionWeight(AvatarIKHint.RightElbow, rightHandWeight);
    }
}
