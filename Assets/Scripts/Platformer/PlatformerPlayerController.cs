using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class PlatformerPlayerController : MonoBehaviour
{

    [Header("Dependencies")]


    [SerializeField] Rigidbody2D rb;
    
    [SerializeField] LayerMask groundCheckMask;
    [SerializeField] SpriteRenderer sR;
    [SerializeField] Animator anim;
    [SerializeField] Health healthScript;

    [SerializeField] Attack dmgHitbox;

    
    public Collider2D[] colliders;
    [SerializeField] Collider2D bosscollider;


    [Header("STATS")]
    [SerializeField] float jumpHeight;
    [SerializeField] float speed;


    [SerializeField] float horizMoveSpeed;
    [SerializeField] float vertVel;
    [SerializeField] Vector2 velocity;
    float deathMultiplier = 1;
    public float attackMult = 1;
    public Vector3 spawnpoint;
    public bool canJump = true;
    public int maxScene = 0;

    [Header("Data")]
    int currentPotions;
    bool grounded;
    //bool pause;
    bool wasGrounded = false;
    public float attackTime;
    public bool attackInterupted;
    int comboAttack;

public UnityEvent deathEventNotRespawn;

    public Interactable currentInteractable;
    public List<Interactable> inRange;

    bool respawnOnDeath = true;


    private void Awake()
    {
        RespawnHere();
    }
    public void ChangeInteraction(Interactable interact, bool add)
    {
        if (add)
        {
            inRange.Add(interact);
            currentInteractable = interact;
        }
        else
        {
            inRange.Remove(interact);
            if (inRange.Count == 0)
            {
                currentInteractable = null;
            }
        }
    }

    public void CheckInteractable()
    {
        if (currentInteractable == null && inRange.Count != 0)
        {
            currentInteractable = inRange[inRange.Count - 1];
        }
    }


    // Start is called before the first frame update
    public void Init()
    {
        gameObject.SetActive(true);
        canJump = true;
        anim.SetTrigger("Land");
        Time.timeScale = 1f;

        StopBossCollision();

    }
    public void StopBossCollision()
    {
        foreach (Collider2D c in colliders)
        {
            Physics2D.IgnoreCollision(c, bosscollider);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Movement
        velocity.x = Mathf.Lerp(rb.velocity.x, Input.GetAxisRaw("Horizontal") * speed * deathMultiplier* attackMult, Time.deltaTime * 10);

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (currentInteractable != null)
            {
                currentInteractable.interact.Invoke();
            }
        }



        velocity.y = rb.velocity.y;
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            //Debug.Log(grounded);
            if (grounded && canJump&&deathMultiplier!=0)
            {
                if (!GameObject.FindGameObjectWithTag("DialogueController").GetComponent<DialogueManager>().showing)
                {
                    velocity.y = jumpHeight * deathMultiplier;
                    anim.SetTrigger("Jump");
                    anim.ResetTrigger("Land");
                }

            }


        }
        rb.velocity = velocity;

        if (Input.GetKeyDown(KeyCode.Mouse0) /*&& grounded*/)
        {
            //attack
            if (attackTime == 0)
            {
                anim.SetTrigger("Attack1");
                attackTime = dmgHitbox.attackDelay();
            }
            else
            {
                if (comboAttack < 1)
                {
                    comboAttack = 1;
                }
            }
            
        }
    }

    public void Medicine()
    {
        if (currentPotions>0)
        {
            //Use Healing item
            healthScript.HealthChange(3);
            currentPotions--;
        }
    }

    private void FixedUpdate()
    {
        CheckInteractable(); 
        Vector3 offset = -transform.up * 0.75f * transform.localScale.x;
        RaycastHit2D groundCheck1 = Physics2D.Raycast(transform.position + offset + transform.right * 0.3f * transform.localScale.x, Vector2.down, 0.1f, groundCheckMask);
        RaycastHit2D groundCheck2 = Physics2D.Raycast(transform.position + offset + transform.right * -0.3f * transform.localScale.x, Vector2.down, 0.1f, groundCheckMask);
        RaycastHit2D groundCheck3 = Physics2D.Raycast(transform.position + offset, Vector2.down, 0.1f, groundCheckMask);
        if (groundCheck1.collider != null || groundCheck2.collider != null || groundCheck3.collider != null)
        {
            grounded = true;

        }
        else
        {
            grounded = false;
        }



        //animations
        if (Mathf.Abs(rb.velocity.x) > 0.1f)
        {
            sR.flipX = rb.velocity.x < 0 ? true : false;
        }

        if (grounded && !wasGrounded && rb.velocity.y <= 0)
        {
            anim.SetTrigger("Land");
            anim.ResetTrigger("Jump");
        }
        anim.SetFloat("Horizontal", Mathf.Abs(Input.GetAxisRaw("Horizontal")*attackMult));
        wasGrounded = grounded;


        bool was = attackTime == 0;

        attackTime -= attackTime == 0 ? 0 : Time.deltaTime;



        if (attackTime <= 0&&!was)
        {
            attackTime = 0;
            attackMult = 1;
            canJump = true;
            if (comboAttack == 1)
            {
                comboAttack++;
                anim.SetTrigger("Attack2");
                attackTime = dmgHitbox.attackDelay();
            }
            else
            {
                comboAttack = 0;
            }

        }
        else if(attackTime!=0)
        {
            attackMult = 0;
            canJump = false;
            if (attackInterupted)
            {
                comboAttack = 0;
                dmgHitbox.interupt = true;
                attackInterupted = false;
            }
            
        }

    }
    public void Die()
    {
        StartCoroutine(death());

        //teleport back to the place.
    }
    IEnumerator death()
    {
        anim.SetTrigger("Die");
        foreach (CapsuleCollider2D hitbox in GetComponents<CapsuleCollider2D>())
        {
            hitbox.enabled = false;
        }
        rb.velocity = Vector3.zero;
        rb.gravityScale = 0;
        deathMultiplier = 0;

        yield return new WaitForSeconds(1);
        canJump = true;
        attackInterupted = false;
        Respawn();


    }   
    public void Respawn()
    {
        if(respawnOnDeath)
        {
        transform.position = spawnpoint;

        foreach (CapsuleCollider2D hitbox in GetComponents<CapsuleCollider2D>())
        {
            hitbox.enabled = true;
        }
        rb.gravityScale = 1;
        anim.SetTrigger("Respawn");
        healthScript.HealthChange(10000);
        healthScript.UpdateHealthBar();
        deathMultiplier = 1;
        }else{
            deathEventNotRespawn.Invoke();
        }
        
    }
    public void RespawnHere()
    {
        spawnpoint= transform.position;
    }
    public void StopRespawning(){
        respawnOnDeath = false;
    }






}
