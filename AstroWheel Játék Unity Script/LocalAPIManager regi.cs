using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class APIManager : MonoBehaviour {
    private string apiUrl = "https://your-api-url.com/api/Inventory";

    public void GetInventory(int playerId)
    {
        StartCoroutine(GetInventoryCoroutine(playerId));
    }

    private IEnumerator GetInventoryCoroutine(int playerId)
    {
        using (UnityWebRequest request = UnityWebRequest.Get($"{apiUrl}/{playerId}"))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Inventory Data: " + request.downloadHandler.text);
            } else
            {
                Debug.LogError("Error: " + request.error);
            }
        }
    }
}
