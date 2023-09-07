using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderCheck : MonoBehaviour
{
    public bool detected = false;

    [SerializeField] float detectRadius = 0.05f;
    [SerializeField] LayerMask detectableLayer;
    // Automatically call animation while detecting object 
    [SerializeField] Animator callAnimationSystem = null;
    [SerializeField] string animationValueName = "";
    [SerializeField] bool oppositeResult = false;

    // When physic engine is updaing
    private void FixedUpdate()
    {
        // Having a sphere immediately and to detect what does it touch
        Collider[] detectedColliderList =  Physics.OverlapSphere(this.transform.position, detectRadius, detectableLayer);

        // If number of detectedColliderList greater than 0
        if (detectedColliderList.Length > 0)
        {
            // Touch something!
            detected = true;
        }
        // Otherwise
        else
        {
            // No touch anything!
            detected = false;
        }
        // If there is using animation system that need to do animation 
        if (callAnimationSystem != null) 
        {
            // If don't need the animation system that write into animation directly
            if(oppositeResult == false)
            {
                callAnimationSystem.SetBool(animationValueName, detected);
            }
            // Else output reault is oppsite by intentionally
            else
            {
                callAnimationSystem.SetBool(animationValueName, !detected);
            }
        }
    }

    // Displing assistance line in editor
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        if (detected)
        {
            Gizmos.DrawSphere(this.transform.position, detectRadius);
        }
        else
        {
            // Frame of assistance line
            Gizmos.DrawWireSphere(this.transform.position, detectRadius);
        } 
    }
}
