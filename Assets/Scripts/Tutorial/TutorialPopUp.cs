using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

public class TutorialPopUp : MonoBehaviour
{
    [Serializable]
    public struct ObjectAppear
    {
        public GameObject objToAppear;
        public int afterDialog;
    }
    
    public float delayBetweenLetters = 0.2f; /* Seconds */

    public List<TextMeshProUGUI> tmpDialogs;
    private float timeElapsed;
    private List<string> dialogs;

    public List<ObjectAppear> objectsToAppear;
    
    private int currentDialogIdx;
    private int currentLetterIdx;

    public bool finishedCurrentDialog;
    public bool finished;
    public bool canStart;
    
    // Start is called before the first frame update
    void Start()
    {
        if (objectsToAppear != null)
        {
            foreach (var obj in objectsToAppear)
            {
                obj.objToAppear.gameObject.SetActive(false);
            }
        }
        
        if (tmpDialogs != null)
        {
            dialogs = new List<string>();
            foreach (var tmp in tmpDialogs)
            {
                dialogs.Add(tmp.text);
                tmp.text = "";

                tmp.gameObject.SetActive(false);
            }
        }

        currentDialogIdx = 0;
        currentLetterIdx = 0;
        finished = false;
        finishedCurrentDialog = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (canStart)
        {
            timeElapsed += Time.deltaTime;

            UpdateDialogs();
            UpdateTextForCurrentDialog();
            UpdateObjectsToAppear();
            CheckForInput();
        }
    }

    private void UpdateObjectsToAppear()
    {
        if (!finishedCurrentDialog && objectsToAppear != null)
        {
            if (currentDialogIdx < dialogs.Count)
            {
                foreach (var obj in objectsToAppear)
                {
                    if (obj.afterDialog < currentDialogIdx)
                    {
                        obj.objToAppear.gameObject.SetActive(true);
                    }
                }
            }
        }
    }
    
    private void UpdateTextForCurrentDialog()
    {
        if (!finishedCurrentDialog)
        {
            var currentDialog = dialogs[currentDialogIdx];
            var currentTmpDialog = tmpDialogs[currentDialogIdx];

            if (currentLetterIdx < currentDialog.Length)
            {
                if (timeElapsed >= delayBetweenLetters)
                {
                    currentTmpDialog.text += currentDialog[currentLetterIdx];
                    currentLetterIdx++;
                    if (currentLetterIdx < currentDialog.Length && currentDialog[currentLetterIdx] == ' ')
                    {
                        currentTmpDialog.text += currentDialog[currentLetterIdx];
                        currentLetterIdx++;
                    }

                    timeElapsed = 0;
                }
            }
            else
            {
                finishedCurrentDialog = true;
            }
        }
    }

    private void UpdateDialogs()
    {
        if (currentDialogIdx < dialogs.Count && !tmpDialogs[currentDialogIdx].gameObject.activeSelf)
        {
            tmpDialogs[currentDialogIdx].gameObject.SetActive(true);
        }
    }

    private void CheckForInput()
    {
        /* Advance to next dialog */
        if (!finished && Input.GetMouseButtonDown(0))
        {
            var currentDialog = dialogs[currentDialogIdx];
            var currentTmpDialog = tmpDialogs[currentDialogIdx];
            
            if (!finishedCurrentDialog)
            {
                /* Complete current dialog */
                currentTmpDialog.text = currentDialog;
                currentLetterIdx = currentDialog.Length;
                finishedCurrentDialog = true;
            }
            else
            {
                /* Advance to next dialog */
                currentTmpDialog.gameObject.SetActive(false);

                currentDialogIdx++;
                if (currentDialogIdx < dialogs.Count)
                {
                    currentTmpDialog = tmpDialogs[currentDialogIdx];
                    currentTmpDialog.gameObject.SetActive(true);

                    finishedCurrentDialog = false;
                    currentLetterIdx = 0;
                }
                else
                {
                    /* PopUp finished if all dialogs have been shown */
                    finished = true;
                }
            }
        }
    }
}