using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;


[Serializable]
public struct Cutscene
{
    public int id;
    public float duration;
    public Animator[] animators;
    public GameObject[] objectsTurningOn;
    public GameObject[] objectsTurningOff;
    public GameObject[] prefabsToBeInstantiated;
    public Vector3[] prefabPositions;
}




public class CutsceneManager : MonoBehaviour
{
    public Cutscene[] cutscenes;

    int currentCutscene;


    public void PlayCutscene(int num)
    {
        StartCoroutine(CutsceneProcess(cutscenes[num]));
    }

    public void PlayNextCutscene()
    {
        currentCutscene++;
        PlayCutscene(currentCutscene);
    }

    IEnumerator CutsceneProcess(Cutscene cutscene)
    {
        Dictionary<GameObject, Vector3> initialPos;
        List<GameObject> cutsceneObjectsWerentOn = new List<GameObject>();
        List<GameObject> cutsceneObjectsWerentOff = new List<GameObject>();
        foreach(GameObject obj in cutscene.objectsTurningOn)
        {
            if (!obj.activeSelf)
            {
                cutsceneObjectsWerentOn.Add(obj);
            }
        }
        foreach(GameObject obj in cutscene.objectsTurningOff)
        {
            if (obj.activeSelf)
            {
                cutsceneObjectsWerentOff.Add(obj);
            }
        }
        foreach(Animator anim in cutscene.animators)
        {
            anim.SetTrigger($"Cutscene_{cutscene}");
        }
        List<GameObject> cutscenePrefabsInstantiated = new List<GameObject>();
        for (int i = 0; i<cutscene.prefabsToBeInstantiated.Length; i++)
        {
            
            GameObject temp = Instantiate(cutscene.prefabsToBeInstantiated[i], cutscene.prefabPositions[i], Quaternion.identity);
            if (temp.TryGetComponent<Animator>(out Animator _prefabAnim))
            {
                _prefabAnim.SetTrigger($"Cutscene_{cutscene}");
            }
            cutscenePrefabsInstantiated.Add(temp);
        }

        yield return new WaitForSeconds(cutscene.duration);

        foreach(GameObject obj in cutsceneObjectsWerentOn)
        {
            obj.SetActive(false);
        }
        foreach(GameObject obj in cutsceneObjectsWerentOff)
        {
            obj.SetActive(true);
        }
        foreach(Animator anim in cutscene.animators)
        {
            anim.SetTrigger($"Cutscene_End");
        }

        foreach(GameObject prefab in cutscenePrefabsInstantiated)
        {
            Destroy(prefab);
        }


    }
}
