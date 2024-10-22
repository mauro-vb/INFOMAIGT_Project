using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class TextWriter : MonoBehaviour
{
    public float delayBetweenLetters = 0.2f; /* Seconds */

    private TextMeshProUGUI tmp;
    private float timeElapsed;
    private string text;
    private int currentIdx;
    
    // Start is called before the first frame update
    void Start()
    {
        tmp = GetComponent<TextMeshProUGUI>();
        
        text = tmp.text;
        tmp.text = "";
        currentIdx = 0;
    }

    // Update is called once per frame
    void Update()
    {
        /* Automatically paste all text when click */
        if (Input.GetMouseButtonDown(0) && currentIdx < text.Length)
        {
            tmp.text = text;
            currentIdx = text.Length;
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
    }
}
