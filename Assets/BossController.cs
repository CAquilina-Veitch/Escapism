using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{

    public float speed;
    public float actionTime;
    public int currentPhase = 1;

    public float currentAttackDelay;

    public Attack[] atks;
    public Vector2 atkMotor;
    public float xMotor;
    public Rigidbody2D rb;
    public float attackMult = 1;
    public bool isDead = false;

    bool hasCooledDown;

    Rigidbody2D prb;

    Animator anim;
    [SerializeField] ParticleSystem[] ps;
    [SerializeField] LayerMask groundCheckMask;


    private void OnEnable()
    {
        prb = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
        Physics2D.IgnoreCollision(prb.GetComponent<Collider2D>(), GetComponent<Collider2D>()); 
        anim = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if (currentAttackDelay == 0&&!isDead)
        {
            //Attack(Random.Range(0, currentPhase));
            Attack(1);
        }


        bool was = currentAttackDelay == 0;

        currentAttackDelay -= currentAttackDelay == 0 ? 0 : Time.deltaTime;

        if (currentAttackDelay <= 0 && !was)
        {
            currentAttackDelay = 0;
            attackMult = 1;
            if (!hasCooledDown)
            {
                EndAttack();
            }

        }
        else if (currentAttackDelay != 0)
        {
            attackMult = 0;

        }

        rb.velocity = new Vector2(xMotor * attackMult, rb.velocity.y)+atkMotor;
        


    }
    public void Attack(int num)
    {
        hasCooledDown = false;
        switch (num)
        {
            case 0:
                
                StartCoroutine(Sweep());
                break;
            case 1:
                StartCoroutine(Backstab());
                break;
            case 2:
                break;
            default:
                break;
        }
    }

    IEnumerator Charge()
    {
        int playerDirection = prb.transform.position.x < transform.position.x ? -1 : 1;
        atkMotor = new Vector2(-playerDirection, 0);
        yield return new WaitForSeconds(atks[0].preDelay/2);
        if (!isDead)
        {
            atkMotor = Vector2.zero;
            yield return new WaitForSeconds(atks[0].preDelay / 2);
            if (!isDead)
            {
                atkMotor = new Vector2(15 * playerDirection, 0);
                yield return new WaitForSeconds(atks[0].activeTime);
                if (!isDead)
                {
                    atkMotor = Vector2.zero;
                }
            }
        }
        if (isDead)
        {
            atks[0].interupt = true;
        }


    }
    IEnumerator Sweep()
    {
        currentAttackDelay = atks[0].attackDelay();
        
        int playerDirection = prb.transform.position.x < transform.position.x ? -1 : 1;
        atkMotor = new Vector2(4*playerDirection, 0.3f);
        rb.gravityScale = 0;
        yield return new WaitForSeconds(atks[0].preDelay-0.25f-0.2f);
        if (!isDead)
        {
            atkMotor = new Vector2(4 * playerDirection, -30);
            rb.gravityScale = 1;
            yield return new WaitForSeconds(0.2f);
            if (!isDead)
            {
                anim.SetTrigger("Sweep");
                yield return new WaitForSeconds(0.25f);
                if (!isDead)
                {
                    atkMotor = Vector2.zero;
                    yield return new WaitForSeconds(atks[0].activeTime + atks[0].cooldown);
                    if (!isDead)
                    {

                    }
                }
            }
            if (isDead)
            {
                atks[0].interupt = true;
            }
        }
        
        

    }
    IEnumerator Backstab()
    {
        currentAttackDelay = atks[1].attackDelay();
        atkMotor = Vector2.zero;
        anim.SetTrigger("Fade");
        yield return new WaitForSeconds(0.5f);
        GetComponent<Health>().enabled = false;
        ps[0].enableEmission = false;
        yield return new WaitForSeconds(0.5f);
        transform.position = prb.transform.position;
        RaycastHit2D floorPoint = Physics2D.Raycast(transform.position+Vector3.up*20, Vector3.down, 100f, groundCheckMask);
        {
            if (floorPoint.collider != null)
            {
                transform.position = floorPoint.point + Vector2.up * GetComponent<CapsuleCollider2D>().size.y *transform.localScale.y;
            }
        }
        yield return new WaitForSeconds(0.25f);
        GetComponent<Health>().enabled = true;
        ps[0].enableEmission = true;
        anim.SetTrigger("Crawl");
        yield return new WaitForSeconds(0.25f);
        if (!isDead)
        {
            rb.velocity = Vector3.up * 6;
            rb.gravityScale = 0.2f;
            yield return new WaitForSeconds(atks[1].activeTime);
            if (!isDead)
            {
                rb.gravityScale = 0;
                rb.velocity = Vector2.zero;
                atkMotor = Vector2.zero;
                yield return new WaitForSeconds(atks[1].cooldown);
                if (!isDead)
                {
                    rb.gravityScale = 1;
                }
                
            }
        }
        if (isDead)
        {
            atks[1].interupt = true;
            rb.gravityScale = 1;
        }




    }



    public void EndAttack()
    {
        currentAttackDelay = Random.Range(1, 4);
        hasCooledDown = true;
        atkMotor = Vector2.zero;
    }

    public void Die()
    {
        speed = 0;
        xMotor = 0;
        atkMotor = Vector2.zero;
        rb.velocity = Vector2.down;
        isDead = true;
        anim.SetBool("isDead", true);
        //GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        //drop item
        /*
                float temp = Random.Range(0f, 1f);
                if (temp <= dropChance)
                {
                    GameObject itemobj = Instantiate(itemDropPrefab, transform.position, Quaternion.identity);
                    itemobj.GetComponent<Rigidbody2D>().velocity = new Vector3(0, 3);
                }*/

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
