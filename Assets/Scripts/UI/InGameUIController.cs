using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameUIController : MonoBehaviour
{
    public Camera camera;
    
    private Scene scene;

    private void Start()
    {
        scene = SceneManager.GetActiveScene();
    }

    private void Update()
    {
        /* Get the */
        var gameobjects = scene.GetRootGameObjects();
        for (int i = 0; i < gameobjects.Length; i++)
        {
            if (gameobjects[i].layer == Layers.PROJECTILES)
            {
                
            }
        }
    }
}