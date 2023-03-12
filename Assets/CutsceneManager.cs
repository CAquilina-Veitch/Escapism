using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class CutsceneManager : MonoBehaviour
{
    int currentCutscene;
    
    [SerializeField] float[] cutsceneDurations;
    [SerializeField] Animator[] cutsceneAnimations;

    [Header("Data that will be used to be set in script")]
    [SerializeField] GameObject[] cutsceneObjects;

    [SerializeField] int[][] cutsceneObjectsOn;
    [SerializeField] int[][] cutsceneObjectsOff;

    [SerializeField] int[][] cutscenePrefabsInstantiate;
    [SerializeField] Vector2[][] cutscenePrefabsPosition;
      

    [Header("Temporary storage to restore after cutscene")]
    [SerializeField] int[] cutsceneObjectsWereOn;
    [SerializeField] GameObject[] cutscenePrefabsInstantiated;



    public void PlayCutscene(int num)
    {

    }

    public void PlayNextCutscene()
    {
        currentCutscene++;
        PlayCutscene(currentCutscene);
    }







    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
