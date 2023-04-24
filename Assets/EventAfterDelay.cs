using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventAfterDelay : MonoBehaviour
{
    public UnityEvent action;
    public float delay;

    private void OnEnable()
    {
        DelayEvent();
    }
    public void DelayEvent()
    {
        StartCoroutine(delayedEvent());
    }
    IEnumerator delayedEvent()
    {
        yield return new WaitForSeconds(delay);
        action.Invoke();
    }

}
