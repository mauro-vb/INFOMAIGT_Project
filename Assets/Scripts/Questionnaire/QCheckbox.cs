using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QCheckbox : MonoBehaviour
{
    public int answer = -1;
    public List<Checkbox> checkboxes;
    public string question;

    public TMP_Text tmPro;

    private void Start()
    {
        if (tmPro)
        {
            tmPro.text = question;
        }
    }

    public void OnCheckboxClick(Checkbox checkbox)
    {
        foreach (var cb in checkboxes)
        {
            cb.Reset();
        }
        answer = checkbox.value;
        checkbox.Check();

    }
}