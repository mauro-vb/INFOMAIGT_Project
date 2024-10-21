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
    public List<IQuestion> questions;
    public GameObject errorLog;
    public GameObject tyLog;
    public TextMeshProUGUI tyMessage;

    public string previousSceneName;
    
    private MailSender mailSender;

    private void Start()
    {
        /* Get data from previous scene */
        if (QDataManager.Instance != null)
        {
            previousSceneName = QDataManager.Instance.CurrentSceneName;
        }

        mailSender = new MailSender();
        if (errorLog)
        {
            errorLog.SetActive(false);
        }

        if (tyLog)
        {
            tyLog.SetActive(false);
        }
    }

    public void Submit()
    {
        if (questions != null)
        {
            foreach (var question in questions)
            {
                if (question.GetType() == typeof(QCombobox))
                {
                    if (question.isRequired && ((QCombobox)question).answer == -1)
                    {
                        StartCoroutine(nameof(ShowError));
                        /* TODO: Scroll to question and some UX */
                        return;
                    }
                }
                else if (question.GetType() == typeof(QOpen))
                {
                    if (question.isRequired &&((QOpen)question).answer == "")
                    {
                        StartCoroutine(nameof(ShowError));
                        /* TODO: Scroll to question and some UX */
                        return;
                    }
                }
                else if (question.GetType() == typeof(QCheckbox))
                {
                    if (question.isRequired &&((QCheckbox)question).answer == false)
                    {
                        StartCoroutine(nameof(ShowError));
                        /* TODO: Scroll to question and some UX */
                        return;
                    } 
                }
            }

            StartCoroutine(SendAnswersMail());
        }
    }

    private IEnumerator SendAnswersMail()
    {
        string body = "Data for level " + previousSceneName + "<br/><br/>";
        if (questions != null)
        {
            /* HEADERS */
            body += "No.Crt,Question,Answer<br/>";
            for (int i = 0; i < questions.Count; i++)
            {
                body += i + "," + questions[i].question;
                if (questions[i].GetType() == typeof(QCombobox))
                {
                    body += "," + ((QCombobox)questions[i]).answer;
                }
                else if (questions[i].GetType() == typeof(QOpen))
                {
                    body += "," + ((QOpen)questions[i]).answer;
                }
                else if (questions[i].GetType() == typeof(QCheckbox))
                {
                    body += "," + ((QCheckbox)questions[i]).answer;
                }

                body += "<br/>";
            }

            MailSender.EmailData emailData = new MailSender.EmailData();
            emailData.From = new MailSender.RecipientData("vlad.cpuscaru@gmail.com", "Unity AIGT Project");
            emailData.To = new List<MailSender.RecipientData>()
                { new("vlad.cpuscaru@gmail.com", "Vlad Puscaru") };
            emailData.Subject = "[AIGT] - Questionnaire Data";
            emailData.TextPart = body;
            emailData.HTMLPart = body;

            MailSender.BodyData bodyData = new MailSender.BodyData();
            bodyData.Messages = new List<MailSender.EmailData>();
            bodyData.Messages.Add(emailData);
            StartCoroutine(mailSender.SendEmail(bodyData, OnQuestionnaireComplete));
            
            yield return new WaitForSeconds(2f);

            if (QDataManager.Instance != null)
            {
                SceneManager.LoadScene(QDataManager.Instance.NextSceneName);
            }
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

    private IEnumerator ShowThankYou(bool mailSuccess)
    {
        if (errorLog)
        {
            tyLog.gameObject.SetActive(true);

            if (tyMessage)
            {
                tyMessage.text = mailSuccess
                    ? "Thank you! We got your answers!"
                    : "Thank you! Unfortunately, we didn't get your answers because of a technical error. Please contact us!";
            }

            yield return new WaitForSeconds(4.0f);

            tyLog.gameObject.SetActive(false);
        }
    }

    private void OnQuestionnaireComplete(bool successSendingEmail)
    {
        if (successSendingEmail)
        {
            Debug.Log("Mail sent successfully!");
        }
        else
        {
            Debug.LogError("Failed to send mail.");
        }
        StartCoroutine(nameof(ShowThankYou), successSendingEmail);
    }
}