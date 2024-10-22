using System;
using UnityEngine;

public class TutorialStageTransition : MonoBehaviour
{
    public TutorialManager managerRef;
    private BoxCollider2D bc;

    private void Start()
    {
        bc = GetComponent<BoxCollider2D>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == Layers.PLAYER)
        {
            managerRef.EndCurrentStage();
            Destroy(bc);
        }
    }
}