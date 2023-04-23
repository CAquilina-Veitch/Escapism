using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAnimation : MonoBehaviour
{
    Image image; // The UI image to cycle
    public Sprite[] sprites; // An array of sprites to cycle through
    public float fps = 12; // The rate at which to cycle through the sprites (in seconds)

    private IEnumerator CycleImages()
    {
        int index = 0;
        while (true)
        {
            image.sprite = sprites[index];
            index = (index + 1) % sprites.Length;
            yield return new WaitForSeconds(1/fps);
        }
    }

    private void OnEnable()
    {
        image = GetComponent<Image>();
        StartCoroutine(CycleImages());
    }
}
