using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public bool isPlayer;
    //[SerializeField] Rigidbody2D rb;
    //[SerializeField] float knockBack;

    [SerializeField] Image[] Hearts;
    //[Header("Dont set these in inspector, but in their controller scripts")]
    int maxHealth=1000;
    int healthValue=1000;
    SpriteRenderer sR;



    // Start is called before the first frame update
    void Start()
    {
        sR = GetComponent<SpriteRenderer>();
    }
    public void setHealth (int to)
    {
        maxHealth = to;
        healthValue = to;
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
            //rb.velocity = Vector3.zero;

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
        for(int i = 0; i < 3; i++) 
        { 
            Hearts[i].enabled = healthValue>= i-1?true:false;
        }

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
