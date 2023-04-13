using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectionalCamera : MonoBehaviour
{
    [SerializeField] Transform player;

    [SerializeField] float[] edges;
    [SerializeField] float yOffset;
    [SerializeReference]int currentCameraPos;
    [SerializeField] float radius = 9;
    [SerializeField] bool yLock = false;
    private void FixedUpdate()
    {
        for (int i = 0; i < edges.Length; i++)
        {
            if (player.transform.position.x > edges[i] && player.transform.position.x < edges[i + 1])
            {
                currentCameraPos = i;
                break;
            }
        }
        Vector3 temp = Vector3.Lerp(transform.position, player.position+new Vector3(0,yOffset),Time.deltaTime*10);
        temp.y = yLock ? transform.position.y:temp.y;
        transform.position = new Vector3(Mathf.Clamp(temp.x,edges[currentCameraPos]+radius,edges[currentCameraPos+1]-radius), temp.y, -10);
    }
}
