using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{

    public bool killable;

    public float speed;
    public float actionTime;
    public int currentPhase = 1;

    public float currentAttackDelay;

    public Attack[] atks;
    public Vector2 atkMotor;
    public float xMotor;
    public Rigidbody2D rb;
    public float attackMult = 1;

    Rigidbody2D prb;

    //charge
    public float chargeStartup = 0.2f;




    private void OnEnable()
    {
        prb = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (currentAttackDelay == 0)
        {
            Attack(Random.Range(0, currentPhase));
        }


        bool was = currentAttackDelay == 0;

        currentAttackDelay -= currentAttackDelay == 0 ? 0 : Time.deltaTime;

        if (currentAttackDelay <= 0 && !was)
        {
            currentAttackDelay = 0;
            attackMult = 1;

        }
        else if (currentAttackDelay != 0)
        {
            attackMult = 0;

        }

        rb.velocity = new Vector2(xMotor * attackMult, rb.velocity.y)+atkMotor;



    }
    public void Attack(int num)
    {
        currentAttackDelay = atks[num].attackDelay();
        switch (num)
        {
            case 0:
                StartCoroutine(Charge());
                break;
            case 1:
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
        atkMotor = Vector2.zero;
        yield return new WaitForSeconds(atks[0].preDelay / 2);
        atkMotor = new Vector2(15* playerDirection, 0);
        yield return new WaitForSeconds(atks[0].activeTime);
        atkMotor = Vector2.zero;



    }


}