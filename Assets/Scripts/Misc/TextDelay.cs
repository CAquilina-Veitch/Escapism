using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextDelay : MonoBehaviour
{

    public string textToShow;
    [SerializeField] TextMeshProUGUI dialogueText;
    public float waitSeconds = 0.1f;
    public float blinky = 0.25f;
    public float beginDelay = 2.2f;
    bool isUnderscore;

    private void OnEnable()
    {
        textToShow = dialogueText.text;
        dialogueText.text = "";
        StartCoroutine(ShowCharacters());

    }

    IEnumerator ShowCharacters()
    {
        yield return new WaitForSeconds(beginDelay);
        for (int i = 0; i<textToShow.Length; i++)
        {
            dialogueText.text = textToShow[..i] + " _";
            
            yield return new WaitForSeconds(waitSeconds);
        }
        while (true)
        {
            dialogueText.text = isUnderscore? textToShow: textToShow + " _";
            isUnderscore = !isUnderscore;
            yield return new WaitForSeconds(blinky);
        }
        
    }

    public void StartText()
    {
        StartCoroutine(ShowCharacters());
    }
}
