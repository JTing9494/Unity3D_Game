using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerBarDisplay : MonoBehaviour
{
    void Start()
    {
        // Refresh content on power bar change and initialization
        SaveManager.instance.OnPowerChangeEvent += RefreshPower;
        RefreshPowerBar();
    }

    private void OnDisable()
    {
        SaveManager.instance.OnPowerChangeEvent -= RefreshPower;
    }

    [SerializeField] Image powerBar = null;

    void RefreshPowerBar()
    {
        powerBar.fillAmount = SaveManager.instance.playerData.pw / SaveManager.instance.playerData.maxPw;
    }
}
