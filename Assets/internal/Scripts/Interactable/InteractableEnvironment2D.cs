using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class InteractableEnvironment2D : BaseInteractable
{
    [SerializeField] private Image _image;

    private IEnumerator FadeIn()
    {
        var tempColor = _image.color;

        for (float i = 0; i <= 1; i += Time.deltaTime/2)
        {
            tempColor.a = i;
            _image.color = tempColor;
            yield return null;
        }
    }

    public override void Initialize()
    {
        _image = GetComponent<Image>();
        var tempColor = _image.color;
        tempColor.a = 0;
        _image.color = tempColor;
    }

    public override void OnDetected()
    {
        base.OnDetected();
        StartCoroutine(FadeIn());

    }

    public override bool Detected {
        get => base._detected;
        set {

            if (value == base._detected) return;
            base._detected = value;
            if (value)
            {

                OnDetected();
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, transform.localScale/6);
    }
}
