using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public List<GameObject> objectsToRemove; // List of enemies that need to be removed to win
    public GameObject player;                // Reference to the player object

    public bool loadQuestionnaireAfterScene = true;

    private GameObject winScreen; // Reference to the win screen panel
    private GameObject loseScreen; // Reference to the lose screen panel

    private bool gameEnded = false;

    private void Start()
    {
        gameEnded = false;
        GameObject canvas = GameObject.Find("Canvas");
        if (canvas != null)
        {
            winScreen = canvas.transform.Find("WinScreen").gameObject;
            winScreen.SetActive(false); // Ensure the win screen is hidden at the start
            loseScreen = canvas.transform.Find("LoseScreen").gameObject;
            loseScreen.SetActive(false); // Ensure the lose screen is hidden at the start
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameEnded)
        {
            CheckObjects();  // Check if all objects are removed
            CheckPlayer();   // Check if the player is missing (e.g., has fallen out of the level)
        }
    }

    // Function to check if all objects in the list are removed
    void CheckObjects()
    {
        // Loop through the list and remove any null (destroyed) objects
        objectsToRemove.RemoveAll(obj => obj == null);

        // If the list is empty, the level is "won"
        if (objectsToRemove.Count == 0)
        {
            ShowEnd(true);
        }
    }

    // Function to check if the player is missing (destroyed or out of scene)
    void CheckPlayer()
    {
        if (player == null)
        {
            ShowEnd(false);
        }
    }

    // Function to restart the current scene
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ShowEnd(bool won)
    {
        gameEnded = true;
        if (won)
        {
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            int nextSceneIndex = currentSceneIndex + 1;
        
            if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
            {
                if (loadQuestionnaireAfterScene)
                {
                    SceneManager.LoadScene("Scenes/Questionnaire", LoadSceneMode.Single);
                }
                else
                {
                    // Load the next scene
                    SceneManager.LoadScene(nextSceneIndex);
                }
            }
            else
            {
                // Optionally, if you're at the last scene, you can loop back to the first
                winScreen.SetActive(true); // Show win screen
            }
        }
        else
        {
            loseScreen.SetActive(true); // Show lose screen
        }
    }

    // Function called when the level is completed (all objects removed)
    void LevelComplete()
    {
        ShowEnd(true);
    }
}
