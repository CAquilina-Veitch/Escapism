using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    [SerializeField] GameObject buttonIndicator;
    bool currentlyInRange;
    public UnityEvent interact;
    RealPlayerController player;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            player = other.GetComponent<RealPlayerController>();
            player.ChangeInteraction(this, true);
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            player.ChangeInteraction(this, false);
            buttonIndicator.SetActive(false);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            buttonIndicator.SetActive(player.currentInteract == interact);
        }
    }
}
