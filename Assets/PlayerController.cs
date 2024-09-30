using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Rigidbody2D rb;
    public ShootingWeapon shootingWeapon;

    private Vector2 moveDirection;
    private Vector2 mousePosition;

    void Start()
    {
        shootingWeapon = GetComponent<ShootingWeapon>();
        rb = GetComponent<Rigidbody2D>(); // Assuming Rigidbody2D is already part of the prefab
    }

    // Update is called once per frame
    void Update()
    {
      Move();
    }

    private void FixedUpdate()
    {
      Rotate();
      if (Input.GetMouseButtonDown(0)) {
        shootingWeapon.Shoot();
      }
    }

    private void Move()
    {
      float moveX = Input.GetAxis("Horizontal");
      float moveY = Input.GetAxis("Vertical");

      moveDirection = new Vector2(moveX, moveY).normalized;
      mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void Rotate()
    {
      rb.velocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);
      Vector2 aimDirection = mousePosition - rb.position;
      float aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg - 90f;
      rb.rotation = aimAngle;
    }
}
