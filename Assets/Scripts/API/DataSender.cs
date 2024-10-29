using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Mailjet.Client;
using Mailjet.Client.TransactionalEmails;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Purchasing.MiniJSON;

public class DataSender
{
    private static readonly string BASE_URL = "https://infomaigt-game-server.onrender.com";

    [Serializable]
    public struct DataStructure
    {
        public string name;
        public string level;
        public GameLogger.InGameData inGameData;
        public Dictionary<string, int> questionAnswers;
    }

    public IEnumerator SendData(DataStructure data, System.Action<bool> onComplete)
    {
        var url = BASE_URL + "/api/data"; // Replace with your API URL
        UnityWebRequest request = new UnityWebRequest(url, "POST");

        string bodyJson = JsonConvert.SerializeObject(data);
        byte[] bodyRaw = Encoding.UTF8.GetBytes(bodyJson);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        
        /* Because we are using a free tier for the server
         * it goes asleep after 15 mins of inactivity
         * If that happens, the game is stuck until the server comes up
         * In that time, the users usually press the submit button a lot of times,
         * because they think the game is stuck (which it is)
         * So instead of making the game stuck, we just set waiting for a response
         * for 1 second and then proceed anyway
         * The server queues up the requests so no data should be lost
         */
        
        request.SetRequestHeader("Content-Type", "application/json");
        request.SendWebRequest();

        yield return new WaitForSeconds(0.5f);
        // yield return request.SendWebRequest();
        
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
        onComplete(true);
    }
}