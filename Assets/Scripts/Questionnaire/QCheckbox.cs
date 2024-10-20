using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QCheckbox : IQuestion
{
    public int answer = -1;
    public List<Checkbox> checkboxes;
    
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