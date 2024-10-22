using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class TutorialPopUp : MonoBehaviour
{
    public float delayBetweenLetters = 0.2f; /* Seconds */

    public TextMeshProUGUI tmp;
    private float timeElapsed;
    private string text;
    private int currentIdx;

    public bool finished;
    public bool canStart;
    
    // Start is called before the first frame update
    void Start()
    {
        text = tmp.text;
        tmp.text = "";
        currentIdx = 0;
        finished = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (canStart)
        {
            /* Automatically paste all text when click */
            if (Input.GetMouseButtonDown(0) && currentIdx < text.Length)
            {
                tmp.text = text;
                currentIdx = text.Length;
                finished = true;
            }

            if (currentIdx < text.Length)
            {
                timeElapsed += Time.deltaTime;
                if (timeElapsed >= delayBetweenLetters)
                {
                    /* Advance letter */
                    tmp.text += text[currentIdx];

                    // Skip spaces from the counter
                    if (text[currentIdx] == ' ')
                    {
                        tmp.text += text[currentIdx];
                    }

                    /* Reset counter */
                    timeElapsed = 0;
                    currentIdx++;
                }
            }
            else
            {
                finished = true;
            }
        }
    }
}
