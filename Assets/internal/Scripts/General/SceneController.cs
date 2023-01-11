using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
public  class SceneController : MonoBehaviour
{
    [SerializeField] AssetReference _sceneMenu;
    [SerializeField] AssetReference _sceneMap;

    

    private AsyncOperationHandle<SceneInstance> _handle;
    private bool _unloaded=true;

    // Start is called before the first frame update
    private void Awake()
    {
        Application.targetFrameRate = 30;

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        LoadMenu();

    }

    private void Load(AssetReference scene)
    {

        Debug.Log("loading level");
        if (!_unloaded)
        {
            _unloaded = true;
            UnloadScene();
        }
        Addressables.LoadSceneAsync(scene, UnityEngine.SceneManagement.LoadSceneMode.Additive).Completed += SceneLoadCompleted;
    }

    private void SceneLoadCompleted(AsyncOperationHandle<SceneInstance> obj)
    {
        if (obj.Status == AsyncOperationStatus.Succeeded)
        {
            _handle = obj;
            _unloaded = false;
            
        }
    }

    private void UnloadScene()
    {
        Debug.Log("unloading level");

        Addressables.UnloadSceneAsync(_handle, true).Completed += op =>
        {
            if (op.Status == AsyncOperationStatus.Succeeded)
                Debug.Log("Successfully unloaded scene.");
            else
            {
                Debug.Log(op.Status.ToString());
            }
        };
    }


    public void LoadMap()
    {
        Load(_sceneMap);
    }

    public void LoadMenu()
    {
        Load(_sceneMenu);
    }

 
   

    

}
