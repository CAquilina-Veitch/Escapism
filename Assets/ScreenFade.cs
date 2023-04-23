using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFade : MonoBehaviour
{
    public Color colour;
    Color clr;
    public bool fadeToThisColor;
    Image img;
    public float fadeLength;
    float progress;
    public float delay;

    private void OnEnable()
    {
        clr = colour;
        clr.a = 0;
        img = GetComponent<Image>();
        img.color = fadeToThisColor ? clr : colour;
        StartFade();
    }
    public void StartFade()
    {
        progress += 0.0001f;
    }

    private void FixedUpdate()
    {
        if (progress == 0)
        {
            return;
        }


        if (progress < fadeLength)
        {
            img.color = Color.Lerp(fadeToThisColor ? clr : colour, fadeToThisColor ? colour : clr, progress / fadeLength);
            progress += Time.deltaTime;

        }
        else if (progress >= fadeLength)
        {
            img.color = fadeToThisColor ? colour : clr;
            this.enabled = false;
        }
    }


}
