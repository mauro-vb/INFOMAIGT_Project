using UnityEngine;

public class TutorialPlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private ResourceController rc;

    public Sprite[] forwardSprites;
    public Sprite[] sidewaysSprites;
    public Sprite[] backwardSprites;
    public float animationSpeed = 0.3f;
    public float moveSpeed = 5.0f;
    private int currentFrame;
    private float timer;

    /* Tutorial variables */
    public bool canMove;
    public bool canFire;

    void Start()
    {
        rc = GetComponent<ResourceController>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        currentFrame = 0;
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Timer();
    }

    private void Move()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        Vector2 moveDirection = new Vector2(moveX, moveY).normalized;
        if (canMove)
        {
            rb.velocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);
        }

        if (moveDirection != Vector2.zero)
        {
            animationSpeed = 0.1f;
            if (moveDirection.x == 0)
            {
                if (moveDirection.y > 0)
                {
                    sr.sprite = backwardSprites[currentFrame];
                }
                else
                {
                    sr.sprite = forwardSprites[currentFrame];
                }
            }
            else
            {
                sr.sprite = sidewaysSprites[currentFrame];
                if (moveDirection.x < 0)
                {
                    sr.flipX = false;
                }
                else
                {
                    sr.flipX = true;
                }
            }
        }
        else
        {
            animationSpeed = 0.2f;
            sr.sprite = forwardSprites[currentFrame];
        }
    }

    private void Timer()
    {
        timer += Time.deltaTime;
        if (timer >= animationSpeed)
        {
            currentFrame++;
            if (currentFrame >= 5)
            {
                currentFrame = 0;
            }

            timer = 0f;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if ((other.gameObject.layer == Layers.ENVIRONMENT_ABSORBING || other.gameObject.layer == Layers.ENEMIES) &&
            rc.currentResource > 10)
        {
            rc.currentResource -= 10;
        }
    }
}