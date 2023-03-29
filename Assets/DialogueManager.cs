using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum speaker {player, mom, friend }
public enum dialogueType { start, response, choice, link, end }

enum dialogueState { empty, typing, skipTriggered, full }


public class DialogueManager : MonoBehaviour
{
    [Serializable]
    public struct Person
    {
        public speaker person;
        public string name;
        public Sprite[] sprites;
    }


    [Serializable]
    public struct Dialogue
    {
        public speaker speaker;
        public string text;
        public int expression;
    }
    [Serializable]
    public struct Choice
    {
        public Dialogue[] messages;
        public int[] to;
        public bool[] available;
    }

    [Serializable]
    public struct DialogueComplex
    {
        public dialogueType type;
        public Dialogue dialogue;
        public Choice choice;
        public int link;
    }

    [Serializable]
    public struct Conversations
    {
        public int id;
        public List<DialogueComplex> lines;
    }

    [Header("Dependencies")]

    [SerializeField] TextMeshProUGUI speakerName;
    [SerializeField] TextMeshProUGUI dialogueText;
    [SerializeField] Image speakerPortrait;
    [SerializeField] Transform optionsParent;

    [Header("Prefabs")]
    [SerializeField] GameObject diaOptionPrefab;
    [SerializeField] GameObject diaBrokenOptionPrefab;

    [Header("Stats")]
    public float numCharPerLine = 15;
    public float optionBuffer = 10;
    public float optionGapSpacer = 40;
    public float optionCharacterWidth = 8;
    public float optionLineHeight = 10;

    public float speakDelay;

    [Header("Data")]
    public List<Person> people;
    public List<Conversations> listConversations;




    [Header("Runtime")]

    public int currentConversationID;
    public Conversations currentConversation;
    public float scrollValue;
    //public float currentMessagesLength;
    public int currentDialogueID;
    public List<GameObject> optionButtons;
    dialogueState dState;

    DialogueComplex currentDialogue;

    private void OnEnable()
    {
        StartConversation(0);
    }

    public void StartConversation(int num)
    {
        currentConversationID = num;

        currentConversation = listConversations[currentConversationID];


        //SetCurrentPerson(findPersonFromSpeaker(currentConversation.with));

        NextDialogueLine();



    }
    public Person findPersonFromSpeaker(speaker speaker)
    {
        return people.Find(x => x.person == speaker);
    }

    public void SetSpeaker(speaker speaker)
    {
        speakerPortrait.sprite = findPersonFromSpeaker(speaker).sprites[currentDialogue.dialogue.expression];
        //speakerName.text = findPersonFromSpeaker(speaker).name;///////////////////////////////////////////////////////////////////////////////////////////
    }

    public void NextDialogueLine()
    {
        if (currentDialogueID < currentConversation.lines.Count)
        {

            if (currentConversation.lines[currentDialogueID].type == dialogueType.choice)
            {
                Debug.LogWarning("CHOICE");
                GenerateChoiceOptions();
            }
            else if (currentConversation.lines[currentDialogueID].type == dialogueType.link)
            {
                currentDialogueID = currentConversation.lines[currentDialogueID].link;
                ShowNextDialogue(currentConversation.lines[currentDialogueID]);
                currentDialogueID++;
            }
            else
            {
                ShowNextDialogue(currentConversation.lines[currentDialogueID]);
                currentDialogueID++;
            }


        }
        else
        {
            //out of messages
            EndConversation();
        }
    }

    public void GenerateChoiceOptions()
    {
        float optionMessagesLength = 0;
        optionButtons.Clear();
        for (int i = 0; i < currentConversation.lines[currentDialogueID].choice.messages.Length; i++)
        {
            Dialogue msg = currentConversation.lines[currentDialogueID].choice.messages[i];
            Choice ch = currentConversation.lines[currentDialogueID].choice;
            int _msgLines = Mathf.CeilToInt((float)msg.text.Length / numCharPerLine);
            GameObject _message = ch.available[i] ? Instantiate(diaOptionPrefab, Vector3.zero, Quaternion.identity, optionsParent) : Instantiate(diaBrokenOptionPrefab, Vector3.zero, Quaternion.identity, optionsParent);


            _message.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, -optionMessagesLength);
            _message.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, _msgLines * optionLineHeight + optionBuffer);
            optionMessagesLength += _msgLines * optionLineHeight + optionBuffer + optionGapSpacer / 2;
            _message.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = msg.text;
            _message.GetComponent<MessageChoice>().optionId = i;
            optionButtons.Add(_message);
        }

    }

    public void OptionChosen(int id)
    {
        SetSpeaker(currentDialogue.choice.messages[id].speaker);
        StartCoroutine(TypeLetters(currentDialogue.choice.messages[id]));
        currentDialogueID = currentDialogue.choice.to[id];
        foreach (GameObject obj in optionButtons)
        {
            Destroy(obj);
        }
    }

    public void EndConversation()
    {
        Debug.LogWarning("End of conversation");
    }
    public void ShowNextDialogue(DialogueComplex dia)
    {
        currentDialogue = dia;
        if (dia.type == dialogueType.end)
        {
            EndConversation();
        }
        else
        {
            SetSpeaker(currentDialogue.dialogue.speaker);




            StartCoroutine(TypeLetters(dia.dialogue));
        }
    }
    private void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            if (dState == dialogueState.typing)
            {
                dState = dialogueState.skipTriggered;

            }
            else if (dState == dialogueState.full)
            {
                NextDialogueLine();
            }
        }
    }
    IEnumerator TypeLetters(Dialogue dia)
    {
        dState = dialogueState.typing;
        for (int i = 0; i < dia.text.Length; i++)
        {
            if (dState != dialogueState.typing)
            {
                if(dState == dialogueState.skipTriggered)
                {
                    dialogueText.text = dia.text;
                    dState = dialogueState.full;
                }
                break;

            }
            dialogueText.text = dia.text.Substring(0, i);
            yield return new WaitForSeconds(speakDelay);
        }
        dState = dialogueState.full;
    }


}
