using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goto : MonoBehaviour
{
    public Vector3 position;
    public Vector3 Scale;

    public void go(){
        transform.position = position;
        transform.localScale = Scale;
    }



}
