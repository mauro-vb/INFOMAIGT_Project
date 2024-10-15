using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    private Rigidbody2D rb;
    private ResourceController rc;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rc = GetComponent<ResourceController>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void Move()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        Vector2 moveDirection = new Vector2(moveX, moveY).normalized;
        rb.velocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == Layers.ENVIRONMENT_ABSORBING || other.gameObject.layer == Layers.ENEMIES)
        {
            rc.currentResource -= 10;
        }
    }
}
