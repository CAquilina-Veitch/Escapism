using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum attackOwner { player,skeleton,boss}


public class Attack : MonoBehaviour
{
    public int damage;
    BoxCollider2D hitbox;
    [SerializeField]SpriteRenderer ownerSprite;
    [SerializeField] attackOwner owner;
    EnemyScript skeleton;

    float xHitboxOffset;

    public float preDelay;
    public float activeTime;
    public float cooldown;

    public bool interupt;

    public float attackDelay()
    {
        StartCoroutine(attack());


        return preDelay + activeTime + cooldown;
    }


    IEnumerator attack()
    {
        yield return new WaitForSeconds(preDelay);
        if (!interupt)
        {
            hitbox.enabled = true;
            yield return new WaitForSeconds(activeTime);
            hitbox.enabled = false;
        }
        else
        {
            interupt = false;
        }
        
    }



    private void FixedUpdate()
    {
        if (owner == attackOwner.player)
        {
            if (Input.GetAxisRaw("Horizontal") != 0)
            {
                hitbox.offset = new Vector2(xHitboxOffset * Input.GetAxisRaw("Horizontal"), hitbox.offset.y);
            }


        }
        else if (owner == attackOwner.skeleton)
        {
            hitbox.offset = new Vector2(xHitboxOffset * skeleton.currentDirection, hitbox.offset.y);
        }
        if (interupt)
        {
            hitbox.enabled = false;
        }
    }
    private void Awake()
    {
        hitbox = GetComponent<BoxCollider2D>();
    }
    private void OnEnable()
    {
        
        
        xHitboxOffset = hitbox.offset.x;
        if (owner == attackOwner.skeleton)
        {
            skeleton = GetComponentInParent<EnemyScript>();
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.TryGetComponent(out Health health))
        {
            hitbox.enabled = false;
            health.stunFor = cooldown*0.75f;
            health.HealthChange(-damage);
        }
    }
}
