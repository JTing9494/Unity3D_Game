using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NoteBox : MonoBehaviour
{
	[SerializeField] bool switchControl = false;
	[SerializeField] bool onlyOpen = false;
	void Start()
	{
		// Switch state refreshed to ensure lights are off
		SwitchStatus();
	}
	public void DoE()
	{
		if (switchControl == true && onlyOpen == true)
			return;


		// Original Status = Original Status Oppsite
		switchControl = !switchControl;
		SwitchStatus();
	}
	[SerializeField] UnityEvent opening = null;
	[SerializeField] UnityEvent closing = null;
	
	void SwitchStatus()
	{
		if (switchControl == true)
		{
			opening.Invoke();
		}

		if (switchControl == false)
		{
			closing.Invoke();
		}
	}
}
