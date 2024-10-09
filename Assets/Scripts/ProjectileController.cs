using System;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    public float speed = 2.5f;
    public Vector3 dir = Vector3.up;

    public int damage = 5;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        /* Move */
        transform.position += dir * (Time.deltaTime * speed);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Destroy(gameObject);
    }
}