using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mailjet.Client;
using Mailjet.Client.TransactionalEmails;
using UnityEngine;
using UnityEngine.Networking;

public class MailSender
{
    /* Using corsproxy for bypassing CORS without handling a separate server */
    private static readonly string BASE_URL = "https://api.mailjet.com/v3.1";
    private static readonly string API_KEY = "9c085655c76f76a4cca85e565d4023a6"; /* really secure haha */
    private static readonly string SECRET_KEY = "041e0fee9a00dec54e54e9c3fd539d8d"; /* really secure haha */

    private static readonly bool SANDBOX_MODE = false;

    private MailjetClient client;
    private TransactionalEmail email;

    public MailSender()
    {
        client = new MailjetClient(
            API_KEY,
            SECRET_KEY);
    }

    // https://dev.mailjet.com/email/guides/send-api-v31/#sandbox-mode
    // {
    //     "Messages":[
    //     {
    //         "From": {
    //             "Email": "pilot@mailjet.com",
    //             "Name": "Mailjet Pilot"
    //         },
    //         "To": [
    //         {
    //             "Email": "passenger1@mailjet.com",
    //             "Name": "passenger 1"
    //         }
    //         ],
    //         "Subject": "Your email flight plan!",
    //         "TextPart": "Dear passenger 1, welcome to Mailjet! May the delivery force be with you!",
    //         "HTMLPart": "<h3>Dear passenger 1, welcome to <a href=\"https://www.mailjet.com/\">Mailjet</a>!<br />May the delivery force be with you!"
    //     }
    //     ],
    //     "SandboxMode":true
    // }
    [System.Serializable]
    public struct RecipientData
    {
        public string Email;
        public string Name;

        public RecipientData(string email, string name)
        {
            Email = email;
            Name = name;
        }
    }

    [System.Serializable]
    public struct EmailData
    {
        public RecipientData From;
        public List<RecipientData> To;
        public string Subject;
        public string TextPart;
        public string HTMLPart;
    }

    [System.Serializable]
    public struct BodyData
    {
        public List<EmailData> Messages;
        public bool SandboxMode;
    }

    public async Task<bool> SendRequest()
    {
        Debug.Log("WTF");
        var response = await client.SendTransactionalEmailAsync(email);
        Debug.Log("WTF AFTER");
        Debug.Log(response);
        return response.Messages.Length == 1;
    }

    public IEnumerator SendEmail(BodyData body, System.Action<bool> onComplete)
    {
        var url = BASE_URL + "/send"; // Replace with your API URL
        UnityWebRequest request = new UnityWebRequest(url, "POST");

        body.SandboxMode = SANDBOX_MODE;
        
        Debug.Log("TEEEEST");

        // construct your email with builder
        email = new TransactionalEmailBuilder()
            .WithFrom(new SendContact("vlad.cpuscaru@gmail.com"))
            .WithSubject("INFOMAIGT Game Answer")
            .WithHtmlPart(body.Messages[0].HTMLPart)
            .WithTo(new SendContact("vlad.cpuscaru@gmail.com"))
            .Build();

        // invoke API to send email
        // var t = Task.Run(SendRequest); /* Don't use Task for WebGL. it hangs indefinitely */
        var t = SendRequest();
        yield return new WaitUntil(() => t.IsCompleted);
        
        if (t.Result)
        {
            onComplete(true);
        }
        else
        {
            onComplete(false);
        }

        //
        // string bodyJson = JsonUtility.ToJson(body);
        // byte[] bodyRaw = Encoding.UTF8.GetBytes(bodyJson);
        // request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        // request.downloadHandler = new DownloadHandlerBuffer();
        //
        // request.SetRequestHeader("Content-Type", "application/json");
        //
        // /* Basic Auth user:pass base64 encoded */
        // var userPass = System.Text.Encoding.UTF8.GetBytes(API_KEY + ":" + SECRET_KEY);
        // var auth = System.Convert.ToBase64String(userPass);
        // request.SetRequestHeader("Authorization", "Basic " + auth);
        //
        // yield return request.SendWebRequest();
        //
        // if (request.result == UnityWebRequest.Result.ConnectionError ||
        //     request.result == UnityWebRequest.Result.ProtocolError)
        // {
        //     Debug.Log("Error: " + request.downloadHandler.text);
        //     onComplete(false);
        // }
        // else
        // {
        //     Debug.Log("Response: " + request.downloadHandler.text);
        //     onComplete(true);
        // }
    }
}