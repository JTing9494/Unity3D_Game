using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>Make this object follow the target's rotation</summary>
public class RotationFollower : MonoBehaviour
{
    [SerializeField] Transform followTarget = null;
    [SerializeField] Vector2 xRange = new Vector2(-360f, 360f);
    [SerializeField] Vector2 yRange = new Vector2(-360f, 360f);
    [SerializeField] Vector2 zRange = new Vector2(-360f, 360f);
    [SerializeField] float followSpeed = 10f;

    private void Update()
    {
        // Read the target's rotation values
        float x = followTarget.localRotation.eulerAngles.x;
        float y = followTarget.localRotation.eulerAngles.y;
        float z = followTarget.localRotation.eulerAngles.z;
        // Clamp the values within the specified range
        x = Mathf.Clamp(x, xRange.x, xRange.y);
        y = Mathf.Clamp(y, yRange.x, yRange.y);
        z = Mathf.Clamp(z, zRange.x, zRange.y);
        // Set this object's rotation
        // this.transform.localRotation = Quaternion.Euler(x, y, z);
        this.transform.localRotation = Quaternion.Lerp(this.transform.localRotation, Quaternion.Euler(x, y, z), followSpeed);
    }
}
