using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialStageTransition : MonoBehaviour
{
    public TutorialManager managerRef;
    public GameObject wallAppear;

    public GameObject nextTransition;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == Layers.PLAYER)
        {
            managerRef.EndCurrentStage();
            if (wallAppear)
            {
                wallAppear.gameObject.SetActive(true);
            }

            if (nextTransition)
            {
                nextTransition.gameObject.SetActive(true);
                Destroy(gameObject);
            }
            else
            {
                QDataManager.Instance.UpdateScenes("Level1", "Level2");
                /* Load first Level */
                SceneManager.LoadScene("Level1");
            }
        }
    }
}