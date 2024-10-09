using System;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    public float speed = 2.5f;
    public Vector3 dir = Vector3.up;
    
    void Start()
    {
    }
    
    // Update is called once per frame
    void Update()
    {
        /* Move */
        transform.position += dir * (Time.deltaTime * speed);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Destroy(gameObject);
    }
}