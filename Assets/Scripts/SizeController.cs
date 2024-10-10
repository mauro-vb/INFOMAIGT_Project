using System;
using UnityEngine;

public class SizeController : MonoBehaviour
{
    public ResourceController resource; /* To be added from the editor */

    private CircleCollider2D objectCollider;
    private float objectColliderInitialRadius = -1.0f;
    
    public void Start()
    {
        objectCollider = GetComponent<CircleCollider2D>();
        if (objectCollider)
        {
            objectColliderInitialRadius = objectCollider.radius;
        }
    }

    public void Update()
    {
        /* Set scale based on current resource */
        float ratio = (float)resource.currentResource / resource.maxResource;
        transform.localScale = new Vector3(ratio, ratio, ratio);

        if (objectCollider)
        {
            objectCollider.radius = objectColliderInitialRadius * ratio;
        }
    }
}
