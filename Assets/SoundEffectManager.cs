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
    public bool isMusic;
    public bool dontDeleteNewScene;
}


public class SoundEffectManager : MonoBehaviour
{
    public List<SoundEffectData> sfxd;
    public GameObject sfxPrefab;
    public float SFXVolume = 1;
    GameObject music;

    int currentScene;
    public List<GameObject> sfxPlayers;
    public List<GameObject> paused;

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
        if(currentSfx.isMusic&&music!=null)
        {
            sfxPlayers.Remove(music);
            Destroy(music);

        }
        GameObject obj = Instantiate(sfxPrefab,transform);
        obj.name = $"SFX - {currentSfx.name}";
        obj.GetComponent<AudioSource>().volume = currentSfx.relativeVolume * SFXVolume;
        obj.GetComponent<AudioSource>().clip = currentSfx.clip;
        obj.GetComponent<SoundEffect>().Init(currentSfx);
        sfxPlayers.Add(obj);
        if (currentSfx.isMusic)
        {
            music = obj;
        }
    }
    public void PauseSound(string id,bool trueifpause)
    {
        if (trueifpause)
        {
            foreach (GameObject sfxi in sfxPlayers.FindAll(x => x.name == id))
            {
                paused.Add(sfxi);
                sfxi.GetComponent<AudioSource>().Pause();
            }
        }
        else
        {
            foreach(GameObject o in paused)
            {
                o.GetComponent<AudioSource>().UnPause();
                paused.Remove(o);
            }
        }

    }
    // void 

    public void PauseMusic(bool setToPaused)
    {
        if (setToPaused)
        {
            music.GetComponent<AudioSource>().Pause();
        }
        else
        {
            music.GetComponent<AudioSource>().UnPause();
        }
        
        
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
    private void OnLevelWasLoaded(int level)
    {
        foreach(GameObject g in sfxPlayers)
        {
            SoundEffectData currentSfx;
            currentSfx = sfxd.Find(x => x.name == g.name);
            Destroy(g);
        }
    }



    private void OnEnable()
    {
        DontDestroyOnLoad(gameObject);
    }
}
