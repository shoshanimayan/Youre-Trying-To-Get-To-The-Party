using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
[RequireComponent(typeof(GameManager))]

public class UIManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup _textCanvas;
    [SerializeField] private CanvasGroup _gameCanvas;
    [SerializeField] private CanvasGroup _pauseCanvas;

    [SerializeField] private TextMeshProUGUI _hint;
    [SerializeField] private TextMeshProUGUI _text;


  
    private State _prevState = State.Paused;

    private void Awake()
    {
        TogglePause(false);
    }

    public void TogglePause(bool paused)
    {
        if (paused)
        {
            _pauseCanvas.alpha = 1;
            _pauseCanvas.interactable = true;
            _prevState = GameManager.GetState();
            GameManager.Pause();
        }
        else
        {
            _pauseCanvas.alpha = 0;
            _pauseCanvas.interactable = false;
            GameManager.ForceSetState(_prevState);


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
                if (canvas.alpha != 0) { canvas.alpha = tempAlpha; }
                yield return null;
            }
        }
        else
        {
            
                for (float i = 0; i <= 1; i += Time.deltaTime / 2)
                {
                    tempAlpha = i;
                if (canvas.alpha != 1) { canvas.alpha = tempAlpha; }
                    yield return null;
                }
            
        }

        if (callbackText != null)
        {
            Debug.Log(callbackText);
            SetText(callbackText);
            yield return null;

        }
    }

    private IEnumerator RevealText(TextMeshProUGUI text)
    {
        GameManager.setTouch(false);
        var TotalVisibleCharacters =text.text.Length;
        int counter = 0;
        while (counter<TotalVisibleCharacters+1)
        {
            int visibleCount = counter % (TotalVisibleCharacters + 1);

            text.maxVisibleCharacters = visibleCount;

            //if (visibleCount >= TotalVisibleCharacters) { yield return new WaitForSeconds(1); }
            counter++;
            yield return new WaitForSeconds(.05f);
        }
        GameManager.setTouch(true);
        yield return null;
    }

    private void SetText(string text)
    {
        _text.text = text;
        if (text.Length > 0)
        {
            Debug.Log(text);
            Debug.Log(_text.maxVisibleCharacters);

            StartCoroutine(RevealText(_text));
        }
    }

    public void SetTextMode(bool active, string text=null)
    {
        if (active)
        {
            GameManager.ToText();
            StartCoroutine(FadeCanvas(_gameCanvas, true));
            StartCoroutine(FadeCanvas(_textCanvas, false,text));

        }
        else
        {
            StartCoroutine(FadeCanvas(_gameCanvas, false));
            StartCoroutine(FadeCanvas(_textCanvas, true,""));
            GameManager.ToMap();
        }
    
    }

    public void HideHint()
    {
        StartCoroutine(FadeOutText(_hint));

    }

    public void Initialize(string StartText)
    {
        _gameCanvas.alpha = 0;
        _textCanvas.alpha = 1;
        SetText(StartText);
    }

    






}
