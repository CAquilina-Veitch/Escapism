using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
public enum Game { real, platformer}
public class Interactable : MonoBehaviour
{
    [SerializeField] GameObject buttonIndicator;
    public UnityEvent interact;
    public Game game;
    PlatformerPlayerController ppc;
    RealPlayerController rpc;
    public bool hideIndicator;
    public bool alwaysActive;

    public void IndicatorVisiblity(bool to)
    {
        hideIndicator = !to;
        buttonIndicator.SetActive(to);
        buttonIndicator.SetActive(alwaysActive ? true : buttonIndicator.activeSelf);
    }
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
                buttonIndicator.SetActive(alwaysActive ? true : buttonIndicator.activeSelf);
                rpc.CheckInteractable();
            }
            else
            {
                ppc.ChangeInteraction(this, false);
                buttonIndicator.SetActive(false);
                buttonIndicator.SetActive(alwaysActive ? true : buttonIndicator.activeSelf);
                ppc.CheckInteractable();
            }
                
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            
            if (game == Game.real)
            {
                buttonIndicator.SetActive(rpc.currentInteractable == this&&!hideIndicator);
                buttonIndicator.SetActive(alwaysActive ? true : buttonIndicator.activeSelf);
            }
            else
            {
                buttonIndicator.SetActive(ppc.currentInteractable == this && !hideIndicator);
                buttonIndicator.SetActive(alwaysActive ? true : buttonIndicator.activeSelf);
            }
                
        }
    }
    private void OnEnable()
    {
        buttonIndicator.SetActive(alwaysActive ? true : buttonIndicator.activeSelf);
    }
    private void OnDisable()
    {
        if (game == Game.real)
        {
            if (rpc != null)
            {
                if (rpc.currentInteractable == this)
                {
                
                    rpc.ChangeInteraction(this, false);
                    rpc.CheckInteractable();
                }
                
            }
        }
        else
        {
            if (ppc != null)
            {
                if (ppc.currentInteractable == this)
                {
                
                    ppc.ChangeInteraction(this, false);
                    ppc.CheckInteractable();
                }
            }
            
        }
    }
}
