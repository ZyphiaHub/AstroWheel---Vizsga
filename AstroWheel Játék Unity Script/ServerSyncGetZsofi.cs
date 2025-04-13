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
            // K�r�s elk�ld�se
            yield return webRequest.SendWebRequest();

            // Hibakezel�s
            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error: " + webRequest.error);
            } else
            {
                // Sikeres v�lasz feldolgoz�sa
                string jsonResponse = webRequest.downloadHandler.text;
                Debug.Log("Response: " + jsonResponse);

                // JSON v�lasz deszerializ�l�sa (ha sz�ks�ges)
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