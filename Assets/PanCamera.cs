using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanCamera : MonoBehaviour
{
    Camera cam;
    public float panTime;
    public float fadeTime = 2;
    public Vector2 orthographicBounds = new Vector2(2,5);
    public Vector3 panFrom = new Vector3(0, 0, -10);
    public Vector3 panTo = new Vector3(0, 0, -10);
    SpriteRenderer player;
    private void OnEnable()
    {
        cam = GetComponent<Camera>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<SpriteRenderer>();
        StartCoroutine(Pan());
    }
    IEnumerator Pan()
    {
        bool y = true;
        float t = 0;
        while (t <= fadeTime)
        {
            if (t <= fadeTime / 2)
            {
                player.color = Color.Lerp(Color.clear, Color.black, t / (fadeTime/2));
            }
            else
            {
                player.color = Color.Lerp(Color.black, Color.white, (t / (fadeTime / 2)) -1);
                if (y)
                {
                    player.GetComponent<RealPlayerController>().Stand();
                    y = false;
                }
            }

            t += 1 / 60f;
            yield return new WaitForSeconds(1 / 60f);
        }

        t = 0;
        while (t <= panTime)
        {
            cam.orthographicSize = Mathf.Lerp(orthographicBounds.x, orthographicBounds.y, t/panTime);
            transform.position = Vector3.Lerp(panFrom, panTo, t / panTime);
            t += 1/60f;
            yield return new WaitForSeconds(1/60f);
        }
        GetComponent<SectionalCamera>().enabled = true;
        enabled = false;
    }
}
