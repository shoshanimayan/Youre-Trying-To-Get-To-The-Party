using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State {Paused, Map, Text,Menu,Loading}
[RequireComponent(typeof(UIManager))]
[RequireComponent(typeof(AudioManager))]
public class GameManager : MonoBehaviour
{
    private static State _state;
    private static bool _canTouch;

    private static UIManager _uiManager;
    private void Awake()
    {
        _uiManager = GetComponent<UIManager>();

        _state = State.Text;

        _uiManager.SetTextMode(true, "test Text\n(Click To Continue)");


    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    

    public static void Pause()
    {
        _state = State.Paused;
    }

    public static void ToMap()
    {
        _state = State.Map;
    }

    public static void ToText()
    {
        _state = State.Text;
    }

    public static void ForceSetState(State state)
    {
        _state = state;
    }

    public static State GetState()
    {
        return _state;
    }

    public static bool CanTouch()
    {
        return _canTouch;
    }

    public static void setTouch(bool CanTouch)
    {
        _canTouch = CanTouch;
    }
    //read asset and send string to ui manager
    public static void SetTextMode()
    { 
    
    }

    

}
