using System;
using UnityEngine;

public class TutorialSizeController : MonoBehaviour
{
    private ResourceController resource; /* To be added from the editor */

    private void Start()
    {
        resource = GetComponent<ResourceController>();
    }

    public void Update()
    {
        /* Set scale based on current resource */
        float ratio = 0.15f + 0.85f * (float)resource.currentResource / resource.maxResource;
        transform.localScale = new Vector3(ratio, ratio, ratio);
    }
}