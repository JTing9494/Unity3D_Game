using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Detector
public class ColliderCheck : MonoBehaviour
{
    public bool yes = false;

    [SerializeField] float detectionRadius = 0.05f;
    [SerializeField] LayerMask detectableLayers;
    // Automatically call animation when something is detected
    [SerializeField] Animator callAnimationSystem = null;
    [SerializeField] string animationParameterName = "";
    [SerializeField] bool shouldInvertResult = false;

    // Physics engine update
    private void FixedUpdate()
    {
        // Instantly create a sphere and detect what it collides with
        Collider[] detectedColliders = Physics.OverlapSphere(this.transform.position, detectionRadius, detectableLayers);

        // If the number of colliders detected is greater than 0
        if (detectedColliders.Length > 0)
        {
            // Define that something was detected
            yes = true;
        }
        // Otherwise
        else
        {
            // Nothing was detected
            yes = false;
        }

        if (callAnimationSystem != null)
        {
            // If inversion of results is not required, directly pass "yes" to the animation
            if (shouldInvertResult == false)
            {
                callAnimationSystem.SetBool(animationParameterName, yes);
            }
            // Otherwise, output the inverted result
            else
            {
                callAnimationSystem.SetBool(animationParameterName, !yes);
            }
        }
    }

    // Display auxiliary lines in the editor
    private void OnDrawGizmos()
    {
        // Set the color of auxiliary lines
        Gizmos.color = Color.yellow;

        if (yes)
        {
            // Draw a solid sphere according to the coordinates and radius
            Gizmos.DrawSphere(this.transform.position, detectionRadius);
        }
        else
        {
            // Draw a wireframe sphere according to the coordinates and radius
            Gizmos.DrawWireSphere(this.transform.position, detectionRadius);
        }
    }
}
