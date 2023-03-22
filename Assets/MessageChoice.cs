using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageChoice : MonoBehaviour
{
    public int optionId;
    public void chooseDialogueChoice()
    {
        GameObject.FindGameObjectWithTag("Phone").GetComponent<PhoneManager>().OptionChosen(optionId);
    }
}
