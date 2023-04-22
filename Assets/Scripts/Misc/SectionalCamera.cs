using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectionalCamera : MonoBehaviour
{
    [SerializeField] Transform player;
    Rigidbody2D p_rb;

    [SerializeField] float[] edges;
    [SerializeField] float ymin, yacc, defaultYOffset, currentYOffset,resetacc;
    [SerializeReference] int currentCameraPos;
    [SerializeField] float radius = 9;
    [SerializeField] bool yLock = false;
    private void OnEnable()
    {
        p_rb = player.GetComponent<Rigidbody2D>();
    }
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
        currentYOffset = Mathf.Max(Mathf.Lerp(currentYOffset, defaultYOffset + Mathf.Min(0, p_rb.velocity.y<-1?p_rb.velocity.y+1:0), yacc *Time.deltaTime),ymin);
        currentYOffset = p_rb.velocity.y == 0 ? Mathf.Lerp(currentYOffset, defaultYOffset, resetacc) : currentYOffset;
        Vector3 temp = Vector3.Lerp(transform.position, player.position+new Vector3(0,currentYOffset),Time.deltaTime*10);
        temp.y = yLock ? transform.position.y:temp.y;
        transform.position = new Vector3(Mathf.Clamp(temp.x,edges[currentCameraPos]+radius,edges[currentCameraPos+1]-radius), temp.y, -10);
    }
}
