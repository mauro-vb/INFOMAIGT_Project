using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TutorialPlayerControlsManager : MonoBehaviour
{
    public GameObject player;
    private List<TutorialResourceController> allFriendlyResourceControllers = new List<TutorialResourceController>();

    void Start()
    {
        FindLargestResourceController();
    }

    public void RegisterResourceController(TutorialResourceController resourceController)
    {
        allFriendlyResourceControllers.Add(resourceController);
    }

    public void UnregisterResourceController(TutorialResourceController resourceController)
    {
        allFriendlyResourceControllers.Remove(resourceController);
    }

    public void FindLargestResourceController()
    {
        if (allFriendlyResourceControllers == null || allFriendlyResourceControllers.Count == 0) return;

        TutorialResourceController playerResourceController = player.GetComponent<TutorialResourceController>();
        if (playerResourceController == null) return;

        TutorialResourceController largestResourceController = allFriendlyResourceControllers
            .Where(rc => rc != null)
            .Append(playerResourceController)
            .OrderByDescending(rc => rc.currentResource)
            .FirstOrDefault();

        if (playerResourceController != largestResourceController && largestResourceController != null)
            ChangeControls(largestResourceController);
    }

    private void ChangeControls(TutorialResourceController largestResourceController)
    {
        if (player.GetComponent<TutorialPlayerController>().canTeleport)
        {
            int newPlayerResourceAmount = largestResourceController.currentResource;
            int newResourceControllerAmount = player.GetComponent<TutorialResourceController>().currentResource;

            Vector3 playerPosition = player.transform.position;
            Vector3 largestResourcePosition = largestResourceController.gameObject.transform.position;

            player.GetComponent<TutorialResourceController>().currentResource = newPlayerResourceAmount;
            largestResourceController.currentResource = newResourceControllerAmount;

            player.transform.position = largestResourcePosition;
            largestResourceController.gameObject.transform.position = playerPosition;
        }
    }
}