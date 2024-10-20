using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public List<GameObject> objectsToRemove; // List of enemies that need to be removed to win
    public GameObject player;                // Reference to the player object

    // Update is called once per frame
    void Update()
    {
        CheckObjects();  // Check if all objects are removed
        CheckPlayer();   // Check if the player is missing (e.g., has fallen out of the level)
    }

    // Function to check if all objects in the list are removed
    void CheckObjects()
    {
        // Loop through the list and remove any null (destroyed) objects
        objectsToRemove.RemoveAll(obj => obj == null);

        // If the list is empty, the level is "won"
        if (objectsToRemove.Count == 0)
        {
            LevelComplete();
        }
    }

    // Function to check if the player is missing (destroyed or out of scene)
    void CheckPlayer()
    {
        if (player == null)
        {
            RestartLevel();
        }
    }

    // Function to restart the current scene
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Function called when the level is completed (all objects removed)
    void LevelComplete()
    {
        Debug.Log("Level Complete!");
        // You can add logic here to transition to the next level, show a win screen, etc.
        // For now, let's just restart the level
        RestartLevel();
    }
}
