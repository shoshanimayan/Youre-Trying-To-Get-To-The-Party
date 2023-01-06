using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableEnvironment3D :BaseInteractable
{
    [SerializeField] private MeshRenderer _render;

    private IEnumerator FadeIn()
    {

        var tempColor = _render.material.color;
        _render.enabled = true;

        _render.material.DisableKeyword("_EMISSION");

        for (float i = 0; i <= 1; i += Time.deltaTime/2)
        {
            tempColor.a = i;
            _render.material.color = tempColor;
            yield return null;
        }
        _render.material.EnableKeyword("_EMISSION");

    }

    public override void Initialize()
    {
        _render = GetComponent<MeshRenderer>();
        var tempColor = _render.material.color;
        tempColor.a = 0;
        _render.material.color = tempColor;
        _render.enabled = false;
    }

    public override void OnDetected()
    {
        base.OnDetected();
        StartCoroutine(FadeIn());

    }

    public override bool Detected
    {
        get => base._detected;
        set
        {

            if (value == base._detected) return;
            base._detected = value;
            if (value)
            {
                Debug.Log(1);

                OnDetected();
            }
        }
    }
}
