using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable2D : InteractableEnvironment2D
{
    //text addressable
    private bool _highlighted;

    public override void Initialize()
    {
        base.Initialize();
        _interactable = true;
    }

    void OnMouseOver()
    {
        if (!_highlighted)
        {
            Debug.Log("Mouse is over GameObject.");
            _highlighted = true;
        }
    }

    void OnMouseExit()
    {
        Debug.Log("Mouse is no longer on GameObject.");
        if (_highlighted)
        {
            _highlighted = false;
        }
    }

    public override void OnClicked()
    {
        //communicate reader transition
    }
}
