using System;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject ObjectToFollow; // The object the camera follows
    // public Vector2 minPosition;  // Minimum limit for camera (x, y)
    // public Vector2 maxPosition;  // Maximum limit for camera (x, y)

    public Transform topBound;
    public Transform rightBound;
    public Transform bottomBound;
    public Transform leftBound;

    public bool boundCamera;

    private float halfHeight;
    private float halfWidth;

    private void Start()
    {
        var cam = Camera.main;
        if (cam)
        {
            halfHeight = cam.orthographicSize;
            halfWidth = halfHeight * cam.aspect;
        }
    }
    
    void LateUpdate()
    {
        if (ObjectToFollow != null)
        {
            // Get the desired position based on the object to follow
            float targetX = ObjectToFollow.transform.position.x;
            float targetY = ObjectToFollow.transform.position.y;

            // Clamp the camera's X and Y positions to stay within the defined limits
            // float clampedX = Mathf.Clamp(targetX, minPosition.x, maxPosition.x);
            // float clampedY = Mathf.Clamp(targetY, minPosition.y, maxPosition.y);

            if (boundCamera)
            {
                // Clamp the camera to some defined transforms in the scene (usually some walls)
                float clampedX = Mathf.Clamp(targetX, leftBound.position.x + halfWidth, rightBound.position.x - halfWidth);
                float clampedY = Mathf.Clamp(targetY, bottomBound.position.y + halfHeight, topBound.position.y - halfHeight);

                // Apply the clamped position to the camera
                transform.position = new Vector3(
                    clampedX, // Limit the X position
                    clampedY, // Limit the Y position
                    transform.position.z // Keep the current Z position (assuming this is a 2D camera)
                );
            }
            else
            {
                transform.position = new Vector3(
                    targetX, // Limit the X position
                    targetY, // Limit the Y position
                    transform.position.z // Keep the current Z position (assuming this is a 2D camera)
                );
            }
        }
    }


    private void Update()
    {
        
    }
}