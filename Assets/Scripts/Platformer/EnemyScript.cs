using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    [Header("Stats")]
    public int maxHealth;
    public int speed;
    public int damage;
    [SerializeField] float attackAnimationTime;
    [SerializeField] Vector3 offset;
    [SerializeField] float hitboxWidth;
    float dropChance;
    [SerializeField] Vector2 xBounds;


    [Header("Data")]
    int currentDirection=-1;
    Vector2 velocity;
    bool dead = false;
    bool canAttack = true;

    [Header("Dependencies")]
    [SerializeField] Animator anim;
    [SerializeField] DamagingHitbox dmgHitbox;
    [SerializeField] GameObject itemDropPrefab;
    [SerializeField] SpriteRenderer sR;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Health healthScript;



    // Start is called before the first frame update
    void Start()
    {
        dmgHitbox.damage = damage;
        healthScript.setHealth(maxHealth);
        returnToLoop();
    }


    private void FixedUpdate()
    {
        if (dead) { return; }


        //check for wall
        RaycastHit2D wallCheck = Physics2D.Raycast(transform.position + new Vector3(currentDirection * hitboxWidth, 0) + offset, Vector2.down, 0.1f);
        Debug.DrawRay(transform.position + new Vector3(currentDirection * hitboxWidth, 0)  + offset, Vector2.down * 0.1f, Color.magenta, 5);
        //Debug.Log(wallCheck.collider);
        if (wallCheck.collider != null)
        {
            
            if (wallCheck.collider.tag == "Player")
            {
                if (canAttack)
                {
                    attack();
                }
            }
            else
            {
                currentDirection = -currentDirection;
            }
        }
        else
        {
            //check for walk off edge
            RaycastHit2D edgeCheck = Physics2D.Raycast(transform.position + new Vector3(currentDirection * hitboxWidth, 0) + offset, Vector2.down, 1.11f);
            Debug.DrawRay(transform.position + new Vector3(currentDirection * hitboxWidth, 0) + offset, Vector2.down, Color.yellow, 5);
            if (edgeCheck.collider != null)
            {
                if (edgeCheck.collider.tag != "GroundCollision")
                {
                    currentDirection = -currentDirection;
                }
            }
            else
            {
                RaycastHit2D floor = Physics2D.Raycast(transform.position - (new Vector3(currentDirection * hitboxWidth, 0) + offset), Vector2.down, 1.6f);
                Debug.DrawRay(transform.position - (new Vector3(currentDirection * hitboxWidth, 0) + offset), Vector2.down, Color.green, 5) ;
                if (floor.collider != null)
                {
                    if (floor.collider.tag == "GroundCollision")
                    {
                        currentDirection = -currentDirection;
                    }

                }

            }
        }
        currentDirection = transform.position.x < xBounds.x && currentDirection == -1?1:currentDirection;
        currentDirection = transform.position.x > xBounds.y && currentDirection == 1?-1:currentDirection;


        velocity.x = Mathf.Lerp(rb.velocity.x, currentDirection * speed, Time.deltaTime * 10);
        rb.velocity = new Vector3(velocity.x, rb.velocity.y);

        sR.flipX = rb.velocity.x > 0 ? false : true;


    }
    void attack()
    {
        canAttack = false;
        StartCoroutine(attackHitbox());
    }
    IEnumerator attackHitbox()
    {
        anim.SetTrigger("Attack");
        yield return new WaitForSeconds(attackAnimationTime*0.5f);
        if (dead) { dmgHitbox.GetComponent<BoxCollider2D>().enabled = false; yield break; }
        dmgHitbox.GetComponent<BoxCollider2D>().enabled = true;
        yield return new WaitForSeconds(0.2f);
        dmgHitbox.GetComponent<BoxCollider2D>().enabled = false;
        yield return new WaitForSeconds(0.4f);
        canAttack = true;
    }
    public void Die()
    {
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        GetComponent<CapsuleCollider2D>().enabled = false;
        //drop item
        
        float temp = Random.Range(0f, 1f);
        if (temp <= dropChance)
        {
            GameObject itemobj = Instantiate(itemDropPrefab, transform.position, Quaternion.identity);
            itemobj.GetComponent<Rigidbody2D>().velocity = new Vector3(0, 3);
        }
        
        GetComponent<Health>().enabled = false;
        anim.SetTrigger("Die");
        StartCoroutine(Death());
        this.enabled = false;
        

    }
    IEnumerator Death()
    {
        yield return new WaitForSeconds(3);

        Destroy(gameObject);
    }

    void returnToLoop()
    {
        StartCoroutine(headattackloop());
    }
    IEnumerator headattackloop()
    {
        yield return new WaitForSeconds(1);
        if (canAttack)
        {
            RaycastHit2D upCheck = Physics2D.Raycast(transform.position + new Vector3(currentDirection * hitboxWidth, 1.3f) + offset, new Vector3(-currentDirection, 0), 1.11f);
            Debug.DrawRay(transform.position + new Vector3(currentDirection * hitboxWidth, 1.3f) + offset, new Vector3(-currentDirection, 0), Color.cyan, 5);
            if (upCheck.collider != null)
            {
                Debug.Log(upCheck.collider.gameObject.name);
                if (upCheck.collider.tag == "Player")
                {
                    attack();
                }
            }

        }
        returnToLoop();

    }





}
