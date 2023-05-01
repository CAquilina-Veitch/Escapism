using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum user {mom, friend}
public enum messageType { start, response, choice, link, end, noChoice}


public class PhoneManager : MonoBehaviour
{
    [Serializable]
    public struct Contact
    {
        public user user;
        public string name;
        public Sprite photo;
    }


    [Serializable]
    public struct Message
    {
        //[Header("Is the player talking?")]
        public bool isPlayer;
        //[Header("Dialogue")]
        public string text;
        public float delay;
    }
    [Serializable]
    public struct Choice
    {
        public Message[] messages;
        public int[] to;
        public bool[] available;
    }

    [Serializable]
    public struct MessageComplex
    {
        public messageType type;
        public Message message;
        public Choice choice;
        public int link;
    }

    [Serializable]
    public struct MessageConversations
    {
        public int id;
        public user with;
        public List<MessageComplex> messages;
    }

    [Header("Dependencies")]

    [SerializeField] TextMeshProUGUI contactName;
    [SerializeField] Image contactIcon;
    [SerializeField] Transform scrollParent;
    [SerializeField] Transform optionsParent;

    [Header("Prefabs")]
    [SerializeField] GameObject msgOtherPrefab;
    [SerializeField] GameObject msgPlayerPrefab;
    [SerializeField] GameObject brokenPlayerPrefab;

    [Header("Stats")]
    public float scrollSpeed = 5;
    public float messageLineLength = 10;
    public float messageBubbleBuffer = 10;
    public float messageBubbleHorizontalBuffer = 10;
    public float messageGapSpacer = 40;
    public float messageCharacterWidth = 8;
    public float numCharPerLine = 10;
    float scrollParentStartBuffer = 10;
    float scrollMinimumPageLength = 90;
    bool started = false;
    bool showingPhone = false;
    [Header("Data")]
    public List<Contact> contacts;
    public List<MessageConversations> listMessageConversations;
    



    [Header("Runtime")]

    public int currentConversationID;
    public MessageConversations currentConversation;
    public float scrollValue;
    public float currentMessagesLength;
    public int currentMessageID;
    public List<GameObject> optionButtons;

   private void OnEnable()
    {
        //Time.timeScale = 100;
        StartConversation(1);
    }

    public void StartConversation(int num)
    {
        if (started)
        {
            return;
        }
        currentConversationID = num;

        currentConversation = listMessageConversations[currentConversationID];


        SetCurrentContact(findContactFromUser(currentConversation.with));

        NextMessage();
        started = true;


    }
    public Contact findContactFromUser(user user)
    {
        return contacts.Find(x => x.user == user);
    }

    public void SetCurrentContact(Contact contact)
    {
        contactName.text = contact.name;
        contactIcon.sprite= contact.photo;
    }

    public void NextMessage()
    {
        if (currentMessageID < currentConversation.messages.Count)
        {

            if (currentConversation.messages[currentMessageID].type == messageType.choice)
            {
                //Debug.LogWarning("CHOICE");
                GenerateChoiceOptions();
            }else if(currentConversation.messages[currentMessageID].type == messageType.link)
            {
                currentMessageID = currentConversation.messages[currentMessageID].link;
                CreateNewMessage(currentConversation.messages[currentMessageID].message);
                currentMessageID++;
            }else if (currentConversation.messages[currentMessageID].type == messageType.noChoice)
            {
                //Debug.LogWarning("NOCHOICE");
                GenerateChoiceOptions();
                currentMessageID++;
                StartCoroutine(NextMessageAfterDelay(currentConversation.messages[currentMessageID-1].message.delay));
                StartCoroutine(DestroyFakeOptions(currentConversation.messages[currentMessageID-1].message.delay));
            }
            else
            {
                CreateNewMessage(currentConversation.messages[currentMessageID].message);
                currentMessageID++;
            }
            

        }
        else
        {
            //out of messages
            EndConversation();
        }
    }
    IEnumerator DestroyFakeOptions(float delay)
    {
        yield return new WaitForSeconds(delay);
        foreach (GameObject obj in optionButtons)
        {
            Destroy(obj);
        }
    }

    public void GenerateChoiceOptions()
    {
        float optionMessagesLength = 0;
        optionButtons.Clear();
        for(int i = 0; i < currentConversation.messages[currentMessageID].choice.messages.Length; i++)
        {
            Message msg = currentConversation.messages[currentMessageID].choice.messages[i];
            Choice ch = currentConversation.messages[currentMessageID].choice;
            int _msgLines = Mathf.CeilToInt((float)msg.text.Length / numCharPerLine);
            bool unbroken = ch.available[i];
            GameObject _message = unbroken ? Instantiate(msgPlayerPrefab, Vector3.zero, Quaternion.identity, optionsParent) : Instantiate(brokenPlayerPrefab, Vector3.zero, Quaternion.identity, optionsParent);

            if (unbroken)
            {
                _message.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, -optionMessagesLength);
                _message.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, _msgLines * messageLineLength + messageBubbleBuffer);
                if (_msgLines <= 1)
                {
                    _message.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, msg.text.Length * messageCharacterWidth + messageBubbleHorizontalBuffer);
                }
                _message.GetComponent<MessageChoice>().setBackground(_msgLines, msg.text.Length);

                optionMessagesLength += _msgLines * messageLineLength + messageBubbleBuffer + messageGapSpacer / 2;
                _message.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = msg.text;
                _message.GetComponent<MessageChoice>().optionId = i;
                optionButtons.Add(_message);
            }
            else
            {
                _message.GetComponent<BrokenMessage>().Work(optionMessagesLength, _msgLines, msg.text, messageLineLength,messageBubbleBuffer,messageCharacterWidth,messageBubbleHorizontalBuffer);
                

                

                //_message.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, -optionMessagesLength);
                //_message.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, _msgLines * messageLineLength + messageBubbleBuffer);
                /*if (_msgLines <= 1)
                {
                    _message.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, msg.text.Length * messageCharacterWidth + messageBubbleHorizontalBuffer);
                }*/
                //_message.GetComponent<MessageChoice>().setBackground(_msgLines, msg.text.Length);

                optionMessagesLength += _msgLines * messageLineLength + messageBubbleBuffer + messageGapSpacer / 2;
                //_message.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = msg.text;
                //--_message.GetComponent<MessageChoice>().optionId = i;
                optionButtons.Add(_message);
            }

            
            
        }

    }

    public void OptionChosen(int id)
    {
        CreateNewMessage(currentConversation.messages[currentMessageID].choice.messages[id]);
        currentMessageID = currentConversation.messages[currentMessageID].choice.to[id];
        foreach (GameObject obj in optionButtons)
        {
            Destroy(obj);
        }
    }

    public void EndConversation()
    {
        Debug.LogWarning("End of conversation");
        foreach(GameObject obj in GameObject.FindGameObjectsWithTag("Requirements")){
        obj.GetComponent<RequirementsToEnable>().FinishRequirement("Text");
        }

    }
    public void CreateNewMessage(Message msg)
    {

        int _msgLines =  Mathf.CeilToInt((float)msg.text.Length / numCharPerLine);

        GameObject _message = msg.isPlayer? Instantiate(msgPlayerPrefab, Vector3.zero, Quaternion.identity, scrollParent) : Instantiate(msgOtherPrefab, Vector3.zero, Quaternion.identity, scrollParent);


        _message.GetComponent<RectTransform>().anchoredPosition = new Vector3(0,-currentMessagesLength);
        _message.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, _msgLines * messageLineLength + messageBubbleBuffer);
        if (_msgLines <= 1)
        {
            _message.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, msg.text.Length*messageCharacterWidth + messageBubbleHorizontalBuffer);
        }
        _message.GetComponent<MessageChoice>().setBackground(_msgLines, msg.text.Length);
        currentMessagesLength += _msgLines * messageLineLength + messageBubbleBuffer + messageGapSpacer;
        scrollValue += _msgLines * messageLineLength + messageBubbleBuffer + messageGapSpacer;
        _message.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = msg.text;
        ScrollMessages(0);

        if (currentConversation.messages[currentMessageID].type == messageType.end)
        {
            EndConversation();
        }
        else
        {
            StartCoroutine(NextMessageAfterDelay(msg.delay));
        }
    }
    private void Update()
    {
        if(Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            ScrollMessages(Input.GetAxis("Mouse ScrollWheel"));
        }
    }
    public void ScrollMessages(float val)
    {
        scrollValue += -val* scrollSpeed;

        scrollValue = Mathf.Clamp(scrollValue, scrollParentStartBuffer, currentMessagesLength-scrollMinimumPageLength>scrollParentStartBuffer? currentMessagesLength - scrollMinimumPageLength:scrollParentStartBuffer );

        scrollParent.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, scrollValue);




    }
    IEnumerator NextMessageAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        NextMessage();
    }

    public void ShowPhone(bool to)
    {
        showingPhone = to;
        transform.GetChild(0).gameObject.SetActive(to);
    }


}
