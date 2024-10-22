using System;
using System.Collections;
using UnityEngine;

public class TutorialStageTransition : MonoBehaviour
{
    public TutorialManager managerRef;
    public GameObject wallAppear;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == Layers.PLAYER)
        {
            managerRef.EndCurrentStage();
            wallAppear.gameObject.SetActive(true);
            Destroy(gameObject);
        }
    }
}