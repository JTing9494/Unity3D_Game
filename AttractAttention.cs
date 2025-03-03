using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AttractAttention : MonoBehaviour
{
    public bool hasBeenThrown = true;
    [SerializeField] float impactRadius = 999f;
    [SerializeField] float maxImpactForce = 15f;
    [SerializeField] float minImpactForce = 5f;
    [SerializeField] AudioSource[] collisionSounds = null;

    private void OnCollisionEnter(Collision collision)
    {
        float collisionForce = collision.relativeVelocity.magnitude;
        // If the collision force is greater than the minimum impact force
        if (collisionForce > minImpactForce)
        {
            // Play sound, the greater the force, the louder the sound, clamped between 0 and 1
            int soundToPlay = Random.Range(0, collisionSounds.Length);
            collisionSounds[soundToPlay].Play();
            collisionSounds[soundToPlay].volume = Mathf.Clamp01(collisionForce / maxImpactForce);
        }
        if (hasBeenThrown)
        {
            hasBeenThrown = false;
            Collider[] objectsInRange = Physics.OverlapSphere(this.transform.position, impactRadius);
            for (int i = 0; i < objectsInRange.Length; i++)
            {
                // Send message "AttractAttention" to all units in range, without requiring a receiver
                objectsInRange[i].gameObject.SendMessage("AttractAttention", this.transform.position, SendMessageOptions.DontRequireReceiver);
            }
        }
    }
}
