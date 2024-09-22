using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Rigidbody2D rb;
    public ShootingWeapon shootingWeapon;

    private Vector2 moveDirection;
    private Vector2 mousePosition;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        if (Input.GetMouseButtonDown(0)) {
          shootingWeapon.Shoot();
        }

        moveDirection = new Vector2(moveX, moveY).normalized;
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void FixedUpdate()
    {
      rb.velocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);
      Vector2 aimDirection = mousePosition - rb.position;
      float aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg - 90f;
      rb.rotation = aimAngle;
    }
}
