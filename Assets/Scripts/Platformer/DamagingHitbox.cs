using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagingHitbox : MonoBehaviour
{
    public int damage;
    BoxCollider2D hitbox;
    [SerializeField]SpriteRenderer ownerSprite;
    [SerializeField] bool isPlayer;
    private void OnEnable()
    {
        hitbox = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if (isPlayer)
        {

        }
        else
        {
            if (Input.GetAxisRaw("Horizontal") != 0)
            {
                hitbox.offset = Input.GetAxisRaw("Horizontal") > 0 ? new Vector2(0.8f, 0) : new Vector2(-0.8f, 0);
            }
        }
        
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.TryGetComponent(out Health health))
        {
            hitbox.enabled = false;
            health.HealthChange(-damage);
        }
    }
}
