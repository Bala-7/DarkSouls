using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InfoMessage : MonoBehaviour
{
    private TMP_Text _messageText;
    private Transform _background;
    private Transform _continue;

    private List<string> _messageList;

    private bool _isShowing = false;
    public bool IsShowing() { return _isShowing; }

    private void Awake()
    {
        _background = transform.Find("Background");
        _messageText = transform.Find("MessageText").GetComponent<TMP_Text>();
        _continue = transform.Find("Continue");


        _messageList = new List<string>();

        SetShowingMessage(false);
        _messageList.Clear();
    }

    public void AddMessageToList(string message)
    {
        _messageList.Add(message);
    }

    public void SetNewMessage(string newMessage)
    {
        _messageText.text = newMessage;
    }

    public void SetShowingMessage(bool enabled)
    {
        string text = "";
        foreach (string message in _messageList)
        {
            text += message + "\n";
        }
        _messageText.text = text;
        _background.gameObject.SetActive(enabled);
        _messageText.gameObject.SetActive(enabled);
        _continue.gameObject.SetActive(enabled);
        _isShowing = enabled;
    }

    public void ClearMessageList()
    { 
        _messageList.Clear();
    }
}
