using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonMessage : MonoBehaviour
{
    private Image _buttonIcon;
    private TMP_Text _messageText;
    private Transform _background;

    public List<Sprite> buttonIcons;

    public enum CONTROLLER_BUTTONS { BUTTON_A = 0, BUTTON_X, BUTTON_B, BUTTON_Y}

    private void Awake()
    {
        _background = transform.Find("Background");
        _buttonIcon = transform.Find("Icon").GetComponent<Image>();
        _messageText = transform.Find("MessageText").GetComponent<TMP_Text>();

        SetShowingMessage(false);

    }

    public void SetNewMessage(CONTROLLER_BUTTONS button, string newMessage)
    {
        _buttonIcon.sprite = buttonIcons[(int)button];
        _messageText.text = newMessage;
    }

    public void SetShowingMessage(bool enabled)
    {
        _background.gameObject.SetActive(enabled);
        _buttonIcon.gameObject.SetActive(enabled);
        _messageText.gameObject.SetActive(enabled);
    }
}
