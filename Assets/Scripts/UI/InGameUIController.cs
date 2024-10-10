using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameUIController : MonoBehaviour
{
    private Scene scene;

    private void Start()
    {
        scene = SceneManager.GetActiveScene();
    }

    private void Update()
    {
        /* Get the */
        var weapons = scene.GetRootGameObjects();
    }
}