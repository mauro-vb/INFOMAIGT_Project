using TMPro;
using UnityEngine;

public class ResourceController : MonoBehaviour
{
    public int maxResource = 100;
    public int currentResource = 100;

    private TextMeshProUGUI hpDisplay; // Reference to the hp display

    public void Start()
    {
        if (transform.tag == "Player")
        {
            GameObject inGameUI = GameObject.Find("Canvas");
            if (inGameUI != null)
            {
                hpDisplay = inGameUI.transform.Find("Hp").transform.Find("HpDisplay").gameObject.GetComponent<TextMeshProUGUI>();
             }
        }
    }

    public void Update()
    {
        if (gameObject != null)
        {
            if (hpDisplay != null)
            {
                hpDisplay.text = "HP: " + currentResource + "/" + maxResource;
            }

            if (currentResource <= 0)
            {
                /* Dead */
                Destroy(gameObject);

                if (hpDisplay != null)
                {
                    hpDisplay.text = "DEAD";
                    hpDisplay.color = new Color(1f, 0.4f, 0.4f);
                }
            }
        }
    }
}
