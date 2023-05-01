using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Mathematics;
using UnityEngine.PlayerLoop;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public enum speaker {player, mom, friend, knight, samurai, hunter, medievalman, boss }
public enum dialogueType { start, response, choice, link, end }

enum dialogueState { empty, typing, skipTriggered, choosing, full }


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
        public UnityEvent evnt;
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
    [SerializeField] GameObject nextIndicator;

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

    public bool isGame; 


    [Header("Runtime")]

    public int currentConversationID;
    public Conversations currentConversation;
    public float scrollValue;
    //public float currentMessagesLength;
    public int currentDialogueID;
    public List<GameObject> optionButtons;
    [SerializeField] dialogueState dState;
    bool showing = false;
    DialogueComplex currentDialogue;

       /*private void OnEnable()
        {
            StartConversation(1);
        }*/

    public void StartConversation(int num)
    {
        ShowBox(true);
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
        //speakerName.text = findPersonFromSpeaker(speaker).name;///////////////////////////////////////////////////////////////////////////////////////////?/////////////////////////////
    }

    public void NextDialogueLine()
    {
        Debug.Log(1);
        if (currentDialogueID < currentConversation.lines.Count)
        {
            currentConversation.lines[currentDialogueID].evnt.Invoke();

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
            } else if (currentDialogueID > 0 && currentConversation.lines[currentDialogueID - 1].type == dialogueType.end)
            {
                EndConversation();
            }
            else
            {
                ShowNextDialogue(currentConversation.lines[currentDialogueID]);
                currentDialogueID++;
            }


        }
        else
        {
            Debug.Log("This happened");
            //out of messages
            EndConversation();
        }
    }

    public void GenerateChoiceOptions()
    {
        currentDialogue = currentConversation.lines[currentDialogueID];
        dState = dialogueState.choosing;
        float optionMessagesLength = 0;
        optionButtons.Clear();
        dialogueText.text = "";
        speakerPortrait.enabled = false;
        for (int i = 0; i < currentDialogue.choice.messages.Length; i++)
        {
            Dialogue msg = currentDialogue.choice.messages[i];
            Choice ch = currentDialogue.choice;
            int _msgLines = Mathf.CeilToInt((float)msg.text.Length / numCharPerLine);
            GameObject _message = ch.available[i] ? Instantiate(diaOptionPrefab, Vector3.zero, Quaternion.identity, optionsParent) : Instantiate(diaBrokenOptionPrefab, Vector3.zero, Quaternion.identity, optionsParent);


            _message.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, -optionMessagesLength);
            _message.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, _msgLines * optionLineHeight + optionBuffer);
            optionMessagesLength += _msgLines * optionLineHeight + optionBuffer + optionGapSpacer / 2;
            _message.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = msg.text;
            _message.GetComponent<DialogueChoice>().optionId = i;
            optionButtons.Add(_message);
        }

    }

    public void OptionChosen(int id)
    {
        speakerPortrait.enabled = true;
        Debug.Log(currentDialogue.dialogue.text);
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
        ShowBox(false); 
        if(isGame){
                    GameObject.FindGameObjectWithTag("Player").GetComponent<PlatformerPlayerController>().attackMult = 1;
        }else{
                    GameObject.FindGameObjectWithTag("Player").GetComponent<RealPlayerController>().moveMult = 1;
                     GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>().SetTrigger("Release");
        }
    }
    public void ShowNextDialogue(DialogueComplex dia)
    {
        Debug.Log(2);
        currentDialogue = dia;
        SetSpeaker(currentDialogue.dialogue.speaker);
        StartCoroutine(TypeLetters(dia.dialogue));
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PressedNextButton();
        }
    }
    public void PressedNextButton()
    {
        Debug.Log(3);
        if (dState == dialogueState.typing)
        {
            dState = dialogueState.skipTriggered;

        }
        else if (dState == dialogueState.full)
        {
            NextDialogueLine();
        }
    }
    private void FixedUpdate()
    {
        if(dState == dialogueState.full && !nextIndicator.activeSelf)
        {
            nextIndicator.SetActive(true);
        }
        else if(dState != dialogueState.full && nextIndicator.activeSelf)
        {
            nextIndicator.SetActive(false);
        }
    }

    IEnumerator TypeLetters(Dialogue dia)
    {
        Debug.Log(4);
        //Debug.Log("Triggered");
        dState = dialogueState.typing;
        //RectTransform textBase = dialogueText.GetComponent<RectTransform>();
        //Vector3 textBasePos = textBase.anchoredPosition;
        for (int i = 0; i <= dia.text.Length; i++)
        {
            //Debug.Log($"{i}, length {dia.text.Length} of {dia.text}, {dState}");
            if (dState != dialogueState.typing)
            {
                if(dState == dialogueState.skipTriggered)
                {
                    dialogueText.text = dia.text;
                    dState = dialogueState.full;
                }
                break;

            }
            dialogueText.text = dia.text[..i];
            //textBase.anchoredPosition = textBasePos + new Vector3(UnityEngine.Random.Range(-1,1), UnityEngine.Random.Range(-1,1f));
            yield return new WaitForSeconds(speakDelay);
            //textBase.anchoredPosition = textBasePos;
        }
        dState = dialogueState.full;
    }
    public void ShowBox(bool to)
    {
        Debug.Log(6);
        showing = to;
        transform.GetChild(0).gameObject.SetActive(to);
        if (to)
        {
            if(isGame){
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlatformerPlayerController>().attackMult = 0;
            }else{
                GameObject.FindGameObjectWithTag("Player").GetComponent<RealPlayerController>().moveMult = 0;
                GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>().SetTrigger("Idle");
            }

        }
    }

}
