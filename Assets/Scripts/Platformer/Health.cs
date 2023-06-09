using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public attackOwner owner;
    PlatformerPlayerController player;
    Animator anim;
    EnemyScript enemy;
    BossController boss;
    //public bool isPlayer;
    //[SerializeField] Rigidbody2D rb;
    //[SerializeField] float knockBack;

    public bool killable = true;
    [SerializeField] Image[] Hearts;
    [SerializeField] Image BossHealth;
    //[Header("Dont set these in inspector, but in their controller scripts")]
    public int maxHealth=3;
    public int healthValue=3;
    SpriteRenderer sR;
    public float stunFor;


    private void Awake()
    {
        sR = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        healthValue = maxHealth;
        if(owner == attackOwner.boss)
        {
            BossHealth.transform.parent.gameObject.SetActive(true);
        }
        
        if(owner == attackOwner.player)
        {
            player = GetComponent<PlatformerPlayerController>();
        }
        else if (owner == attackOwner.skeleton)
        {
            enemy = GetComponent<EnemyScript>();
        }
        else if (owner == attackOwner.boss)
        {
            boss = GetComponent<BossController>();
        }
    }

    public void setHealth (int to)
    {
        //maxHealth = to;
        healthValue = to;
    }
    public void HealthChange(int dmg)
    {
        if (owner == attackOwner.boss)
        {
            if (boss.isDead)
            {
                BossHealth.transform.parent.gameObject.SetActive(false);
                return;
            }
            
            
        }
        if (!killable&&healthValue==1)
        {
            return;
        }
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









        if(owner == attackOwner.player)
        {
            if (player.attackTime > 0)
            {
                player.attackInterupted = true;
                player.attackTime = stunFor;
                stunFor = 0;
            }
            

        }else if (owner == attackOwner.skeleton)
        {
            if (enemy.attackTime > 0)
            {
                enemy.attackInterupted = true;
                enemy.attackTime = stunFor;
                stunFor = 0;
                
            }
            
        }else if (owner == attackOwner.boss)
        {
            BossHealth.fillAmount = (float)healthValue / (float)maxHealth;
        }








        if (healthValue <= 0)
        {
            //die

            if (owner==attackOwner.player)
            {
                player.Die();
                UpdateHealthBar();
            }
            else if (owner == attackOwner.skeleton)
            {
                enemy.Die();
            }else if (owner == attackOwner.boss)
            {
                boss.Die();
                this.enabled = false;
            }

        }
        else
        {
            if (owner == attackOwner.player)
            {
                UpdateHealthBar();
            }
            anim.SetTrigger("Hurt");
        }

    }
    public void UpdateHealthBar()
    {
        for(int i = 0; i < 3; i++) 
        { 
            Hearts[i].enabled = healthValue>= i+1?true:false;
        }

    }
    IEnumerator ColourFlash( bool heal)
    {

        //GetComponent<Animator>().SetTrigger("Hurt");
        sR.color = heal ? Color.green : Color.red;
        yield return new WaitForSeconds(0.25f);
        sR.color = Color.white;
        if (!heal)
        {
            GetComponent<Animator>().ResetTrigger("Hurt");
        }
    }





}
