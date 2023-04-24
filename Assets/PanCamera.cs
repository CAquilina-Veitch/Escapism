using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanCamera : MonoBehaviour
{
    Camera cam;
    float panTime;
    public Vector2 orthographicBounds = new Vector2(2,5);
    public Vector3 panFrom = new Vector3(0, 0, -10);
    public Vector3 panTo = new Vector3(0, 0, -10);
    private void OnEnable()
    {
        cam = GetComponent<Camera>();
        StartCoroutine(Pan());
    }


    IEnumerator Pan()
    {
        float t = 0;
        while (t <= panTime)
        {
            cam.orthographicSize = Mathf.Lerp(orthographicBounds.x, orthographicBounds.y, t);
            t += 1f / 60f;
            yield return new WaitForSeconds(1f / 60f);
        }
        GetComponent<SectionalCamera>().enabled = true;
    }





}
