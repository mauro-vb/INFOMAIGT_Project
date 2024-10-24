using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class SizeController : MonoBehaviour
{
    public ResourceController resource; /* To be added from the editor */
    public float lightMult = 1.0f;
    public float baseScale =  1.0f;

    private Light2D lightComponent;
    private float lightVal;
    private float radiusVal;


    public void Start()
    {
        lightComponent = GetComponentInChildren<Light2D>();
        if (lightComponent != null)
        {
            lightVal = lightComponent.intensity;
            radiusVal = lightComponent.pointLightOuterRadius;
        }
    }

    public void Update()
    {
        /* Set scale based on current resource */
        float ratio = 0.15f + 0.85f * (float)resource.currentResource / resource.maxResource;
        if (lightComponent != null)
        {
            lightComponent.intensity = lightVal - (lightVal * lightMult * (1 - ratio));
            lightComponent.pointLightOuterRadius = radiusVal - (radiusVal * lightMult * (-0.5f + 0.5f * (1 - ratio)));
        }
        transform.localScale = new Vector3(baseScale * ratio, baseScale * ratio, baseScale * ratio);
    }
}
