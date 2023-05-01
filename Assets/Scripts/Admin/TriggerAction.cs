using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class TriggerAction : MonoBehaviour
{
    public UnityEvent onPlayerEnter;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(8);
        if(collision.tag == "Player")
        {
            onPlayerEnter.Invoke();
        }
    }
    private void OnEnable()
    {
        if(GameObject.FindGameObjectWithTag("Player").TryGetComponent<PlatformerPlayerController>(out PlatformerPlayerController ppc))
        {
            Physics2D.IgnoreCollision(ppc.colliders[1], GetComponent<Collider2D>());
        }
        
    }
}
