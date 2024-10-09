using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    
    void Start()
    {
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

        Vector3 moveDirection = new Vector2(moveX, moveY).normalized;
        transform.Translate(moveDirection * (moveSpeed * Time.deltaTime));
    }
}