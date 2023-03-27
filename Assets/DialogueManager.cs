using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum speaker { mom, friend }
public enum dialogueType { start, response, choice, link, end }

enum dialogueState { empty, typing, full }


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
        public bool isPlayer;
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
        public speaker with;
        public List<DialogueComplex> lines;
    }

    [Header("Dependencies")]

    [SerializeField] TextMeshProUGUI speakerName;
    [SerializeField] TextMeshProUGUI dialogueText;
    [SerializeField] Image contactIcon;
    [SerializeField] Transform scrollParent;
    [SerializeField] Transform optionsParent;

    [Header("Prefabs")]
    [SerializeField] GameObject msgOtherPrefab;
    [SerializeField] GameObject msgPlayerPrefab;
    [SerializeField] GameObject brokenPlayerPrefab;

    [Header("Stats")]
    public float numCharPerLine = 15;
    public float optionBuffer = 10;
    public float optionGapSpacer = 40;
    public float optionCharacterWidth = 8;
    public float optionLineHeight = 10;

    public float speakDelay;
    /*    [Header("Stats")]
        public float scrollSpeed = 5;
        public float messageLineLength = 10;
        public float messageBubbleBuffer = 10;
        public float messageBubbleHorizontalBuffer = 10;
        public float messageGapSpacer = 40;
        public float messageCharacterWidth = 8;
        public float numCharPerLine = 10;
        float scrollParentStartBuffer = 10;
        float scrollMinimumPageLength = 90;*/

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

    private void OnEnable()
    {
        StartConversation(0);
    }

    public void StartConversation(int num)
    {
        currentConversationID = num;

        currentConversation = listConversations[currentConversationID];


        SetCurrentContact(findContactFromUser(currentConversation.with));

        NextDialogueLine();



    }
    public Person findContactFromUser(speaker speaker)
    {
        return people.Find(x => x.person == speaker);
    }

    public void SetCurrentContact(Person person)
    {
        /*contactName.text = person.name;
        contactIcon.sprite = person.sprites[0];*/
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
                CreateNewDialogue(currentConversation.lines[currentDialogueID].dialogue);
                currentDialogueID++;
            }
            else
            {
                CreateNewDialogue(currentConversation.lines[currentDialogueID].dialogue);
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
            GameObject _message = ch.available[i] ? Instantiate(msgPlayerPrefab, Vector3.zero, Quaternion.identity, optionsParent) : Instantiate(brokenPlayerPrefab, Vector3.zero, Quaternion.identity, optionsParent);


            _message.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, -optionMessagesLength);
            _message.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, _msgLines * optionLineHeight + optionBuffer);
            /*if (_msgLines <= 1)
            {
                _message.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, msg.text.Length * messageCharacterWidth + messageBubbleHorizontalBuffer);
            }*/

            optionMessagesLength += _msgLines * optionLineHeight + optionBuffer + optionGapSpacer / 2;
            _message.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = msg.text;
            _message.GetComponent<MessageChoice>().optionId = i;
            optionButtons.Add(_message);
        }

    }

    public void OptionChosen(int id)
    {
        CreateNewDialogue(currentConversation.lines[currentDialogueID].choice.messages[id]);
        currentDialogueID = currentConversation.lines[currentDialogueID].choice.to[id];
        foreach (GameObject obj in optionButtons)
        {
            Destroy(obj);
        }
    }

    public void EndConversation()
    {
        Debug.LogWarning("End of conversation");
    }
    public void StartNextDialogue(Dialogue dia)
    {
        
    }
    public void CreateNewDialogue(Dialogue dia)
    {

        /*int _msgLines = Mathf.CeilToInt((float)dia.text.Length / numCharPerLine);

        GameObject _message = dia.isPlayer ? Instantiate(msgPlayerPrefab, Vector3.zero, Quaternion.identity, scrollParent) : Instantiate(msgOtherPrefab, Vector3.zero, Quaternion.identity, scrollParent);


        //_message.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, -currentMessagesLength);
        //_message.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, _msgLines * messageLineLength + messageBubbleBuffer);
        if (_msgLines <= 1)
        {
            _message.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, dia.text.Length * messageCharacterWidth + messageBubbleHorizontalBuffer);
        }

        //currentMessagesLength += _msgLines * messageLineLength + messageBubbleBuffer + messageGapSpacer;
        //scrollValue += _msgLines * messageLineLength + messageBubbleBuffer + messageGapSpacer;
        _message.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = dia.text;
        //ScrollMessages(0);
*/
        if (currentConversation.lines[currentDialogueID].type == dialogueType.end)
        {
            EndConversation();
        }
        else
        {
            //StartCoroutine(NextDialogueAfterDelay(dia.delay));
        }
    }
/*    private void Update()
    {
        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            ScrollMessages(Input.GetAxis("Mouse ScrollWheel"));
        }
        Debug.Log(currentDialogueID);
    }
    public void ScrollMessages(float val)
    {
        scrollValue += -val * scrollSpeed;

        scrollValue = Mathf.Clamp(scrollValue, scrollParentStartBuffer, currentMessagesLength - scrollMinimumPageLength > scrollParentStartBuffer ? currentMessagesLength - scrollMinimumPageLength : scrollParentStartBuffer);

        scrollParent.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, scrollValue);




    }*/
    IEnumerator NextDialogueAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        NextDialogueLine();
    }
    IEnumerator TypeLetter(Dialogue dia)
    {
        yield return new WaitForSeconds(0);
        for (int i = 0; i < dia.text.Length; i++)
        {
            dialogueText.text = dia.text.Substring(0, i);
            yield return new WaitForSeconds(speakDelay);
        }


    }


}
