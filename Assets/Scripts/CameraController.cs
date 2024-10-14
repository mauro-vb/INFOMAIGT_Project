using System;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject ObjectToFollow;

    private void Update()
    {
        transform.position = new Vector3(
            ObjectToFollow.transform.position.x,
            ObjectToFollow.transform.position.y,
            transform.position.z
        );
    }
}