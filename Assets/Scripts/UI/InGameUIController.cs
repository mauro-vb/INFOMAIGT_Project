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
    public GameObject enemyWidgetPrefab;  // New: Widget prefab for enemies

    private Dictionary<GameObject, GameObject> projectilesWidgets;
    private Dictionary<GameObject, GameObject> enemiesWidgets;  // New: Dictionary for enemy widgets

    private Scene scene;

    private readonly float PROJECTILE_MAX_DISTANCE = 20.0f;
    private readonly float ENEMY_MAX_DISTANCE = 30.0f;  // New: Max distance for enemy widget scaling

    private void Start()
    {
        scene = SceneManager.GetActiveScene();
        projectilesWidgets = new Dictionary<GameObject, GameObject>();
        enemiesWidgets = new Dictionary<GameObject, GameObject>();  // New: Initialize dictionary for enemy widgets
    }

    private void Update()
    {
        CheckForProjectileWidgets();
        CheckForEnemyWidgets();  // New: Check for enemy widgets
        UpdateProjectileWidgets();
        UpdateEnemyWidgets();  // New: Update enemy widgets
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
                UpdateWidgetPositionAndScale(p, w, playerCameraBounds, PROJECTILE_MAX_DISTANCE);
            }
            else
            {
              RestartAllWidgets();
            }
        }
    }

    // New: Updates enemy widgets
    private void UpdateEnemyWidgets()
    {
        Bounds playerCameraBounds = OrthographicBounds(playerCamera);

        foreach (var item in enemiesWidgets)
        {
            var enemy = item.Key;
            var widget = item.Value;
            if (enemy != null)
            {
                UpdateWidgetPositionAndScale(enemy, widget, playerCameraBounds, ENEMY_MAX_DISTANCE);
            }
            else
            {
              RestartAllWidgets();
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

                bool isVisible = IsObjectVisible(pr, sp, playerCameraBounds);

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

    // New: Checks for enemy widgets
    private void CheckForEnemyWidgets()
    {
        Bounds playerCameraBounds = OrthographicBounds(playerCamera);
        var gameobjects = scene.GetRootGameObjects();
        for (int i = 0; i < gameobjects.Length; i++)
        {
            var enemy = gameobjects[i];

            if (enemy.layer == Layers.ENEMIES)
            {
                var sp = enemy.gameObject.GetComponent<SpriteRenderer>();

                bool isVisible = IsObjectVisible(enemy, sp, playerCameraBounds);

                /* Create a widget if enemy is not visible */
                if (!isVisible && !enemiesWidgets.ContainsKey(enemy))
                {
                    enemiesWidgets.Add(enemy, Instantiate(enemyWidgetPrefab));
                }
                else if (isVisible && enemiesWidgets.ContainsKey(enemy))
                {
                    Destroy(enemiesWidgets[enemy]);
                    enemiesWidgets.Remove(enemy);
                }
            }
        }
    }

    // New: Refactored function to update widget position and scale (used for both projectiles and enemies)
    private void UpdateWidgetPositionAndScale(GameObject obj, GameObject widget, Bounds cameraBounds, float maxDistance)
    {
        Vector2 vecToObj = new Vector2(
            obj.transform.position.x - playerCamera.transform.position.x,
            obj.transform.position.y - playerCamera.transform.position.y
        );

        Vector2 dir = vecToObj.normalized;
        List<float> finalTs = new List<float>();

        // Calculate intersection points with camera bounds
        float t = (cameraBounds.size.y / 2) / dir.y;
        if (t > 0) finalTs.Add(t);

        t = (-cameraBounds.size.x / 2) / dir.x;
        if (t > 0) finalTs.Add(t);

        t = (cameraBounds.size.x / 2) / dir.x;
        if (t > 0) finalTs.Add(t);

        t = (-cameraBounds.size.y / 2) / dir.y;
        if (t > 0) finalTs.Add(t);

        if (finalTs.Count > 0)
        {
            float finalT = finalTs.Min();
            Vector2 wPosition = new Vector2(
                playerCamera.transform.position.x + (dir * finalT).x,
                playerCamera.transform.position.y + (dir * finalT).y
            );

            float distance = (vecToObj - wPosition).magnitude;
            float ratio = 1 - (distance / maxDistance);
            ratio = Mathf.Clamp(ratio, 0.1f, 1.0f);

            widget.transform.localScale = new Vector3(ratio, ratio, 1);

            var sp = widget.gameObject.GetComponent<SpriteRenderer>();
            var x = (sp.size.x * widget.transform.localScale.x) / 2;
            var y = (sp.size.y / 2 * widget.transform.localScale.y) / 2;
            float positionAdjust = Mathf.Sqrt(x * x + y * y);

            wPosition += -dir * positionAdjust;

            widget.transform.position = new Vector3(wPosition.x, wPosition.y, 0);
        }
    }

    private static bool IsObjectVisible(GameObject obj, SpriteRenderer sp, Bounds cameraBounds)
    {
        if (!sp) return false;

        Rect rectObj = new Rect(
            new Vector2(
                obj.transform.position.x - sp.size.x / 2 * obj.transform.localScale.x,
                obj.transform.position.y - sp.size.y / 2 * obj.transform.localScale.y),
            new Vector2(sp.size.x * obj.transform.localScale.x, sp.size.y * obj.transform.localScale.y)
        );

        Rect rectCam = new Rect(
            new Vector2(cameraBounds.center.x - cameraBounds.size.x / 2,
                cameraBounds.center.y - cameraBounds.size.y / 2),
            new Vector2(cameraBounds.size.x, cameraBounds.size.y)
        );

        return RectToRectIntersection(rectObj, rectCam);
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

    public void RestartAllWidgets()
    {
        // Remove all projectile widgets
        foreach (var widget in projectilesWidgets.Values)
        {
            if (widget != null)
            {
                Destroy(widget);
            }
        }
        projectilesWidgets.Clear();  // Clear the dictionary

        // Remove all enemy widgets
        foreach (var widget in enemiesWidgets.Values)
        {
            if (widget != null)
            {
                Destroy(widget);
            }
        }
        enemiesWidgets.Clear();  // Clear the dictionary

        // Recreate widgets for all projectiles and enemies
        CheckForProjectileWidgets();
        CheckForEnemyWidgets();
    }

}
