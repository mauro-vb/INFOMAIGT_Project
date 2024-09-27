using UnityEngine;

public class SizeManager : MonoBehaviour
{
    public ResourceComponent resourceComponent; // Reference to the ResourceComponent
    public Transform blobTransform;
    [SerializeField]
    private float smoothTime = 0.5f; // Smoothing time for scale changes

    private Vector3 currentVelocity = Vector3.zero; // Used for smooth damping

    void Start()
    {
      blobTransform = gameObject.transform; // Assign this blob's transform to SizeManager
      blobTransform.localScale = GetTargetSize();
    }

    private Vector3 GetTargetSize()
    {
      float targetSize = (float)resourceComponent.CurrentResource / 25;
      return new Vector3(targetSize, targetSize, targetSize);
    }

    void Update()
    {
        // Lerp between minSize and maxSize based on sizeRatio
        Vector3 targetSize = GetTargetSize();

        // Smoothly interpolate the size over time
        blobTransform.localScale = Vector3.SmoothDamp(blobTransform.localScale, targetSize, ref currentVelocity, smoothTime);
    }
}
