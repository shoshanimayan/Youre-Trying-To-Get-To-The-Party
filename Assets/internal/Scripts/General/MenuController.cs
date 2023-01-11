using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MenuController : MonoBehaviour
{
	public void QuitApplication()
	{
#if UNITY_EDITOR
		EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
	}

	public void StartGame()
	{ 
	
	}
}
