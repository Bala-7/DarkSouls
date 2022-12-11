using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BigEventUI : MonoBehaviour
{
    private Image _background;
    private TMP_Text _victoryText;
    private TMP_Text _deathText;

    private TMP_Text _displayedText;
    private float _tweenerSpeed = 0.05f;

    private void Awake()
    {
        _background = transform.Find("Background").GetComponent<Image>();
        _victoryText = transform.Find("VictoryText").GetComponent<TMP_Text>();
        _deathText = transform.Find("YouDiedText").GetComponent<TMP_Text>();

        _background.gameObject.SetActive(false);
        _victoryText.gameObject.SetActive(false);
        _deathText.gameObject.SetActive(false);
    }

    

    public void DisplayVictoryMessage()
    {
        _displayedText = _victoryText;

        StartCoroutine("DisplayMessageCoroutine");
    }

    public void DisplayDeathMessage()
    {
        _displayedText = _deathText;

        StartCoroutine("DisplayMessageCoroutine");
    }

    private IEnumerator DisplayMessageCoroutine()
    {
        yield return new WaitForSeconds(2.0f);
        _displayedText.transform.localScale = 0.7f * Vector3.one;
        _background.gameObject.SetActive(true);
        _displayedText.gameObject.SetActive(true);
        float scale = _displayedText.transform.localScale.x;
        while (scale <= 1)
        {
            scale += _tweenerSpeed * Time.deltaTime;
            _displayedText.transform.localScale = scale * Vector3.one;
            yield return null;
        }

        yield return null;
    }

    public void Restart()
    {
        _background.gameObject.SetActive(false);
        _victoryText.gameObject.SetActive(false);
        _deathText.gameObject.SetActive(false);
    }
}
