using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class Interactable2D : InteractableEnvironment2D
{
    [SerializeField] private AssetReference _addressableTextAsset = null;
     private Image _outline;
    private Coroutine _fadeCor = null;
    private bool _stopped;
    private bool _clickedOn;
    public override void Initialize()
    {
        base.Initialize();
        _interactable = true;
        _outline = transform.GetChild(0).GetComponent<Image>();
        var tempColor = _outline.color;
        tempColor.a = 0;
        _outline.color = tempColor;

    }

    private IEnumerator FadeOutline(bool fadein)
    {
        if (_detected)
        {
            _stopped = false;
            var tempColor = _outline.color;
            if (fadein)
            {
                for (float i = tempColor.a; i <= 1; i += Time.deltaTime )
                {
                    tempColor.a = i;
                    _outline.color = tempColor;
                    yield return null;
                    if (_stopped) { break; }
                }

            }
            else
            {
                for (float i = tempColor.a; i > 0; i -= Time.deltaTime )
                {
                    tempColor.a = i;
                    _outline.color = tempColor;
                    yield return null;
                    if (_stopped) { break; }

                }

            }
        }
        _fadeCor = null;

        yield return null;
    }

    void OnMouseOver()
    {
        _stopped = true;

        if (_fadeCor != null) { StopCoroutine(_fadeCor); }
        StartCoroutine(FadeOutline(true));
    }

    void OnMouseExit()
    {
        _stopped = true;

        if (_fadeCor != null) { StopCoroutine(_fadeCor); }

        var tempColor = _outline.color;
        tempColor.a = 0;
        _outline.color = tempColor;
    }

    public override void OnClicked()
    {
        if (_addressableTextAsset!=null && !_clickedOn)
        {
            _clickedOn = true;
            GameManager.ReadTextAsset(_addressableTextAsset);
        }
    }
}
