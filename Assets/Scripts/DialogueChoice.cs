using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueChoice : MonoBehaviour
{
    public int optionId;
    public void chooseDialogueChoice()
    {
        GameObject.FindGameObjectWithTag("DialogueController").GetComponent<DialogueManager>().OptionChosen(optionId);
    }
}
