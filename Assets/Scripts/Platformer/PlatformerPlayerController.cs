using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlatformerPlayerController : MonoBehaviour
{

    [Header("Dependencies")]


    [SerializeField] Rigidbody2D rb;
    
    [SerializeField] LayerMask groundCheckMask;
    [SerializeField] SpriteRenderer sR;
    [SerializeField] Animator anim;
    [SerializeField] Health healthScript;

    [SerializeField] DamagingHitbox dmgHitbox;

    //[SerializeField] dialogueManager dM;
    [SerializeField] Image invisBar;
    [SerializeField] GameObject WinObject;
    [SerializeField] Text WinText;
    //[SerializeField] GameObject pauseScreen;

    [Header("Animations")]

    [SerializeField] Material SpriteMaterial;
    [SerializeField] Shader GreyscaleShader;
    [SerializeField] Shader NormalShader;

    [Header("STATS")]
    [SerializeField] int maxHealth;
    [SerializeField] float jumpHeight;
    [SerializeField] float speed;
    [SerializeField] float invisibilityDuration;


    [SerializeField] float horizMoveSpeed;
    [SerializeField] float vertVel;
    [SerializeField] Vector2 velocity;
    float deathMultiplier = 1;
    public bool isInvisible;
    float invisTime;
    public Vector3 spawnpoint;
    bool canJump = true;
    public int maxScene = 0;
    public bool hasGottenChest = false;

    [Header("Data")]
    int currentPotions;
    bool grounded;
    //bool pause;
    bool wasGrounded = false;






    // Start is called before the first frame update
    public void Init()
    {
        DontDestroyOnLoad(this.gameObject);
        gameObject.SetActive(true);
        SpriteMaterial.shader = NormalShader;
        invisTime = invisibilityDuration;
        refreshInvisBar();
        healthScript.healthValue = maxHealth;
        healthScript.maxHealth = maxHealth;
        canJump = true;
        anim.SetTrigger("Land");
        Time.timeScale = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        //Movement
        velocity.x = Mathf.Lerp(rb.velocity.x, Input.GetAxisRaw("Horizontal") * speed * deathMultiplier, Time.deltaTime * 10);

        velocity.y = rb.velocity.y;
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            //Debug.Log(grounded);
            if (grounded && canJump)
            {
                velocity.y = jumpHeight * deathMultiplier;
                anim.SetTrigger("Jump");
                anim.ResetTrigger("Land");
            }


        }
        rb.velocity = velocity;

        if (Input.GetKeyDown(KeyCode.E))
        {
            Medicine();
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            //turn invisible
            isInvisible = !isInvisible;
            if (isInvisible)
            {
                SpriteMaterial.shader = GreyscaleShader;
            }
            else
            {
                SpriteMaterial.shader = NormalShader;
            }
        }
        if (Input.GetKeyDown(KeyCode.Mouse0) && grounded)
        {
            //attack
            StartCoroutine(attackHitbox());
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
        Vector3 offset = -transform.up * 0.75f * transform.localScale.x;
        RaycastHit2D groundCheck1 = Physics2D.Raycast(transform.position + offset + transform.right * 0.3f * transform.localScale.x, Vector2.down, 0.1f, groundCheckMask);
        RaycastHit2D groundCheck2 = Physics2D.Raycast(transform.position + offset + transform.right * -0.3f * transform.localScale.x, Vector2.down, 0.1f, groundCheckMask);
        RaycastHit2D groundCheck3 = Physics2D.Raycast(transform.position + offset, Vector2.down, 0.1f, groundCheckMask);
        /*Debug.DrawRay(transform.position + transform.right * 0.3f - transform.up * 0.75f, Vector2.down, Color.blue, groundCheckMask);
        Debug.DrawRay(transform.position - transform.right * 0.3f - transform.up * 0.75f, Vector2.down, Color.blue, groundCheckMask);*/
        if (groundCheck1.collider != null || groundCheck2.collider != null || groundCheck3.collider != null)
        {
            grounded = true;

        }
        else
        {
            grounded = false;
        }
        if (isInvisible)
        {
            invisTime -= Time.deltaTime;
            if (invisTime <= 0)
            {
                invisTime = 0;
                isInvisible = false;
                SpriteMaterial.shader = NormalShader;
            }

            refreshInvisBar();
        }
        else if (invisTime != invisibilityDuration)
        {
            invisTime += Time.deltaTime;
            invisTime = invisTime > invisibilityDuration ? invisibilityDuration : invisTime;

            refreshInvisBar();
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
        anim.SetFloat("Horizontal", Mathf.Abs(Input.GetAxisRaw("Horizontal")));
        wasGrounded = grounded;
    }
    IEnumerator attackHitbox()
    {
        anim.SetTrigger("Attack");
        dmgHitbox.GetComponent<BoxCollider2D>().enabled = true;
        deathMultiplier = 0;
        canJump = false;
        yield return new WaitForSeconds(0.7f);
        canJump = true;
        deathMultiplier = 1;
        dmgHitbox.GetComponent<BoxCollider2D>().enabled = false;
    }
    public void Die()
    {
        StartCoroutine(death());
        canJump = true;

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


    }
    void refreshInvisBar()
    {
        invisBar.fillAmount = invisTime / invisibilityDuration;
    }
    public void LoadScene(int id)
    {
        //Debug.Log("LOADING " + id);
        maxScene = id > maxScene ? id : maxScene;
        isInvisible = false;
        refreshInvisBar();
        SpriteMaterial.shader = NormalShader;

        SceneManager.LoadScene(id);

        //dM.End();
        //dM.End();
        canJump = true;
        deathMultiplier = 1;
    }
    public void Restart()
    {
        Destroy(gameObject);
        LoadScene(0);
        //Destroy(gameObject);
        canJump = true;
    }

/*    public void Quit()
    {
        Application.Quit();
    }*/
}
