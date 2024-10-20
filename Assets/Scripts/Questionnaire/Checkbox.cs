using System;
using UnityEngine;
using UnityEngine.UI;

public class Checkbox : MonoBehaviour
{
    public int value = 1;
    public QCheckbox qReference;

    private Button btn;
    private Image img;

    private void Start()
    {
        img = GetComponent<Image>();
        btn = GetComponent<Button>();
    }

    public void OnClick()
    {
        qReference.OnCheckboxClick(this);
    }

    public void Reset()
    {
        if (img && btn)
        {
            img.color = btn.colors.normalColor;
        }
    }

    public void Check()
    {
        if (img && btn)
        {
            img.color = btn.colors.pressedColor;
        }
    }
}
