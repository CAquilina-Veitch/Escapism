using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PauseSoundEffect : MonoBehaviour
{
    public bool activateOnEnable = true;
    public bool trueIfYouWantToPause;
    public string id;

    SoundEffectManager sfxm;
    public void pausesfx()
    {
        sfxm.PauseSound(id,trueIfYouWantToPause);
    }


    private void OnEnable()
    {
        sfxm = GameObject.FindGameObjectWithTag("SFXManager").GetComponent<SoundEffectManager>();
        if (activateOnEnable)
        {
            pausesfx();
        }

    }
}
