using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(GameManager))]
[RequireComponent(typeof(UIManager))]

public class PauseController : MonoBehaviour
{

    private UIManager _uIManager;

    private void Awake()
    {
        _uIManager = GetComponent<UIManager>();
    }
    public void ToMenu()
    {
        
    }


    public void Unpause()
    {
        _uIManager.TogglePause(false);
    }
}
