using System;
using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Tilemaps;
using UnityEngine;


[Serializable]
public struct Cutscene
{
    public string id;
    public float duration;
    public Animator[] animators;
    public GameObject[] objectsTurningOn;
    public GameObject[] objectsTurningOff;
    public GameObject[] prefabsToBeInstantiated;
    public Vector3[] prefabPositions;
    public bool playerCanMove;
}




public class CutsceneManager : MonoBehaviour
{
    public List<Cutscene> cutscenes;

    public Cutscene findCutsceneFromID(string ID)
    {
        return cutscenes.Find(x => x.id == ID);
    }

    public void PlayCutscene(string id)
    {
        StartCoroutine(CutsceneProcess(findCutsceneFromID(id)));
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
                obj.SetActive(true);
                
            }
        }
        foreach(GameObject obj in cutscene.objectsTurningOff)
        {
            if (obj.activeSelf)
            {
                cutsceneObjectsWerentOff.Add(obj);
                obj.SetActive(false);
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
        //Debug.Log('A');
        float _couldMove = GameObject.FindGameObjectWithTag("Player").GetComponent<RealPlayerController>().moveMult;
        GameObject.FindGameObjectWithTag("Player").GetComponent<RealPlayerController>().moveMult = 0;
        yield return new WaitForSeconds(cutscene.duration);
        GameObject.FindGameObjectWithTag("Player").GetComponent<RealPlayerController>().moveMult = _couldMove;
        //Debug.Log('B');
        foreach (GameObject obj in cutsceneObjectsWerentOn)
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
