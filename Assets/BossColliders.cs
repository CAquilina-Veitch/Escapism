using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossColliders : MonoBehaviour
{
    private void OnEnable()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlatformerPlayerController>().StopBossCollision(); 
    }
}
