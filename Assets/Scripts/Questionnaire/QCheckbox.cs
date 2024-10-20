using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class QCheckbox : MonoBehaviour
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