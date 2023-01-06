using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UIManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup _textCanvas;
    [SerializeField] private CanvasGroup _gameCanvas;

    [SerializeField] private TextMeshProUGUI _hint;
    [SerializeField] private TextMeshProUGUI _text;


    private bool _tutorialComplete=true;
    private bool _canBeTouched;

    private void Update()
    {
        if (Input.GetButtonDown("Fire1") )
        {
            if (!_tutorialComplete)
            {
                _tutorialComplete = true;
                StartCoroutine(FadeOutText(_hint));
            }
            if (_canBeTouched)
            {
                _canBeTouched = false;
            }
        }
    }

    private IEnumerator FadeOutText(TextMeshProUGUI text)
    {
        var tempColor = text.color;

        for (float i = 0; i <= 1; i += Time.deltaTime / 2)
        {
            tempColor.a = 1-i;
            text.color = tempColor;
            yield return null;
        }

    }

    private IEnumerator FadeCanvas(CanvasGroup canvas, bool fadeOut,string callbackText=null )
    {
        var tempAlpha = canvas.alpha;
        if (fadeOut)
        {
            for (float i = 0; i <= 1; i += Time.deltaTime / 2)
            {
                tempAlpha = 1 - i;
                canvas.alpha = tempAlpha;
                yield return null;
            }
        }
        else
        {
            for (float i = 0; i <= 1; i += Time.deltaTime / 2)
            {
                tempAlpha = i;
                canvas.alpha = tempAlpha;
                yield return null;
            }
        }

        if (callbackText != null)
        {
            SetText(callbackText);
            yield return null;

        }
    }

    private IEnumerator RevealText(TextMeshProUGUI text)
    {
        _canBeTouched = false;
        var TotalVisibleCharacters=text.textInfo.characterCount;
        int counter = 0;

        while (counter<TotalVisibleCharacters+1)
        {
            int visibleCount = counter % (TotalVisibleCharacters + 1);
            text.maxVisibleCharacters = visibleCount;
            if (visibleCount >= TotalVisibleCharacters) { yield return new WaitForSeconds(1); }
            counter++;
            yield return new WaitForSeconds(.05f);
        }
        _canBeTouched = true;
    }

    private void SetText(string text)
    {
        _text.text = text;
        if (text.Length > 0)
        {
            StartCoroutine(RevealText(_text));
        }
    }

    public void SetTextMode(bool active, string text=null)
    {
        if (active)
        {
            StartCoroutine(FadeCanvas(_gameCanvas, true));
            StartCoroutine(FadeCanvas(_textCanvas, false,text));

        }
        else
        {
            StartCoroutine(FadeCanvas(_gameCanvas, false));
            StartCoroutine(FadeCanvas(_textCanvas, true,""));
        }
    
    }

    public void EnableTutorial()
    {
        _tutorialComplete = false;
    }






}
