using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowEmptyCan : MonoBehaviour
{
    [SerializeField] Transform throwPoint = null;
    [SerializeField] GameObject can = null;
    [SerializeField] float throwForce = 10f;

    public void Throw()
    {
        GameObject temp = Instantiate(can, throwPoint.position, Quaternion.Euler(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f)));
        Rigidbody canRigidbody = temp.GetComponent<Rigidbody>();

        Vector3 direction = targetPoint - temp.transform.position;
        direction = direction.normalized;
        SaveManager.instance.playerData.ConsumeItem(3);
        temp.GetComponent<AttractAttention>().hasBeenThrown = true;

        canRigidbody.velocity = direction * throwForce;
    }

    [SerializeField] Transform cameraTransform = null;
    RaycastHit raycastInfo;
    [SerializeField] LayerMask raycastLayer;
    Vector3 targetPoint;

    public void FixedUpdate()
    {
        bool hasHit = Physics.Raycast(cameraTransform.position, cameraTransform.forward, out raycastInfo, 999f, raycastLayer);
        if (hasHit)
        {
            targetPoint = raycastInfo.point;
        }
    }
}
