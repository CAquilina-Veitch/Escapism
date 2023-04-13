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



    [Header("STATS")]
    [SerializeField] float speed;

    [SerializeField] float acceleration = 10;
    Vector2 velocity;
    Vector3 spawnpoint;



    public UnityEvent currentInteract;
    public List<Interactable> inRange;





    // Start is called before the first frame update
    public void Init()
    {
        gameObject.SetActive(true);
        Time.timeScale = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        //Movement
        velocity.x = Mathf.Lerp(rb.velocity.x, Input.GetAxisRaw("Horizontal") * speed, Time.deltaTime * acceleration);
        velocity.y = rb.velocity.y;

        rb.velocity = velocity;


        if (Input.GetKeyDown(KeyCode.E))
        {
            currentInteract.Invoke();
        }

    }


    private void FixedUpdate()
    {




        //animations
        if (Mathf.Abs(rb.velocity.x) > 0.1f)
        {
            sR.flipX = rb.velocity.x < 0 ? true : false;
        }

        
        anim.SetFloat("Horizontal", Mathf.Abs(Input.GetAxisRaw("Horizontal")));
    }
    public void ChangeInteraction(Interactable interact, bool add)
    {
        if (add)
        {
            inRange.Add(interact);
            currentInteract = interact.interact;
        }
        else
        {
            inRange.Remove(interact);
            if (inRange.Count == 0)
            {
                currentInteract = null;
            }
        }
    }
}
