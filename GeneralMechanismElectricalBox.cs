using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class GeneralMechanismElectricalBox : MonoBehaviour, IDoEStuff
{
    [SerializeField] int targetEnergy = 2;
    [SerializeField] Image energyBar = null;
    [SerializeField] Text energyText = null;
    [SerializeField] UnityEvent onPowerOn = null;
    
    int currentEnergy
    {
        get { return _currentEnergy; }
        set
        {
            _currentEnergy = value;
            // Display energy percentage
            energyBar.fillAmount = (float)_currentEnergy / (float)targetEnergy;
            // Display text
            energyText.text = _currentEnergy + " / " + targetEnergy;
            // If current energy is greater than or equal to target energy
            if (_currentEnergy >= targetEnergy)
            {
                onPowerOn.Invoke();
            }
        }
    }
    int _currentEnergy = 0;

    void Start()
    {
        currentEnergy = 0;
    }

    public void IncreaseEnergy()
    {
        currentEnergy += 1;
    }

    public void DecreaseEnergy()
    {
        currentEnergy -= 1;
    }

    [SerializeField] SayStuff electricalBoxExplanation = null;

    public void DoE()
    {
        SaySystem.instance.StartSay(electricalBoxExplanation);
    }
}
