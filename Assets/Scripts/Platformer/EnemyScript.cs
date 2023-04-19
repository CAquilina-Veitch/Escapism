using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    [Header("Stats")]
    public int speed;
    public int damage;
    [SerializeField] float attackAnimationTime;
    [SerializeField] Vector3 offset;
    [SerializeField] float hitboxWidth;
    float dropChance;
    [SerializeField] Vector2 xBounds;
    [SerializeField] LayerMask wallMask;


    [Header("Data")]
    public int currentDirection=-1;
    [SerializeField]int attackMult =1;
    Vector2 velocity;
    bool dead = false;
    public float attackTime;
    public bool attackInterupted;


    [Header("Dependencies")]
    [SerializeField] Animator anim;
    [SerializeField] Attack dmgHitbox;
    [SerializeField] GameObject itemDropPrefab;
    [SerializeField] SpriteRenderer sR;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Health healthScript;



    // Start is called before the first frame update
    void Start()
    {
        dmgHitbox.damage = damage;
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
                if (attackTime==0)
                {
                    anim.SetTrigger("Attack");
                    attackTime = dmgHitbox.attackDelay();
                }
            }
            else
            {
                RaycastHit2D wallcheck2 = Physics2D.Raycast(transform.position + new Vector3(currentDirection * hitboxWidth, 0) + offset, Vector2.down, 0.1f, wallMask);
                if (wallcheck2.collider != null)
                {
                    currentDirection = -currentDirection;
                }
                
            }
        }
        else
        {
            //check for walk off edge
            RaycastHit2D edgeCheck = Physics2D.Raycast(transform.position + new Vector3(currentDirection * hitboxWidth, 0) + offset, Vector2.down, 1.11f, wallMask);
            Debug.DrawRay(transform.position + new Vector3(currentDirection * hitboxWidth, 0) + offset, Vector2.down, Color.yellow, 5);
            if (edgeCheck.collider != null)
            {
                if (edgeCheck.collider.tag != "GroundCollision")
                {
                    Debug.Log("1");
                    currentDirection = -currentDirection;
                }
            }
            else
            {
                RaycastHit2D floor = Physics2D.Raycast(transform.position - (new Vector3(currentDirection * hitboxWidth, 0) + offset), Vector2.down, 1.6f, wallMask);
                Debug.DrawRay(transform.position - (new Vector3(currentDirection * hitboxWidth, 0) + offset), Vector2.down, Color.green, 5) ;
                if (floor.collider != null)
                {
                    if (floor.collider.tag == "GroundCollision")
                    {
                        Debug.Log("2");
                        currentDirection = -currentDirection;
                    }

                }

            }
        }


        bool was = attackTime == 0;

        attackTime -= attackTime == 0 ? 0 : Time.deltaTime;

        if (attackTime <= 0 && !was)
        {
            attackTime = 0;
            attackMult = 1;

        }
        else if (attackTime != 0)
        {
            attackMult = 0;
            if (attackInterupted)
            {
                dmgHitbox.interupt = true;
                attackInterupted = false;
            }

        }

        
        currentDirection = transform.position.x < xBounds.x && currentDirection == -1 ? 1 : currentDirection;
        currentDirection = transform.position.x > xBounds.y && currentDirection == 1 ? -1 : currentDirection;


        velocity.x = Mathf.Lerp(rb.velocity.x, currentDirection * speed * attackMult, Time.deltaTime * 10);
        rb.velocity = new Vector3(velocity.x, rb.velocity.y);


        sR.flipX = currentDirection > 0 ? false : true;
        


    }
    public void Die()
    {
        speed = 0;
        rb.velocity = Vector2.zero;
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        GetComponent<CapsuleCollider2D>().enabled = false;
        //drop item
        
        float temp = Random.Range(0f, 1f);
        if (temp <= dropChance)
        {
            GameObject itemobj = Instantiate(itemDropPrefab, transform.position, Quaternion.identity);
            itemobj.GetComponent<Rigidbody2D>().velocity = new Vector3(0, 3);
        }
        
        //GetComponent<Health>().enabled = false;
        anim.SetTrigger("Die");
        StartCoroutine(Death());
        //this.enabled = false;
        

    }
    IEnumerator Death()
    {

        yield return new WaitForSeconds(3);

        Destroy(gameObject);
    }






}
