using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Questionnaire : MonoBehaviour
{
    public List<QCheckbox> questions;
    public GameObject errorLog;

    private void Start()
    {
        if (errorLog)
        {
            errorLog.SetActive(false);
        }
    }

    public void Submit()
    {
        if (questions != null)
        {
            foreach (var question in questions)
            {
                if (question.answer == -1)
                {
                    StartCoroutine(nameof(ShowError));
                    /* TODO: Scroll to question and some UX */
                    return;
                }
            }

            SendAnswersMail();
            /* TODO: Start next scene in the build */
        }
    }

    private void SendAnswersMail()
    {
        string emailTo = "vlad.cpuscaru@gmail.com";
        string subject = "Unity Form";
        string body = "";
        if (questions != null)
        {
            /* HEADERS */
            body += "No.Crt,Question,Answer\n";
            for (int i = 0; i < questions.Count; i++)
            {
                body += i + "," + questions[i].question + "," + questions[i].answer + "\n";
            }

            MailSender.SendEmail(subject, emailTo, body);
        }
    }

    private IEnumerator ShowError()
    {
        if (errorLog)
        {
            errorLog.gameObject.SetActive(true);

            yield return new WaitForSeconds(2.0f);

            errorLog.gameObject.SetActive(false);
        }
    }
}

public class TextMeshProGUI
{
}