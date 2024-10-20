using System;
using System.Drawing;
using UnityEngine;
using Color = UnityEngine.Color;

public class WeaponController : MonoBehaviour
{
    public WeaponData weaponData;
    public GameObject projectilePrefab;

    public ResourceController resource; /* To be added from the editor */

    private CircleCollider2D objectCollider;
    private Rigidbody2D rb;

    private Vector2 gizmosDir;
    private Vector2 gizmosSpawnPosition;

    void Start()
    {
        objectCollider = GetComponent<CircleCollider2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    // private void OnDrawGizmos()
    // {
    //     Gizmos.color = Color.magenta;
    //     Gizmos.DrawRay(Vector3.zero, transform.position);
    //
    //     Gizmos.color = Color.blue;
    //     Gizmos.DrawRay(Vector3.zero, gizmosDir);
    //
    //     Gizmos.color = Color.yellow;
    //     Gizmos.DrawRay(Vector3.zero, gizmosSpawnPosition);
    // }

    // Update is called once per frame
    void Update()
    {
        /* Shoot on click */
        if (Input.GetMouseButtonDown(0) && resource.currentResource > weaponData.resourceCost)
        {
            Vector3 dir3D = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position);
            Vector2 dir = new Vector2(dir3D.x, dir3D.y).normalized;
            float angle = Mathf.Acos(Vector2.Dot(dir, Vector2.right));

            /* Recoil - might be nice */
            // if (rb)
            // {
            //     rb.AddForce(-dir * 5.0f, ForceMode2D.Impulse);
            // }

            /* Spawn the bullet */
            /* spawn position should be outside so it doesn't mess up the physics */
            float epsilon = 0.1f;
            Vector2 distanceVec = dir * (objectCollider.radius * gameObject.transform.localScale.x + epsilon);
            gizmosDir = distanceVec;
            Vector3 spawnPosition = new Vector3(
                transform.position.x + distanceVec.x,
                transform.position.y + distanceVec.y,
                0
            );


            gizmosSpawnPosition = new Vector2(spawnPosition.x, spawnPosition.y);

            ProjectileController projectile =
                Instantiate(projectilePrefab, spawnPosition, Quaternion.Euler(0, 0, angle))
                    .GetComponent<ProjectileController>();


            if (projectile)
            {
                projectile.speed = weaponData.projectileSpeed;
                projectile.dir = dir;
                projectile.resourceCost = weaponData.resourceCost;
            }

            /* Decrease resource */
            resource.currentResource -= weaponData.resourceCost;
        }
    }
}
