using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public bool isPlayer;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] float knockBack;

    [SerializeField] Image Heart1;
    [SerializeField] Image Heart2;
    [SerializeField] Image Heart3;
    [Header("Dont set these in inspector, but in their controller scripts")]
    public int maxHealth=1000;
    public int healthValue=1000;
    SpriteRenderer sR;



    // Start is called before the first frame update
    void Start()
    {
        sR = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void HealthChange(int dmg)
    {
        StartCoroutine(ColourFlash(dmg > 0));
        if (dmg > 0)
        {
            //heal
            healthValue += dmg;
            healthValue = healthValue > maxHealth ? maxHealth : healthValue;
        }
        else
        {
            //take damage
            healthValue += dmg;
            rb.velocity = Vector3.zero;

        }

        if (healthValue <= 0)
        {
            //die

            if (isPlayer)
            {
                GetComponent<PlatformerPlayerController>().Die();
                UpdateHealthBar();
            }
            else
            {
                GetComponent<EnemyScript>().Die();
            }

        }
        else
        {
            if (isPlayer)
            {
                UpdateHealthBar();
            }
            StartCoroutine(HurtDelay());
        }

    }
    public void UpdateHealthBar()
    {
        Heart1.enabled = healthValue >= 1 ? true : false;
        Heart2.enabled = healthValue >= 2 ? true : false;
        Heart3.enabled = healthValue >= 3 ? true : false;

    }
    IEnumerator ColourFlash( bool heal)
    {
        
        yield return new WaitForSeconds(0.25f);
        sR.color = heal ? Color.green : Color.red;
        yield return new WaitForSeconds(0.25f);
        sR.color = Color.white;
        if (!heal)
        {
            GetComponent<Animator>().ResetTrigger("Hurt");
        }
    }
    IEnumerator HurtDelay()
    {
        yield return new WaitForSeconds(0.3f);
        GetComponent<Animator>().SetTrigger("Hurt");

    }




}
