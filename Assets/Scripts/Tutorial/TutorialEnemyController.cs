using System;
using UnityEngine;

public class TutorialEnemyController : MonoBehaviour
{
    
    public TutorialResourceController resource; /* To be set in the editor */

    private void Start()
    {
        resource = GetComponent<TutorialResourceController>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == Layers.PROJECTILES)
        {
            var projectileResource = other.gameObject.GetComponent<TutorialResourceController>();
            resource.currentResource -= projectileResource.currentResource;
        }
    }
}
