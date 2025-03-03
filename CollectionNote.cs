using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
public class CollectionNote : MonoBehaviour, IDoEStuff
{
    [SerializeField] int targetNote = 4;
    [SerializeField] Image displayNote = null;
    [SerializeField] Text dispayText = null;
    [SerializeField] UnityEvent getAllNoteEvent = null;
	public GameObject cutsceneObject;


	int currentNote 
    {
        get { return _curretnNote; }
        set 
        { 
            _curretnNote = value;
            displayNote.fillAmount = _curretnNote / targetNote;
            dispayText.text = _curretnNote + " / " + targetNote;
			// If Note greater/equal than targetNote
            if (_curretnNote >= targetNote)
            {
				StartCoroutine(TriggerCutscene());
				getAllNoteEvent.Invoke();
			}
		}
	}
    int _curretnNote = 0;

    void Start()
    {
        _curretnNote = 0;
        SaveManager.instance.itemChangeEvent += CheckNoteNum;
		// Disable the cutscene object at the start
		if (cutsceneObject != null)
		{
			cutsceneObject.SetActive(false);
		}
	}

    void OnDisable()
    {
		SaveManager.instance.itemChangeEvent -= CheckNoteNum;
	}

    void CheckNoteNum()
    {
		currentNote = SaveManager.instance.playerData.ConsumeItem(1);
	}

    [SerializeField] SayStuff sayNote = null;
	public void DoE()
	{
		SaySystem.instance.StartSay(sayNote);
	}

	private IEnumerator TriggerCutscene()
	{
		yield return new WaitForSeconds(0.1f); // Short delay before cutscene

		if (cutsceneObject != null)
		{
			cutsceneObject.SetActive(true);
			Debug.Log("Cutscene triggered!");
		}
		else
		{
			Debug.LogWarning("Cutscene object is not assigned!");
		}

		yield return new WaitForSeconds(3f);
		cutsceneObject.SetActive(false);
		Debug.Log("Main Camera!");
	}
}
