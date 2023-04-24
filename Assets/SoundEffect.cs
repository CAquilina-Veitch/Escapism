using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffect : MonoBehaviour
{
    SoundEffectData d;

    public void Init(SoundEffectData data)
    {
        d = data;
        StartCoroutine(Process());
    }
    IEnumerator Process()
    {
        GetComponent<AudioSource>().Play();
        GetComponent<AudioSource>().loop = d.looping;
        yield return new WaitForSeconds(d.clip.length+0.1f);
        if (!d.looping)
        {
            Destroy(gameObject);
        }
    }
    public void ChangeVolume(float to)
    {
        GetComponent<AudioSource>().volume = to * d.relativeVolume;
    }
}
