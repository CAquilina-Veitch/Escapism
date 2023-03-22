using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum user {mom, friend}
public enum messageType { start, response, choice, link, end}


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
        public int numLines;
        public float delay;
    }
    [Serializable]
    public struct Choice
    {
        public Message[] messages;
        public int[] to;
    }

    [Serializable]
    public struct MessageComplex
    {
        public messageType type;
        public Message message;
        public Choice choice;
    }

    [Serializable]
    public struct MessageConversations
    {
        public int id;
        public user with;
        public MessageComplex[] messageConversations;
    }

    [Header("Dependencies")]

    [SerializeField] TextMeshProUGUI contactName;
    [SerializeField] Image contactIcon;
    [SerializeField] Transform scrollParent;

    [Header("Prefabs")]
    [SerializeField] GameObject msgOtherPrefab;
    [SerializeField] GameObject msgPlayerPrefab;

    [Header("Stats")]
    public float scrollSpeed = 5;
    public float messageLineLength = 10;
    public float messageBubbleBuffer = 10;
    public float messageGapSpacer = 40;

    [Header("Data")]
    public List<Contact> contacts;
    public MessageConversations[] messageConversations;
    public float scrollValue;
    public float currentMessagesLength;
    public int currentMessageID;



    [Header("Runtime")]

    public int currentConversationID;
    public MessageConversations currentConversation;

    private void OnEnable()
    {
        StartConversation(0);
    }

    public void StartConversation(int num)
    {
        currentConversationID = num;

        currentConversation = messageConversations[currentConversationID];


        SetCurrentContact(findContactFromUser(currentConversation.with));

        NextMessage();



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
        if (currentMessageID < currentConversation.messageConversations.Length)
        {
            CreateNewMessage(currentConversation.messageConversations[currentMessageID]);
            currentMessageID++;

        }
        else
        {
            //out of messages
            EndConversation();
        }
    }
    public void EndConversation()
    {
        Debug.LogWarning("End of conversation");
    }
    public void CreateNewMessage(MessageComplex msgC)
    {
        Message msg = msgC.message;

        GameObject _message;

        _message = msg.isPlayer? Instantiate(msgPlayerPrefab, Vector3.zero, Quaternion.identity, scrollParent) : Instantiate(msgOtherPrefab, Vector3.zero, Quaternion.identity, scrollParent);
        
        
        
        _message.GetComponent<RectTransform>().anchoredPosition = new Vector3(0,-currentMessagesLength);
        _message.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, msg.numLines * messageLineLength + messageBubbleBuffer);
        currentMessagesLength += Mathf.Clamp(msgC.message.numLines,1,Mathf.Infinity) * messageLineLength + messageBubbleBuffer + messageGapSpacer;
        _message.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = msg.text;









        StartCoroutine(NextMessageAfterDelay(msg.delay));

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
        scrollValue += val* scrollSpeed;

        scrollValue = Mathf.Clamp(scrollValue, 0, currentMessagesLength);

        scrollParent.position = new Vector3(scrollParent.position.x, currentMessagesLength - scrollValue);




    }
    IEnumerator NextMessageAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        NextMessage();
    }



}
