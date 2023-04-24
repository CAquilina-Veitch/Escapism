using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBox : MonoBehaviour
{
    // Start is called before the first frame update
    Health healthScript;
    public float delay = 0.5f;
    float progress = 100; 
    bool playerIsTouching;
    void Start()
    {
        healthScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
    }

    // Update is called once per frame
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            playerIsTouching = true;
        }
    }   
    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            playerIsTouching = false;
        }
    }
    void FixedUpdate()
    {
        if(progress < delay)
        {
            progress += Time.deltaTime;
        }        
        else
        {
            if (playerIsTouching)
            {
                healthScript.HealthChange(-1);
                progress = 0;
            }
        }
    }
}
