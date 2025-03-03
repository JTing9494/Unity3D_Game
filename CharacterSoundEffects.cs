using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSoundEffects : MonoBehaviour
{
    [SerializeField] List<AudioSource> footstepSounds = new List<AudioSource>();
    [SerializeField] float footstepCooldown = 0.1f;
    float lastFootstepTime = 0f;

    public void OnRun()
    {
        OnFoot();
    }

    public void OnFoot()
    {
        if(Time.time > lastFootstepTime + footstepCooldown)
        {
            lastFootstepTime = Time.time;
            int randomIndex = Random.Range(0, footstepSounds.Count);
            footstepSounds[randomIndex].Play();
        }
    }

    public void NewEvent()
    {
        Debug.Log("Empty event");
    }

    [SerializeField] List<AudioSource> landingSounds = new List<AudioSource>();
    public void OnLand()
    {
        int randomIndex = Random.Range(0, landingSounds.Count);
        landingSounds[randomIndex].Play();
    }

    [SerializeField] List<AudioSource> attackSounds = new List<AudioSource>();
    public void OnAttack()
    {
        int randomIndex = Random.Range(0, attackSounds.Count);
        attackSounds[randomIndex].Play();
    }

    [SerializeField] List<AudioSource> damageSounds = new List<AudioSource>();
    public void OnDamage()
    {
        int randomIndex = Random.Range(0, damageSounds.Count);
        damageSounds[randomIndex].Play();
    }
}
