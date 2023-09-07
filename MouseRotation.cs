using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseRotation : MonoBehaviour
{
    [SerializeField] Transform horizontalRotateAxis = null;
    [SerializeField] Transform verticalRotateAxis = null;
    float mouseX = 0f;
    float mouseY = 0f;
    [Range(0.01f, 10f)] public float mouseSensitivity = 1f;

    public void Start()
    {
        // Initialized to lock mouse and hide cursor
        Cursor.lockState = CursorLockMode.Locked;

    }

    private void Update()
    {
        mouseX = mouseX + Input.GetAxis("Mouse X") * mouseSensitivity;
        mouseY = mouseY + Input.GetAxis("Mouse Y") * mouseSensitivity  * -1f;

        // Create a Quaternion
        Quaternion horizontalRotateValue = Quaternion.Euler(0f, mouseX, 0f);
        // Substitute the value of horizontalRotateValue
        horizontalRotateAxis.rotation = horizontalRotateValue;

        // Restric mouseY between -85 and 85
        mouseY = Mathf.Clamp(mouseY, -85f, 85f);


        // Substitute the value of verticalRotateValue
        Quaternion verticalRotateValue = Quaternion.Euler(mouseY, 0f, 0f);
        // Using localRotation is because that is a child object so that it has to be local coordinate
        verticalRotateAxis.localRotation = verticalRotateValue;
    }
}
