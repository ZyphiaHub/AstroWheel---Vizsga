using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class ServerSyncGet : MonoBehaviour {
    public string serverUrl = "https://localhost:7178/api/Players";

    void Start()
    {
        StartCoroutine(GetPlayers());
    }

    IEnumerator GetPlayers()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(serverUrl))
        {
            // Kérés elküldése
            yield return webRequest.SendWebRequest();

            // Hibakezelés
            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error: " + webRequest.error);
            } else
            {
                // Sikeres válasz feldolgozása
                string jsonResponse = webRequest.downloadHandler.text;
                Debug.Log("Response: " + jsonResponse);

                // JSON válasz deszerializálása (ha szükséges)
                 PlayerData[] players = JsonUtility.FromJson<PlayerData[]>(jsonResponse);
            }
        }
    }

    [System.Serializable]
    private class PlayerData {
        public int playerId;
        public string playerName;
        public string userId;
        public string playerPassword;
        public int characterId;
        public int islandId;
        public int totalScore;
        public int lastLogin;
        public int createdAt;
        public int isActive;
    }
}