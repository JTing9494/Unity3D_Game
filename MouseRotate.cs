using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseRotate : MonoBehaviour
{
    [SerializeField] Transform horizontalAxis = null;
    [SerializeField] Transform verticalAxis = null;
    float mouseX = 0f;
    float mouseY = 0f;
    [Range(0.01f, 10f)] public float mouseSensitivity = 1f;

    private void Start()
    {
        // Initialize by locking and hiding the mouse cursor
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        // Accumulate mouse horizontal movement
        // mouseX = mouseX + Input.GetAxis("Mouse X") * mouseSensitivity;
        // mouseY = mouseY + Input.GetAxis("Mouse Y") * -1f * mouseSensitivity;

        // If not in dialogue and not paused
        if (SaySystem.instance.isPlay == false && OptionWindow.instance.isOpen == false)
        {
            // Update only when not in dialogue
            mouseX = mouseX + Input.GetAxis("Mouse X") * mouseSensitivity;
            mouseY = mouseY + Input.GetAxis("Mouse Y") * -1f * mouseSensitivity;
        }

        // Create a Quaternion to replace the rotation of the horizontal axis
        Quaternion horizontalRotation = Quaternion.Euler(0f, mouseX, 0f);
        // Directly replace the value of the horizontal axis
        horizontalAxis.rotation = horizontalRotation;

        // Clamp mouseY range between -85 to 85
        mouseY = Mathf.Clamp(mouseY, -85f, 85f);

        Quaternion verticalRotation = Quaternion.Euler(mouseY, 0f, 0f);
        // The vertical axis is a child object, so use local coordinate system
        verticalAxis.localRotation = verticalRotation;
    }
}
