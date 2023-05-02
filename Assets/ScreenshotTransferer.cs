using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScreenshotTransferer : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Object NextScene;
    public GameObject cam;

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
        SceneManager.LoadScene(NextScene.name);
        yield return new WaitForFixedUpdate();
        transform.position = GameObject.FindGameObjectWithTag("MainCamera").transform.position;
        transform.position = new Vector3(transform.position.x, transform.position.y, -0.5f);
        //transform.position = new Vector3(-3.93f,-0.991f,-0.5f);
        transform.localScale = Vector3.one * 0.1115f;
        //transform.localScale = Vector3.one * 0.32f;
        SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetActiveScene());
        //GameObject.FindGameObjectWithTag("Player").GetComponent<RealPlayerController>().Stand()
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
