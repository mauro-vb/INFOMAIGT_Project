using UnityEngine;

public class ResourceController : MonoBehaviour
{
    public int maxResource = 100;
    public int currentResource = 100;

    public void Start()
    {
        
    }

    public void Update()
    {
        if (currentResource <= 0)
        {
            /* Dead */
            Destroy(gameObject);
        }
    }
}
