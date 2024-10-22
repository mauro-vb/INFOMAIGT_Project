using UnityEngine;

public class TutorialProjectileController : MonoBehaviour
{
    public TutorialManager managerRef;
    
    public float speed = 0;
    public Vector2 dir = Vector2.up;

    private Rigidbody2D rb;
    
    public TutorialPlayerControlsManager playerControlsManager;


    public bool canCollideWithEnemies;
    public bool canCollideWithAbsorbing;
    public bool canCollideWithBouncing;
    public bool canCollideWithPlayer;
    public bool canCollideWithProjectiles;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        rb.velocity = new Vector2(dir.x * speed, dir.y * speed);

        managerRef.SetProjectileParams(this);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == Layers.ENEMIES && canCollideWithEnemies)
        {
            playerControlsManager.UnregisterResourceController(GetComponent<TutorialResourceController>());
            Destroy(gameObject);
        }
        
        if (other.gameObject.layer == Layers.ENVIRONMENT_ABSORBING && canCollideWithAbsorbing)
        {
            playerControlsManager.UnregisterResourceController(GetComponent<TutorialResourceController>());
            Destroy(gameObject);
        }

        if (other.gameObject.layer == Layers.PLAYER && canCollideWithPlayer)
        {
            var playerResource = other.gameObject.GetComponent<TutorialResourceController>();
            if (playerResource)
            {
                playerResource.currentResource += GetComponent<TutorialResourceController>().currentResource;
            }
            Destroy(gameObject);
        }

        if (other.gameObject.layer == Layers.PROJECTILES && canCollideWithProjectiles)
        {
            // Ensure that only one of the two projectiles handles the collision.
            if (GetInstanceID() < other.gameObject.GetInstanceID())
            {
                var bulletResource = other.gameObject.GetComponent<TutorialResourceController>();
                var rigidBody = other.gameObject.GetComponent<Rigidbody2D>();
                if (bulletResource)
                {
                    bulletResource.currentResource += GetComponent<TutorialResourceController>().currentResource;
                    playerControlsManager.RegisterResourceController(bulletResource);
                }
                if (rigidBody)
                {
                  rigidBody.velocity = new Vector2(0,0);
                }
                playerControlsManager.UnregisterResourceController(GetComponent<TutorialResourceController>());
                Destroy(gameObject);
                playerControlsManager.FindLargestResourceController();
            }
        }

        if (other.gameObject.layer == Layers.ENVIRONMENT_BOUNCING && canCollideWithBouncing)
        {
            /* Bounce at a 45 degrees angles */
            Vector2 collisionNormal = other.contacts[0].normal;
            Vector2 reflectedVelocity = Vector2.Reflect(dir, collisionNormal);

            dir = reflectedVelocity;
            rb.velocity = new Vector2(dir.x * speed, dir.y * speed);
        }

        if (other.gameObject.layer == Layers.ENVIRONMENT_WALL)
        {
            rb.velocity = Vector2.zero;
        }
    }
}