using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameUIController : MonoBehaviour
{
    public Camera playerCamera;
    public GameObject projectileWidgetPrefab;
    public Canvas canvas;

    private Dictionary<GameObject, GameObject> projectilesWidgets;

    private Scene scene;

    private Vector2 g0, g1, g2, g3;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(Vector3.zero, new Vector3(g0.x, g0.y, 0));


        Gizmos.color = Color.red;
        Gizmos.DrawLine(Vector3.zero, new Vector3(g1.x, g1.y, 0));

        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(playerCamera.transform.position,
            new Vector3(g2.x, g2.y, 0));

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(playerCamera.transform.position,
            new Vector3(g3.x, g3.y, 0));
    }

    private void Start()
    {
        scene = SceneManager.GetActiveScene();
        projectilesWidgets = new Dictionary<GameObject, GameObject>();
    }

    private void Update()
    {
        CheckForProjectileWidgets();
        UpdateProjectileWidgets();
    }

    private void UpdateProjectileWidgets()
    {
        Bounds playerCameraBounds = OrthographicBounds(playerCamera);

        foreach (var item in projectilesWidgets)
        {
            var p = item.Key;
            var w = item.Value;

            /* Set size based on distance from camera view */

            /* Set position around camera's boundaries */
            Vector2 dir = new Vector2(
                p.transform.position.x - playerCamera.transform.position.x,
                p.transform.position.y - playerCamera.transform.position.y
            ).normalized;

            List<float> finalTs = new List<float>();
            /* Top camera border */
            float t = (playerCameraBounds.size.y / 2) / dir.y;
            if (t > 0) finalTs.Add(t);

            /* Left camera border */
            t = (-playerCameraBounds.size.x / 2) / dir.x;
            if (t > 0) finalTs.Add(t);

            /* Right camera border */
            t = (playerCameraBounds.size.x / 2) / dir.x;
            if (t > 0) finalTs.Add(t);

            /* Bottom camera border */
            t = (-playerCameraBounds.size.y / 2) / dir.y;
            if (t > 0) finalTs.Add(t);

            if (finalTs.Count > 0)
            {
                float finalT = finalTs.Min();

                w.transform.position = new Vector3(
                    playerCamera.transform.position.x + (dir * finalT).x,
                    playerCamera.transform.position.y + (dir * finalT).y,
                    0
                );
            }
        }
    }

    private void CheckForProjectileWidgets()
    {
        Bounds playerCameraBounds = OrthographicBounds(playerCamera);
        var gameobjects = scene.GetRootGameObjects();
        for (int i = 0; i < gameobjects.Length; i++)
        {
            var pr = gameobjects[i];

            if (gameobjects[i].layer == Layers.PROJECTILES)
            {
                var sp = pr.gameObject.GetComponent<SpriteRenderer>();

                bool isVisible = true;
                if (sp)
                {
                    /* Check if projectile is visible to the camera */
                    Rect rectPR = new Rect(
                        new Vector2(pr.transform.position.x - sp.size.x / 2 * pr.transform.localScale.x,
                            pr.transform.position.y - sp.size.y / 2 * pr.transform.localScale.y),
                        new Vector2(sp.size.x * pr.transform.localScale.x, sp.size.y * pr.transform.localScale.y)
                    );

                    Rect rectCA = new Rect(
                        new Vector2(playerCameraBounds.center.x - playerCameraBounds.size.x / 2,
                            playerCameraBounds.center.y - playerCameraBounds.size.y / 2),
                        new Vector2(playerCameraBounds.size.x, playerCameraBounds.size.y)
                    );

                    isVisible = RectToRectIntersection(rectPR, rectCA);
                }

                /* Create a widget if projectile is not visible */
                if (!isVisible && !projectilesWidgets.ContainsKey(pr))
                {
                    projectilesWidgets.Add(pr, Instantiate(projectileWidgetPrefab, canvas.transform));
                }
                else if (isVisible && projectilesWidgets.ContainsKey(pr))
                {
                    Destroy(projectilesWidgets[pr]);
                    projectilesWidgets.Remove(pr);
                }
            }
        }
    }

    public static Bounds OrthographicBounds(Camera camera)
    {
        float screenAspect = (float)Screen.width / (float)Screen.height;
        float cameraHeight = camera.orthographicSize * 2;
        Bounds bounds = new Bounds(
            camera.transform.position,
            new Vector3(cameraHeight * screenAspect, cameraHeight, 0));
        return bounds;
    }

    public static bool RectToRectIntersection(Rect a, Rect b)
    {
        return (a.xMin <= b.xMax &&
                b.xMin <= a.xMax &&
                a.yMin <= b.yMax &&
                b.yMin <= a.yMax);
    }
}