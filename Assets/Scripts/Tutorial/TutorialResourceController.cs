using UnityEngine;

public class TutorialResourceController : MonoBehaviour
{
    public int maxResource = 100;
    public int currentResource = 100;

    public void Update()
    {
        if (gameObject != null)
        {
            if (currentResource <= 0)
            {
                /* Dead */
                Destroy(gameObject);
            }
        }
    }
}
