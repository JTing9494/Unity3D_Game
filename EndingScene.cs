using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static System.Net.WebRequestMethods;

public class EndingScene : MonoBehaviour
{
    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    public void ReturnToMainMenu()
    {
        ShadowManager.ins.Out(SwitchScene);
    }

    void SwitchScene()
    {
        SceneManager.LoadScene(0);
    }
    
    [SerializeField] string producerIcon = "";
    public void GetToKnowAuthor()
    {
        Application.OpenURL(producerIcon);
    }
}
