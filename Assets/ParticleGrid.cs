using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleGrid : MonoBehaviour
{
    ParticleSystem ps;
    ParticleSystem.EmitParams ep;


    public Vector2 bounds = new Vector2(1, 1);
    public Vector2Int resolution = new Vector2Int(10, 10);

    public bool isGlitching;
    public float random = 0.02f;

    Vector2 scale;
    Vector3 boundsHalf;
    float delay = 0.1f;
    IEnumerator Glitch()
    {
        while (isGlitching)
        {
            for (int i = 0; i < resolution.x; i++)
            {
                for (int j = 0; j < resolution.y; j++)
                {
                    if (Random.Range(0, 1f) > random)
                    {
                        continue;
                    } 
                    Vector3 position = Vector3.zero;

                    position.x = (i * scale.x) - boundsHalf.x;
                    position.y = (j * scale.y) - boundsHalf.y;
                    position.z = 0;

                    ep.position = position;
                    ps.Emit(ep, 1);



                }
            }
            yield return new WaitForSeconds(delay);
        }
    }

    void OnEnable()
    {
        ps = GetComponent<ParticleSystem>();

        boundsHalf = bounds / 2.0f;


        scale.x = 1f / 10;
        scale.y = 1f / 10;

         ep = new ParticleSystem.EmitParams();
        
        
    }
    public void SetGlitch(bool to)
    {
        if(!isGlitching&& to)
        {
            isGlitching = to;
            StartCoroutine(Glitch());
        }
        isGlitching = to;
    }
    public void glitchFor(float secs)
    {
        StartCoroutine(LimGlitch(secs));
    }
    IEnumerator LimGlitch(float secs)
    {
        float t = 0;
        while (t <secs)
        {
            for (int i = 0; i < resolution.x; i++)
            {
                for (int j = 0; j < resolution.y; j++)
                {
                    if (Random.Range(0, 1f) > random)
                    {
                        continue;
                    }
                    Vector3 position = Vector3.zero;

                    position.x = (i * scale.x) - boundsHalf.x;
                    position.y = (j * scale.y) - boundsHalf.y;
                    position.z = 0;

                    ep.position = position;
                    ps.Emit(ep, 1);



                }
            }
            yield return new WaitForSeconds(delay);
            t += delay;
        }
    }
}