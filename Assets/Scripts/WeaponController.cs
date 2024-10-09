using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public WeaponData weaponData;
    public GameObject projectilePrefab;

    public ResourceController resource; /* To be added from the editor */

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        /* Shoot on click */
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 dir = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
            float angle = Mathf.Acos(Vector3.Dot(dir, Vector3.right));
            ProjectileController projectile =
                Instantiate(projectilePrefab, transform.position, Quaternion.Euler(0, 0, angle))
                    .GetComponent<ProjectileController>();


            if (projectile)
            {
                projectile.speed = weaponData.projectileSpeed;
                projectile.dir = dir;
            }
            
            /* Decrease resource */
            resource.currentResource -= weaponData.resourceCost;
        }
    }
}