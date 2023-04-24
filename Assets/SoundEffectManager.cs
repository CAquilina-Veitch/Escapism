using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct SoundEffectData
{
    public string name;
    public AudioClip clip;
    public float relativeVolume;
    public bool looping;
}


public class SoundEffectManager : MonoBehaviour
{
    public List<SoundEffectData> sfxd;
    public GameObject sfxPrefab;
    public float SFXVolume = 1;
    
    public void SetVolume(float to)
    {
        SFXVolume = to;
        foreach(GameObject g in GameObject.FindGameObjectsWithTag("Sound"))
        {
            g.GetComponent<SoundEffect>().ChangeVolume(SFXVolume);
        }
    }
    public void PlaySoundEffect(string id)
    {
        SoundEffectData currentSfx;
        if(!sfxd.Exists(x => x.name == id))
        {
            return;
        }
        currentSfx = sfxd.Find(x => x.name == id);
        GameObject obj = Instantiate(sfxPrefab,transform);
        obj.name = $"SFX - {currentSfx.name}";
        obj.GetComponent<AudioSource>().volume = currentSfx.relativeVolume * SFXVolume;
        obj.GetComponent<AudioSource>().clip = currentSfx.clip;
        obj.GetComponent<SoundEffect>().Init(currentSfx);
    }
    public void PlaySoundEffectWithDelay(string id,float delay)
    {
        StartCoroutine(DelayedSFX(id,delay));
    }
    IEnumerator DelayedSFX(string id,float delay)
    {
        yield return new WaitForSeconds(delay);
        PlaySoundEffect(id);
        
    }


    private void OnEnable()
    {
        DontDestroyOnLoad(gameObject);
    }
}
