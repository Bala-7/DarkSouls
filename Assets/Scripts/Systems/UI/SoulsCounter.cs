using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SoulsCounter : MonoBehaviour
{
    TMP_Text _text;


    private void Awake()
    {
        _text = transform.Find("Text").GetComponent<TMP_Text>();
    }

    private void Start()
    {
        UpdateSouls();
    }

    public void UpdateSouls()
    {
        try { _text.text = ThirdPersonControllerMovement.s.Performance.Souls.ToString(); } catch (Exception e) { Debug.LogError("CHECK THIS!"); }
    }
}
