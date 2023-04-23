using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RequirementsToEnable : MonoBehaviour
{
    public List<string> Requirements;

    public GameObject obj;

    public void FinishRequirement(string requirement)
    {
        if (Requirements.Contains(requirement))
        {
            Requirements.Remove(requirement);
        }
        if (Requirements.Count == 0)
        {
            obj.SetActive(true);
        }
    }

    private void OnEnable()
    {
        obj.SetActive(false);
    }
}
