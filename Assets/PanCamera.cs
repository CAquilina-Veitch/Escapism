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
    public SpriteRenderer chair;
    public bool inBed;
    Color[] clrs = {new Color(0,0,0,0),new Color(0,0,0,56),Color.white };
    private void OnEnable()
    {
        cam = GetComponent<Camera>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<SpriteRenderer>();
        
        StartCoroutine(Pan());
    }
    
    IEnumerator Pan()
    {
        cam.orthographicSize = orthographicBounds.x;
        transform.position = panFrom;

        float t = 0;

        while (t <= fadeTime)
        {
            if (t <= fadeTime / 2)
            {
                player.color = Color.Lerp(clrs[0], clrs[01], t / (fadeTime/2));
                chair.color = Color.Lerp(clrs[0], clrs[01], t / (fadeTime/2));
            }
            else
            {
                player.color = Color.Lerp(clrs[01], clrs[02], (t / (fadeTime / 2)) -1);
                chair.color = Color.Lerp(clrs[01], clrs[02], (t / (fadeTime / 2)) - 1);

            }

            t += 1 / 60f;
            yield return new WaitForSeconds(1 / 60f);
        }
        chair.color = clrs[02];
        player.color = clrs[02];

        t = 0;

        while (t <= panTime)
        {
            cam.orthographicSize = Mathf.Lerp(orthographicBounds.x, orthographicBounds.y, t/panTime);
            transform.position = Vector3.Lerp(panFrom, panTo, t / panTime);
            t += 1/60f;
            yield return new WaitForSeconds(1/60f);
        }
        cam.orthographicSize = orthographicBounds.y;
        transform.position = panTo;

        if (inBed)
        {
            player.GetComponent<RealPlayerController>().GetOutOfBed();
        }
        else
        {
            player.GetComponent<RealPlayerController>().Stand();
        }



        GetComponent<SectionalCamera>().enabled = true;
        enabled = false;
        Interactable[] interactables = FindObjectsOfType<Interactable>();
        foreach (Interactable interactable in interactables)
        {
            interactable.Show();
        }
    }
}
