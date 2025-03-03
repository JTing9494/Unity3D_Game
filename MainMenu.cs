using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    private void Awake()
    {
        // Load item encyclopedia
        ItemManager.Instance.Load();
        // Load save file
        SaveManager.instance.Load();
        // Unlock the mouse cursor
        Cursor.lockState = CursorLockMode.None;
        // Normalize time
        Time.timeScale = 1;
    }

    [SerializeField] Text loadProgressButtonText = null;

    private void Start()
    {
        // Display progress on load progress button
        // If there is a saved level in the save file
        if (SaveManager.instance.playerData.saveLevelName != "")
        {
            loadProgressButtonText.text = "Continue \"" + SaveManager.instance.playerData.saveLevelName + "\"";
        }
        else
        {
            // If there is no save, do not allow loading
            loadProgressButtonText.text = "Continue Game";
            // Make the text semi-transparent to indicate it's not usable
            loadProgressButtonText.color = new Color(1f, 1f, 1f, 0.3f);
            // Remove the button collider to prevent clicking when there is no save
            loadProgressButtonText.raycastTarget = false;
        }
    }

    public void OpenAuthorLink() 
    {
        // Request the device to open a specific link
        Application.OpenURL("https://j-ting.itch.io/9487-music-remix");
    }

    [SerializeField] string firstLevelName = "Stage 1";
    [SerializeField] int defaultHP = 3;

    public void NewGame() 
    {
        // 1. Clear the save
        SaveManager.instance.playerData.saveLevelName = firstLevelName;
        SaveManager.instance.playerData.saveHP = defaultHP;
        SaveManager.instance.playerData.saveItemList = new List<Goods>();
        // 2. Fade out and then switch scenes
        ShadowManager.ins.Out(SwitchScene);
    }

    void SwitchScene()
    {
        // Switch levels based on the save
        SceneManager.LoadScene(SaveManager.instance.playerData.saveLevelName);
    }

    public void LoadProgress()
    {
        ShadowManager.ins.Out(SwitchScene);
    }

    public void CloseGameButton()
    {
        ShadowManager.ins.Out(CloseGame);
    }

    void CloseGame()
    {
        Application.Quit();
    }
}
