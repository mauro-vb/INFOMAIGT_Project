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
        
        request.SetRequestHeader("Content-Type", "application/json");
        
        yield return request.SendWebRequest();
        
        if (request.result == UnityWebRequest.Result.ConnectionError ||
            request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log("Error: " + request.downloadHandler.text);
            onComplete(false);
        }
        else
        {
            Debug.Log("Response: " + request.downloadHandler.text);
            onComplete(true);
        }
    }
}