using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MenuController : MonoBehaviour
{
	private SceneController _sceneController;
	private AudioSource _as;
	private void Awake()
	{

		var root = (GameObject.Find("Root"));
		if (root && root.GetComponent<SceneController>())
		{
			_sceneController = root.GetComponent<SceneController>();
		}
		_as = GetComponent<AudioSource>();
	}
	public void QuitApplication()
	{
		_as.Play();

#if UNITY_EDITOR
		EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
	}

	public void StartGame()
	{
		_as.Play();

		if (_sceneController)
		{
			_sceneController.LoadMap();
		}
	}
}
