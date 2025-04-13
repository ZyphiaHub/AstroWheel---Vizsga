using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class ServerSync : MonoBehaviour {
    private string serverUrl = "https://localhost:7178/swagger/index.html";

    public void SyncWithServer(int materials, int points)
    {
        StartCoroutine(SendDataToServer(materials, points));
    }

    IEnumerator SendDataToServer(int materials, int points)
    {
        string jsonData = JsonUtility.ToJson(new PlayerData { materials = materials, points = points });

        using (UnityWebRequest webRequest = new UnityWebRequest(serverUrl, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
            webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            webRequest.SetRequestHeader("Content-Type", "application/json");

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Data synced with server successfully!");
            } else
            {
                Debug.LogError("Error syncing data: " + webRequest.error);
            }
        }
    }

    [System.Serializable]
    private class PlayerData {
        public int materials;
        public int points;
    }
}