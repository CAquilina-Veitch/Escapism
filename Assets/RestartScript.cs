using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartScript : MonoBehaviour
{
  void Awake()
    {
        SceneManager.LoadScene(0);
    }
}
