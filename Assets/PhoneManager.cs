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
        public sprite photo;
    }


    [Serializable]
    public struct Message
    {
        public bool isPlayer;
        public string text;
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



    [Header("Data")]

    public List<Contact> contacts;
    public MessageConversations[] messageConversations;



    [Header("Runtime")]

    public int currentConversationID;
    public MessageConversations currentConversation;



    public void StartConversation(int num)
    {
        currentConversationID = num;

        currentConversation = messageConversations[currentConversationID];


        findContactFromUser(currentConversation.with);





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





}
