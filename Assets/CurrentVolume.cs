using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrentVolume : MonoBehaviour
{
    private void OnEnable()
    {
        GetComponent<Slider>().value = GameObject.FindGameObjectWithTag("SFXManager").GetComponent<SoundEffectManager>().SFXVolume;
    }
    public void ChangeVolume()
    {
        GameObject.FindGameObjectWithTag("SFXManager").GetComponent<SoundEffectManager>().SetVolume(GetComponent<Slider>().value);
    }
}
