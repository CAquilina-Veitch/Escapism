using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class RealPlayerController : MonoBehaviour
{

    [Header("Dependencies")]


    [SerializeField] Rigidbody2D rb;
    
    [SerializeField] LayerMask groundCheckMask;
    [SerializeField] SpriteRenderer sR;
    [SerializeField] Animator anim;
    public bool getOutOfBed = false;
    public float xposAfterStand = -2.28f;


    [Header("STATS")]
    [SerializeField] float speed;

    [SerializeField] float acceleration = 10;
    Vector2 velocity;
    public float moveMult = 0;

    public Interactable currentInteractable;
    public List<Interactable> inRange;


    private void OnEnable()
    {
        //Stand();
        sR.flipX = !getOutOfBed;
        //moveMult = 1;
    }


    // Start is called before the first frame update
    public void Init()
    {
        gameObject.SetActive(true);
        Time.timeScale = 1f;
    }
    public void Stand()
    {
        anim.SetFloat("SitAnimMult", 1);
        StartCoroutine(StandAnimation());
    }
    IEnumerator StandAnimation()
    {
        yield return new WaitForSeconds(2.13f);
        anim.SetTrigger("Release");
        sR.flipX = false;
        moveMult = 1;
        transform.position = new Vector3(xposAfterStand, transform.position.y);



    }
    public void GetOutOfBed()
    {
        anim.SetFloat("BedAnimMult", 1);
        StartCoroutine(BedAnimation());
    }
    IEnumerator BedAnimation()
    {
        yield return new WaitForSeconds(1.01f);
        anim.SetTrigger("Release");
        sR.flipX = true;
        moveMult = 1;
        transform.position = new Vector3(xposAfterStand, transform.position.y);
        GetComponent<Goto>().go();
    }
    public void CanWalk(bool to)
    {
        moveMult = to ? 1 : 0;
    }


    public void Sleep()
    {
        anim.SetFloat("SitAnimMult", 1);
        moveMult = 0;
        sR.enabled = false;
    }
    public void ChangeSpeed(float to)
    {
        speed = to;
    }

    // Update is called once per frame
    void Update()
    {
        //Movement
        velocity.x = Mathf.Lerp(rb.velocity.x, Input.GetAxisRaw("Horizontal") * speed, Time.deltaTime * acceleration);
        velocity.y = rb.velocity.y;

        rb.velocity = velocity* moveMult;


        if (Input.GetKeyDown(KeyCode.E)&&moveMult!=0)
        {
            //currentInteract.Invoke();
            CheckInteractable();
            currentInteractable.interact.Invoke();
        }

    }


    private void FixedUpdate()
    {

        CheckInteractable();


        //animations
        if (Mathf.Abs(rb.velocity.x) > 0.1f)
        {
            sR.flipX = rb.velocity.x < 0 ? true : false;
        }

        
        anim.SetFloat("Horizontal", Mathf.Abs(Input.GetAxisRaw("Horizontal")*moveMult));
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
            currentInteractable = null;
            if (inRange.Count != 0)
            {
                currentInteractable = inRange[inRange.Count - 1];
            }
        }
    }
    public void CheckInteractable()
    {
        if(currentInteractable == null && inRange.Count != 0)
        {
            currentInteractable = inRange[inRange.Count - 1];
        }
    }

}
