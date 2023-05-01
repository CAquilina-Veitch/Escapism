using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goto : MonoBehaviour
{
    public Vector3 position;
    public Vector3 Scale;

    public void go()
    {
        if (position != Vector3.zero)
        {
            transform.position = position;
        }
        if (Scale != Vector3.zero)
        {
            transform.localScale = Scale;
        }




    }



}
