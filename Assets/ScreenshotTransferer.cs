using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScreenshotTransferer : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public float preTime;
    public float time;
    public float i;
    bool moving;
    public Vector3[] scale;
    public Vector3[] pos;
    public Object NextScene;

    private void OnEnable()
    {
        DontDestroyOnLoad(gameObject);
    }
    IEnumerator goToRealLife()
    {
        yield return new WaitForEndOfFrame();   
        // Take a screenshot of the current screen
        Texture2D texture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        texture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        texture.Apply();

        // Convert the screenshot texture to a sprite
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);

        // Set the sprite as the sprite renderer's sprite
        spriteRenderer.sprite = sprite;
        moving = true;
        SceneManager.LoadScene(NextScene.name);
    }
    private void FixedUpdate()
    {
        if (moving)
        {
            if (i < preTime)
            {
                pos[0] = GameObject.FindGameObjectWithTag("MainCamera").transform.position;
                pos[0].z = -0.5f;
                transform.position = pos[0];
            } 
            else if (i < preTime + time)
            {
                transform.localScale = Vector3.Lerp(scale[0], scale[1],(i-preTime)/time);
                transform.position = Vector3.Lerp(pos[0], pos[1],(i-preTime)/time);
            }
            else
            {
                moving = false;
                transform.localScale = scale[1];
                transform.position = pos[1];
                SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetActiveScene());
            }
            i += Time.deltaTime ;
        }
    }
    public void TransitionToReal()
    {
        StartCoroutine(goToRealLife());
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            TransitionToReal();
        }
    }
}
