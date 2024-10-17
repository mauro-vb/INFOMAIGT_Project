using System;
using UnityEngine;

public class SizeController : MonoBehaviour
{
    public ResourceController resource; /* To be added from the editor */


    public void Start()
    {
    }

    public void Update()
    {
        /* Set scale based on current resource */
        float ratio = 0.15f + (float)resource.currentResource / resource.maxResource;
        transform.localScale = new Vector3(ratio, ratio, ratio);
    }
}
