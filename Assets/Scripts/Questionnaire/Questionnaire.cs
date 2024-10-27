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
    private DataSender dataSender;

    private SceneFadeManager sceneFadeManager;

    private void Start()
    {
        sceneFadeManager = GameObject.Find("SceneFadeManager")?.GetComponent<SceneFadeManager>();

        /* Get data from previous scene */
        if (QDataManager.Instance != null)
        {
            previousSceneName = QDataManager.Instance.CurrentSceneName;
        }

        mailSender = new MailSender();
        dataSender = new DataSender();
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
                    if (question.isRequired && ((QOpen)question).answer == "")
                    {
                        StartCoroutine(nameof(ShowError));
                        /* TODO: Scroll to question and some UX */
                        return;
                    }
                }
                else if (question.GetType() == typeof(QCheckbox))
                {
                    if (question.isRequired && ((QCheckbox)question).answer == false)
                    {
                        StartCoroutine(nameof(ShowError));
                        /* TODO: Scroll to question and some UX */
                        return;
                    }
                }
            }

            SendAnswersMail();
        }
    }

    private void SendAnswersMail()
    {
        DataSender.DataStructure data = new DataSender.DataStructure();
        data.name = QDataManager.Instance.PlayerName.Length > 0 ? QDataManager.Instance.PlayerName : "NOT-FILLED";
        data.inGameData = QDataManager.Instance.inGameData;

        data.questionAnswers = new Dictionary<string, int>();

        // string body = "Data from player " + QDataManager.Instance.PlayerName + "<br/><br/>";

        // body += "Data for level " + previousSceneName + "<br/><br/>";
        /* In Game Data */
        // body += QDataManager.Instance.inGameData.GetStringHTML() + "<br/>";

        /* Questionnaire */
        if (questions != null)
        {
            /* HEADERS */
            // body += "No.Crt,Question,Answer<br/>";
            for (int i = 0; i < questions.Count; i++)
            {
                data.questionAnswers.Add(questions[i].question, ((QCombobox)questions[i]).answer);
                // body += i + "," + questions[i].question;
                // if (questions[i].GetType() == typeof(QCombobox))
                // {
                //     body += "," + ((QCombobox)questions[i]).answer;
                // }
                // else if (questions[i].GetType() == typeof(QOpen))
                // {
                //     body += "," + ((QOpen)questions[i]).answer;
                // }
                // else if (questions[i].GetType() == typeof(QCheckbox))
                // {
                //     body += "," + ((QCheckbox)questions[i]).answer;
                // }
                //
                // body += "<br/>";
            }

            // MailSender.EmailData emailData = new MailSender.EmailData();
            // emailData.From = new MailSender.RecipientData("vlad.cpuscaru@gmail.com", "Unity AIGT Project");
            // emailData.To = new List<MailSender.RecipientData>()
            //     { new("vlad.cpuscaru@gmail.com", "Vlad Puscaru") };
            // emailData.Subject = "[AIGT] - Questionnaire Data";
            // emailData.TextPart = body;
            // emailData.HTMLPart = body;
            //
            // MailSender.BodyData bodyData = new MailSender.BodyData();
            // bodyData.Messages = new List<MailSender.EmailData>();
            // bodyData.Messages.Add(emailData);
            // StartCoroutine(mailSender.SendEmail(bodyData, OnQuestionnaireComplete));

            StartCoroutine(dataSender.SendData(data, OnQuestionnaireComplete));
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
        
        if (QDataManager.Instance != null)
        {
            var idxNext = SceneUtility.GetBuildIndexByScenePath(QDataManager.Instance.NextSceneName);
            if (idxNext < SceneManager.sceneCountInBuildSettings)
            {
                var currentScene = QDataManager.Instance.NextSceneName;
                var nextScene = SceneUtility.GetScenePathByBuildIndex(idxNext + 1);
                QDataManager.Instance.UpdateScenes(currentScene, nextScene);
                QDataManager.Instance.ResetLoggerData();

                if (sceneFadeManager)
                {
                    StartCoroutine(sceneFadeManager.FadeOut());
                }

                SceneManager.LoadScene(QDataManager.Instance.CurrentSceneName);
            }
        }
    }
}