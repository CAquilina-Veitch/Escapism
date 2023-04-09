using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagingHitbox : MonoBehaviour
{
    public int damage;
    float cooldownTime = 1;
    float i;
    BoxCollider2D hitbox;
    private void OnEnable()
    {
        hitbox = GetComponent<BoxCollider2D>();
    }
    private void FixedUpdate()
    {
        if (i > 0)
        {
            i -= Time.deltaTime;
        }
        else if (i < 0)
        {
            i = 0;
        }

        


    }

    private void Update()
    {
        if (Input.GetAxisRaw("Horizontal") != 0)
        {
            hitbox.offset = Input.GetAxisRaw("Horizontal") > 0 ? new Vector2(0.8f, 0) : new Vector2(-0.8f, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log($"ME {gameObject.name} hit other {other.gameObject.name}");
        if (i == 0)
        {
            if (other.TryGetComponent(out Health health))
            {
                hitbox.enabled = false;

                //attack
                i = cooldownTime;
                health.HealthChange(-damage);
                GetComponent<BoxCollider2D>().enabled = false;
            }
        }


        
    }
}
