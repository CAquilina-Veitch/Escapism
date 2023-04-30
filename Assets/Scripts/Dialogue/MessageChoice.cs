using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class MessageChoice : MonoBehaviour
{
    public int optionId;
    public void chooseDialogueChoice()
    {
        GameObject.FindGameObjectWithTag("Phone").GetComponent<PhoneManager>().OptionChosen(optionId);
    }
    [SerializeField] Sprite[] msgSprites;
    UnityEngine.UI.Image img;
    public void setBackground(int lines, int length)
    {
        img = GetComponent<UnityEngine.UI.Image>();
        //Debug.Log(lines + "" + length);
        if (lines > 1)
        {
            img.sprite = msgSprites[2];
        }
        else
        {
            img.sprite = length < 10 ? msgSprites[0] : msgSprites[1];
        }
    }
}
