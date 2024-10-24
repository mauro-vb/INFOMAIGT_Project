using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class PlayerControlsManager : MonoBehaviour
{
    public GameObject player ;
    public GameObject inGameUI ;
    private List<ResourceController> allFriendlyResourceControllers = new List<ResourceController>();

    void Start()
    {
        //FindLargestResourceController();
    }

    public void RegisterResourceController(ResourceController resourceController)
    {
        allFriendlyResourceControllers.Add(resourceController);
    }

    public void UnregisterResourceController(ResourceController resourceController)
    {
        allFriendlyResourceControllers.Remove(resourceController);
    }

    public void FindLargestResourceController()
    {
        if (allFriendlyResourceControllers == null || allFriendlyResourceControllers.Count == 0) return;

        ResourceController playerResourceController = player.GetComponent<ResourceController>();
        if (playerResourceController == null) return;

        ResourceController largestResourceController = allFriendlyResourceControllers
            .Where(rc => rc != null)
            .Append(playerResourceController)
            .OrderByDescending(rc => rc.currentResource)
            .FirstOrDefault();

        if (playerResourceController != largestResourceController && largestResourceController != null)
            ChangeControls(largestResourceController);
    }

    private void ChangeControls(ResourceController largestResourceController)
    {
        int newPlayerResourceAmount = largestResourceController.currentResource;
        int newResourceControllerAmount = player.GetComponent<ResourceController>().currentResource;

        Vector3 playerPosition = player.transform.position;
        Vector3 largestResourcePosition = largestResourceController.gameObject.transform.position;

        player.GetComponent<ResourceController>().currentResource = newPlayerResourceAmount;
        largestResourceController.currentResource = newResourceControllerAmount;

        player.transform.position = largestResourcePosition;
        largestResourceController.gameObject.transform.position = playerPosition;

        if (inGameUI != null)
        {
          inGameUI.GetComponent<InGameUIController>().RestartAllWidgets();
        }
    }
}
