using UnityEngine;
using System.Collections.Generic;

public class BlobManager : MonoBehaviour
{
    public List<BlobComponent> allBlobs = new List<BlobComponent>();
    public BlobComponent currentControlledBlob;

    void Start()
    {
        FindLargestBlob(); // Initially find the largest blob
    }

    void Update()
    {
        FindLargestBlob(); // Update each frame to find the largest blob
    }

    public void RegisterBlob(BlobComponent blob)
    {
        allBlobs.Add(blob);
        FindLargestBlob();
    }

    public void UnregisterBlob(BlobComponent blob)
    {
        allBlobs.Remove(blob);
        FindLargestBlob();
    }

    private void FindLargestBlob()
    {
        if (allBlobs.Count == 0) return;

        BlobComponent largestBlob = currentControlledBlob == null ? allBlobs[0] : currentControlledBlob;
        foreach (BlobComponent blob in allBlobs)
        {
            if (blob.resourceComponent.CurrentResource > largestBlob.resourceComponent.CurrentResource)
            {
                largestBlob = blob;
            }
        }

        // If the largest blob has changed, switch control
        if (currentControlledBlob != largestBlob)
        {
            BlobComponent oldBlob = currentControlledBlob;
            currentControlledBlob = largestBlob;
            ChangeControls(oldBlob, largestBlob);  // Switch control
        }
    }


    private void ChangeControls(BlobComponent currentBlob, BlobComponent newBlob)
    {
        // Remove controls from the current controlled blob if it exists
        if (currentBlob != null)
        {
            Destroy(currentBlob.gameObject.GetComponent<PlayerController>());
            Destroy(currentBlob.gameObject.GetComponent<ShootingWeapon>());
            //currentBlob
        }

        // Add controls to the new blob
        if (newBlob != null)
        {
            newBlob.gameObject.AddComponent<PlayerController>();
            newBlob.gameObject.AddComponent<ShootingWeapon>();
        }
    }

}
