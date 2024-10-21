using TMPro;
using UnityEngine;

public abstract class IQuestion : MonoBehaviour
{
    public string question;
    public TMP_Text tmPro;

    public bool isRequired = true;

    public abstract void OnCheckboxClick(Checkbox checkbox);
    
    private void Start()
    {
        if (tmPro)
        {
            tmPro.text = question;
        }
    }
}
