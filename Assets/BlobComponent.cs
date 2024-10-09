using UnityEngine;

public class BlobComponent : MonoBehaviour
{
    public ResourceComponent resourceComponent;  // Reference to the ResourceComponent that controls the size
    private BlobManager blobManager;
    private SizeManager sizeManager;

    void Start()
    {
        // Find and register the BlobManager
        blobManager = FindObjectOfType<BlobManager>();
        if (blobManager != null)
        {
            blobManager.RegisterBlob(this);
        }

        // Attach or find the SizeManager on this blob and initialize its reference
        sizeManager = GetComponent<SizeManager>();
        if (sizeManager != null)
        {
            sizeManager.resourceComponent = resourceComponent;
            sizeManager.blobTransform = transform; // Assign this blob's transform to SizeManager
        }
    }

    void OnDestroy()
    {
        if (blobManager != null)
        {
            blobManager.UnregisterBlob(this);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        BlobComponent otherBlob = other.GetComponent<BlobComponent>();
        if (otherBlob != null && otherBlob != this)
        {
            if (otherBlob.resourceComponent.CurrentResource <= this.resourceComponent.CurrentResource)
            {
               AbsorbBlob(otherBlob);
            }
        }
        else {
            Debug.Log("colliding not other blob");
            ResourceComponent otherResourceComponent = other.GetComponent<ResourceComponent>();
            if (otherResourceComponent != null) {
                Debug.Log("colliding with resource component");
                // Store the current resources for clarity
                int currentResource = resourceComponent.CurrentResource;
                int otherCurrentResource = otherResourceComponent.CurrentResource;

                // Update the resources, damaging both blobs
                resourceComponent.Damage(otherCurrentResource * 2);
                otherResourceComponent.Damage(currentResource * 2);
            }
        }
    }

    public void AbsorbBlob(BlobComponent otherBlob)
    {
        // Combine the resources of both blobs
        resourceComponent.Heal(otherBlob.resourceComponent.CurrentResource);

        // Optionally, destroy the other blob after merging resources
        Destroy(otherBlob.gameObject);
    }
}
