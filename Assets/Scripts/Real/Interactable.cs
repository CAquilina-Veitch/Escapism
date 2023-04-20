using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
public enum Game { real, platformer}
public class Interactable : MonoBehaviour
{
    [SerializeField] GameObject buttonIndicator;
    bool currentlyInRange;
    public UnityEvent interact;
    public Game game;
    PlatformerPlayerController ppc;
    RealPlayerController rpc;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if(game == Game.real)
            {
                rpc = other.GetComponent<RealPlayerController>();
                rpc.ChangeInteraction(this, true);
            }
            else
            {
                ppc = other.GetComponent<PlatformerPlayerController>();
                ppc.ChangeInteraction(this, true);
            }
            
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (game == Game.real)
            {
                rpc.ChangeInteraction(this, false);
                buttonIndicator.SetActive(false);
            }
            else
            {
                ppc.ChangeInteraction(this, false);
                buttonIndicator.SetActive(false);
            }
                
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (game == Game.real)
            {
                buttonIndicator.SetActive(rpc.currentInteract == interact);
            }
            else
            {
                buttonIndicator.SetActive(ppc.currentInteract == interact);
            }
                
        }
    }
}
