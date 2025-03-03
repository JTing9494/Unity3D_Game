using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class SoundGroup : MonoBehaviour
{
    [SerializeField] List<AudioSource> soundEffects = new List<AudioSource>();

    void FetchSoundEffects()
    {
        AudioSource[] foundSoundEffects = this.transform.GetComponentsInChildren<AudioSource>();
        soundEffects = foundSoundEffects.ToList();
    }

    private void Start()
    {
        FetchSoundEffects();
    }

    public void PlayRandom()
    {
        int randomIndex = Random.Range(0, soundEffects.Count);
        soundEffects[randomIndex].Play();
    }
}
