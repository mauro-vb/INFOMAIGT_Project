using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UICommands : MonoBehaviour
{
    public static event Action OnRestartLevel;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void RestartLevel()
    {
        OnRestartLevel?.Invoke();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void EndGame()
    {
        #if UNITY_EDITOR
            // If we're running in the Unity Editor, stop playing
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            // If we're running in a build, quit the application
            Application.Quit();
        #endif
    }
}
