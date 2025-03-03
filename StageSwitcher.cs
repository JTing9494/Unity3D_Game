using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageSwitcher : MonoBehaviour
{
    bool triggerActivated = false;
    [SerializeField] string nextLevel = "Stage 2 Cyber Punk";

    // Collider detects something
    private void OnTriggerEnter(Collider other)
    {
        // Prevent multiple triggers
        if (triggerActivated == true)
        {
            return;
        }

        // If the object that triggered is the player
        if (other.tag == "Player")
        {
            triggerActivated = true;
            ShadowManager.ins.Out(SwitchLevel);
        }
    }

    void SwitchLevel()
    {
        SceneManager.LoadScene(nextLevel);
    }
}
