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

    private Dictionary<GameObject, GameObject> projectilesWidgets;

    private Scene scene;

    private readonly float PROJECTILE_MAX_DISTANCE = 20.0f;

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
            if (p != null)
            {
            Vector2 vecToP = new Vector2(
                p.transform.position.x - playerCamera.transform.position.x,
                p.transform.position.y - playerCamera.transform.position.y
            );
            /* Set position around camera's boundaries */
            Vector2 dir = vecToP.normalized;

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
                Vector2 wPosition = new Vector2(
                    playerCamera.transform.position.x + (dir * finalT).x,
                    playerCamera.transform.position.y + (dir * finalT).y
                );

                /* Set scale based on distance from camera view */
                float distance = (vecToP - wPosition).magnitude;

                float ratio = 1 - (distance / PROJECTILE_MAX_DISTANCE);

                if (ratio < 0.1f) ratio = 0.1f;
                if (ratio > 1.0f) ratio = 1.0f;

                w.transform.localScale = new Vector3(
                    ratio,
                    ratio,
                    1
                );

                /* Adjust position of widget so that it s fully visible */
                // TODO: It's 2AM and I'm tired, fk it. Very inefficient, maybe think of a better way.
                var sp = w.gameObject.GetComponent<SpriteRenderer>();
                var x = (sp.size.x * w.transform.localScale.x) / 2;
                var y = (sp.size.y / 2 * w.transform.localScale.y) / 2;
                float positionAdjust = Mathf.Sqrt(x * x + y * y);

                wPosition += -dir * positionAdjust;

                w.transform.position = new Vector3(
                    wPosition.x,
                    wPosition.y,
                    0
                );
            }
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
                    projectilesWidgets.Add(pr, Instantiate(projectileWidgetPrefab));
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
