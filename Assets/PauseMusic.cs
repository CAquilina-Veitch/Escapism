using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PauseMusic : MonoBehaviour
{
    public bool activateOnEnable = true;
    public bool trueIfYouWantToPause;

    SoundEffectManager sfxm;
    public void PauseSoundEffect()
    {
        sfxm.PauseMusic(trueIfYouWantToPause);
    }


    private void OnEnable()
    {
        sfxm = GameObject.FindGameObjectWithTag("SFXManager").GetComponent<SoundEffectManager>();
        if (activateOnEnable)
        {
            PauseSoundEffect();
        }

    }
}
