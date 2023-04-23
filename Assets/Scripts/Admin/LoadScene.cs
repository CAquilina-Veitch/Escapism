using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    public Object NextScene;
    public float delay;

    public void LoadNext()
    {
        StartCoroutine(go());
    }
    IEnumerator go()
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(NextScene.name);
    }
}
