using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MumMoveThrough : MonoBehaviour
{
    Rigidbody2D prb;
    // Start is called before the first frame update
    private void OnEnable()
    {
        prb = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
        foreach (Collider2D c in prb.GetComponent<PlatformerPlayerController>().colliders)
        {
            Physics2D.IgnoreCollision(c, GetComponent<Collider2D>()); 
        }
    }
}
