using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QCombobox : IQuestion
{
    public int answer = -1;
    public List<Checkbox> checkboxes;
    
    public override void OnCheckboxClick(Checkbox checkbox)
    {
        foreach (var cb in checkboxes)
        {
            cb.Reset();
        }
        answer = checkbox.value;
        checkbox.Check();
    }
}