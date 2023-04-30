    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhoneButton : MonoBehaviour
{
    public float startDelay = 15;
    GameObject visualButton;
    GameObject dialogueBox;
    RectTransform tran;
    Vector3 init;
    bool waited;

    bool phoneUnchecked = true;
    float amt = 2;


    private void OnEnable()
    {
        tran = GetComponent<RectTransform>();
        init = tran.anchoredPosition;
        visualButton = transform.GetChild(0).gameObject;
        dialogueBox = GameObject.FindGameObjectWithTag("DialogueController").transform.GetChild(0).gameObject;
        StartCoroutine(StartDelay());
    }
    IEnumerator StartDelay()
    {
        yield return new WaitForSeconds(startDelay);
        while (dialogueBox.activeSelf)
        {
            yield return new WaitForSeconds(1);
        }
        waited = true;
        StartCoroutine(Buzz());
    }
    private void FixedUpdate()
    {
        if (waited)
        {
            visualButton.SetActive(!dialogueBox.activeSelf);
        }
        
    }
    IEnumerator Buzz()
    {
        while (phoneUnchecked)
        {
            for(int i = 0; i < 30; i++)
            {
                yield return new WaitForSeconds(1f / 60);
                tran.anchoredPosition = init + new Vector3(Random.Range(-amt, amt), Random.Range(-amt, amt));
            }
            tran.anchoredPosition = init;
            yield return new WaitForSeconds(5);
        }
        
    }
}
