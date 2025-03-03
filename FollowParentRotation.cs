using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowParentRotation : MonoBehaviour
{
    public bool shouldFollow = false;
    Quaternion myRotation = Quaternion.identity;
    [SerializeField] float followSpeed = 10f;

    private void Start()
    {
        // Initialize my rotation to the parent's rotation at the start
        myRotation = this.transform.parent.rotation;
    }

    private void Update()
    {
        if (shouldFollow)
        {
            myRotation = Quaternion.Lerp(myRotation, this.transform.parent.rotation, Time.deltaTime * followSpeed);
        }
        // Set rotation to myRotation
        this.transform.rotation = myRotation;
    }
}
