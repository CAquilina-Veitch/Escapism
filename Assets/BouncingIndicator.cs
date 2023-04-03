using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncingIndicator : MonoBehaviour
{
    RectTransform t;
    [SerializeField] float bounceMult;
    [SerializeField] float bounceVel;
    Vector3 basePos;
    float i;

    private void OnEnable()
    {
        t = GetComponent<RectTransform>();
        basePos = t.anchoredPosition;
    }

    private void FixedUpdate()
    {
        i += Time.deltaTime * bounceVel;
        t.anchoredPosition = basePos + new Vector3(0,bounceMult*Mathf.Sin(i));
    }

    private void OnDisable()
    {
        t.anchoredPosition = basePos;
    }
}
