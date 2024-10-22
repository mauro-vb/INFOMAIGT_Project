using UnityEngine;

public class TutorialWeaponController : MonoBehaviour
{
    public TutorialManager managerRef;
    private TutorialPlayerControlsManager playerControlsManager;
    
    public WeaponData weaponData;
    public GameObject projectilePrefab;

    public TutorialResourceController resource; /* To be added from the editor */

    private CircleCollider2D objectCollider;

    public bool canShoot;
    public bool useResource;
    
    void Start()
    {
        objectCollider = GetComponent<CircleCollider2D>();
        resource = GetComponent<TutorialResourceController>();
        playerControlsManager = GetComponent<TutorialPlayerControlsManager>();
    }
    void Update()
    {
        /* Shoot on click */
        if (canShoot)
        {
            if (Input.GetMouseButtonDown(0) && resource.currentResource > weaponData.resourceCost)
            {
                Vector3 dir3D = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position);
                Vector2 dir = new Vector2(dir3D.x, dir3D.y).normalized;
                float angle = Mathf.Acos(Vector2.Dot(dir, Vector2.right));

                /* Spawn the bullet */
                /* spawn position should be outside so it doesn't mess up the physics */
                float epsilon = 0.5f;
                Vector2 distanceVec = dir * (objectCollider.radius * gameObject.transform.localScale.x + epsilon);
                Vector3 spawnPosition = new Vector3(
                    transform.position.x + distanceVec.x,
                    transform.position.y + distanceVec.y,
                    0
                );

                TutorialProjectileController projectile =
                    Instantiate(projectilePrefab, spawnPosition, Quaternion.Euler(0, 0, angle))
                        .GetComponent<TutorialProjectileController>();


                if (projectile)
                {
                    projectile.speed = weaponData.projectileSpeed;
                    projectile.dir = dir;
                    projectile.managerRef = managerRef;
                    projectile.playerControlsManager = playerControlsManager;
                }

                /* Decrease resource */
                if (useResource)
                {
                    resource.currentResource -= weaponData.resourceCost;
                }
            }
        }
    }
}
