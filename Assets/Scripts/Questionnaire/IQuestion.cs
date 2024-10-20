using TMPro;
using UnityEngine;

public class IQuestion : MonoBehaviour
{
    public string question;
    public TMP_Text tmPro;

    private void Start()
    {
        if (tmPro)
        {
            tmPro.text = question;
        }
    }
}
