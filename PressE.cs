using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressE : MonoBehaviour
{
    public bool canInteract = false;

    [SerializeField] Transform cameraTransform = null;
    [SerializeField] float interactionDistance = 2f;
    [SerializeField] LayerMask interactableLayer;
    [SerializeField] LayerMask allRaycastLayers;
    [SerializeField] GameObject interactUI;
    RaycastHit raycastHitInfo;

    // Physics engine update
    private void FixedUpdate()
    {
        float distanceToPlayer = Vector3.Distance(cameraTransform.position, cameraTransform.forward);

        bool hasHit = Physics.Raycast(cameraTransform.position, cameraTransform.forward, out raycastHitInfo, distanceToPlayer + interactionDistance, allRaycastLayers);

        // Default to not showing interact UI
        interactUI.SetActive(false);
        canInteract = false;

        // If raycast hits something and not in dialogue
        if (hasHit && SaySystem.instance.isPlay == false)
        {
            // If the layer of the hit object is in the interactable layer
            if (Aye.IsInLayerMask(raycastHitInfo.collider.gameObject.layer, interactableLayer))
            {
                interactUI.SetActive(true);
                canInteract = true;
            }
        }
    }

    private void Update()
    {
        // If 'E' key is pressed
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Press E");
            if (canInteract)
            {
                List<IDoEStuff> allUSBChannels = new List<IDoEStuff>(raycastHitInfo.collider.gameObject.GetComponents<IDoEStuff>());
                if (allUSBChannels.Count > 0)
                {
                    for (int i = 0; i < allUSBChannels.Count; i++)
                    {
                        allUSBChannels[i].DoE();
                    }
                }
                else
                {
                    Debug.LogError("The script on this object should implement IDoEStuff", raycastHitInfo.collider.gameObject);
                }
            }
        }
    }
}

/// <summary>Interface for handling 'E' key press actions</summary>
public interface IDoEStuff
{
    /// <summary>Method to execute actions when 'E' key is pressed</summary>
    public void DoE();
}
