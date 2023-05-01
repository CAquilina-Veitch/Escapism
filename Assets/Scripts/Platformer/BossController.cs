using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BossController : MonoBehaviour
{

    public float speed;
    public float actionTime;
    public int currentPhase = 1;

    public float currentAttackDelay;

    public Attack[] atks;
    public Vector2 atkMotor;
    public float xMotor;
    public Vector2 xBounds;
    public Rigidbody2D rb;
    public float attackMult = 1;
    public bool isDead = false;
    bool hasCooledDown;

    Rigidbody2D prb;
    int playerDirection;
    Animator anim;
    SpriteRenderer sr;
    [SerializeField] ParticleSystem[] ps;
    [SerializeField] LayerMask groundCheckMask;

    public UnityEvent onDeathAction;

    private void OnEnable()
    {
        prb = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
        
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        currentAttackDelay = 3;
        attackMult = 0;
    }

    private void FixedUpdate()
    {
        if (currentAttackDelay == 0&&!isDead)
        {
            Attack(Random.Range(0, currentPhase));
            //Attack(2);
            playerDirection = prb.transform.position.x < transform.position.x ? -1 : 1;
            sr.flipX = playerDirection < 0 ? true : false;
            
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
        else if (currentAttackDelay != 0&&!hasCooledDown)
        {
            attackMult = 0;

        }
        xMotor = transform.position.x < xBounds.x ? 1 : xMotor;
        xMotor = transform.position.x > xBounds.y ? -1 : xMotor;
        atkMotor = transform.position.x < xBounds.x ? Vector2.zero : atkMotor;
        atkMotor = transform.position.x > xBounds.y ? Vector2.zero : atkMotor;

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
                StartCoroutine(Charge());
                break;
            default:
                break;
        }
    }

    IEnumerator Charge()
    {
        currentAttackDelay = atks[2].attackDelay();
        anim.SetTrigger("Slash");
        atkMotor = new Vector2(-playerDirection*2, 0);
        yield return new WaitForSeconds(atks[2].preDelay/2);
        if (!isDead)
        {
            sr.flipX = playerDirection < 0 ? true : false;
            atkMotor = Vector2.zero;
            yield return new WaitForSeconds(atks[2].preDelay / 2);
            if (!isDead)
            {
                atkMotor = new Vector2(15 * playerDirection, 0);
                ps[4].Play();
                yield return new WaitForSeconds(atks[2].activeTime);
                if (!isDead)
                {
                    atkMotor = Vector2.zero;
                    ps[4].Stop();
                }
            }
        }
        if (isDead)
        {
            atks[0].interupt = true;
            ps[4].Stop();
        }


    }
    IEnumerator Sweep()
    {
        currentAttackDelay = atks[0].attackDelay();
        
        int playerDirection = prb.transform.position.x < transform.position.x ? -1 : 1;
        atkMotor = new Vector2(4*playerDirection, 0.3f);
        rb.gravityScale = 0;
        ps[0].Stop();
        ps[1].Stop();
        yield return new WaitForSeconds(atks[0].preDelay-0.25f-0.2f);
        if (!isDead)
        {
            atkMotor = new Vector2(4 * playerDirection, -30);
            rb.gravityScale = 1;
            yield return new WaitForSeconds(0.2f);
            if (!isDead)
            {
                anim.SetTrigger("Sweep");
                ps[2].Play();
                yield return new WaitForSeconds(0.25f);
                if (!isDead)
                {
                    atkMotor = Vector2.zero;
                    ps[0].Play();
                    ps[1].Play();
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
        ps[0].Stop();
        ps[1].Stop();
        yield return new WaitForSeconds(0.5f);
        if (!isDead)
        {
            transform.position = prb.transform.position;
            RaycastHit2D floorPoint = Physics2D.Raycast(transform.position + Vector3.up * 5, Vector3.down, 10f, groundCheckMask);
            {
                if (floorPoint.collider != null)
                {
                    transform.position = floorPoint.point + Vector2.up * GetComponent<CapsuleCollider2D>().size.y * transform.localScale.y;
                    
                }
                ps[3].Play();
            }
            yield return new WaitForSeconds(1.5f);
            GetComponent<Health>().enabled = true;
            anim.SetTrigger("Crawl");
            yield return new WaitForSeconds(0.25f);
            if (!isDead)
            {
                rb.velocity = Vector3.up * 15;

                yield return new WaitForSeconds(atks[1].activeTime);
                if (!isDead)
                {
                    rb.gravityScale = 1f;
                    rb.velocity *= 0.5f;
                    atkMotor = Vector2.zero;
                    ps[0].Play();
                    ps[1].Play();
                    yield return new WaitForSeconds(atks[1].cooldown);
                    if (!isDead)
                    {
                        rb.gravityScale = 1;
                    }

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
        foreach(ParticleSystem s in ps)
        {
            s.emissionRate = 0f;
        }
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

        yield return new WaitForSeconds(5);
        onDeathAction.Invoke();
        Destroy(gameObject);
    }

}
