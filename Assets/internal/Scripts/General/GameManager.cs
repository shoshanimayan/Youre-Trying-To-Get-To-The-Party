using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public enum State {Paused, Map, Text,Menu,Loading}
[RequireComponent(typeof(UIManager))]
[RequireComponent(typeof(AudioManager))]
public class GameManager : MonoBehaviour
{

    [SerializeField] private AssetReference _addressableTextAsset = null;


    private static State _state;
    private static bool _canTouch;

    private static UIManager _uiManager;
    private static TextReader _textReader;

    public static SceneController _sceneController { get; private set; }
    
    private void Awake()
    {
        _textReader = new TextReader();
        _uiManager = GetComponent<UIManager>();
        _state = State.Text;
        var  root= (GameObject.Find("Root"));
        if (root && root.GetComponent<SceneController>())
        {
            _sceneController = root.GetComponent<SceneController>();
        }


    }

    // Start is called before the first frame update
    void Start()
    {
        ReadTextAsset(_addressableTextAsset);

    }

    public static void ToMenu()
    {
        if (_sceneController)
        {
            _sceneController.LoadMenu();
        }
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
    public static void SetTextMode(string text)
    {
        _uiManager.SetTextMode(true, text);

    }

    public static void ReadTextAsset(AssetReference addressableTextAsset)
    {
        _textReader.ReadAddressableTextAsset(addressableTextAsset);
    }



}
