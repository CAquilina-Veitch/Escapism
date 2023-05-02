using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BrokenMessage : MonoBehaviour
{
    public PixelGlitch glitch;
    public MessageChoice broken;
    public MessageChoice unbroken;
    public TextMeshProUGUI text;
    public Color highlightColor;

    public void Work(float optionMessagesLength, int _msgLines,string msg_text,float messageLineLength,float messageBubbleBuffer, float messageCharacterWidth,float messageBubbleHorizontalBuffer,Font font)
    {
        RectTransform tr = GetComponent<RectTransform>();
        tr.anchoredPosition = new Vector3(0, -optionMessagesLength);
        //broken.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, _msgLines * messageLineLength + messageBubbleBuffer);
        //unbroken.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, _msgLines * messageLineLength + messageBubbleBuffer);
        tr.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, _msgLines * messageLineLength + messageBubbleBuffer);
        if (_msgLines <= 1)
        {
            //tr.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, msg_text.Length * messageCharacterWidth + messageBubbleHorizontalBuffer);
            tr.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, msg_text.PhoneTextWidth(7, font) + msg_text.specialMult(5));
        }
        broken.setBackground(_msgLines, msg_text.Length);
        unbroken.setBackground(_msgLines, msg_text.Length);
        text.text = msg_text;


    }

    public void clicked()
    {
        broken.gameObject.SetActive(true);
        glitch.DoBoth(false);

        //GetComponent<Button>().onClick = new Button.ButtonClickedEvent();
        GetComponent<Button>().targetGraphic = broken.GetComponent<Image>();
        ColorBlock cb = ColorBlock.defaultColorBlock;
        cb.disabledColor = highlightColor;
        //cb.highlightedColor = Color.Lerp(highlightColor,cb.highlightedColor,0.5f);
        //cb.fadeDuration = 1; 
        GetComponent<Button>().colors = cb;
    }



}
