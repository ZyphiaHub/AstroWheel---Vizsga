using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class APIClient : MonoBehaviour {
    public APIConfig apiConfig; 

    
    public static APIClient Instance { get;  private set; }
    public Inventory inventory { get; set; }
    public CraftedInventory craftedInventory { get; set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            //Debug.Log("APIClient initialized.");
        } else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        //StartCoroutine(FetchAndDisplayPlayers());
    }


    // GET kérés a játékosok lekérdezéséhez
    public IEnumerator GetPlayers(System.Action<PlayerData[]> onSuccess, System.Action<string> onError)
    {
        if (!GameManager.Instance.IsInternetAvailable())
        {
            string email = PlayerPrefs.GetString("RegisteredEmail", "");
            string password = PlayerPrefs.GetString("RegisteredPassword", "");
            Debug.Log("No internet connection. Loading data from local SQLite database...");
            LocalDatabaseManager.Instance.LoadPlayerDataByEmailAndPassword(email, password);
            yield break;
        }
        string url = apiConfig.playersGetUrl;

        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                onError?.Invoke(webRequest.error);
            } else
            {
                string jsonResponse = webRequest.downloadHandler.text;
                PlayerDataWrapper wrapper = JsonUtility.FromJson<PlayerDataWrapper>("{\"players\":" + jsonResponse + "}");
                onSuccess?.Invoke(wrapper.players);
            }
        }
    }

    public IEnumerator Register(string email, string password, string playerName, System.Action<string> onSuccess, System.Action<string> onError)
    {
        string url = "https://astrowheelapi.onrender.com/api/auth/register"; 

        // Regisztrációs adatok összeállítása
        var registrationData = new RegisterRequest
        {
            email = email,
            password = password,
            playerName = playerName
        };
        string jsonData = JsonUtility.ToJson(registrationData);

        // POST kérés elküldése
        using (UnityWebRequest webRequest = new UnityWebRequest(url, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
            webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            webRequest.SetRequestHeader("Content-Type", "application/json");

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                onError?.Invoke(webRequest.error);
            } else
            {
                onSuccess?.Invoke(webRequest.downloadHandler.text);
            }
        }
    }

    public IEnumerator Login(string email, string password, System.Action<string> onSuccess, System.Action<string> onError)
    {
        string url = "https://astrowheelapi.onrender.com/api/auth/login"; 

        // Bejelentkezési adatok összeállítása
        var loginData = new LoginRequest
        {
            email = email,
            password = password
        };
        string jsonData = JsonUtility.ToJson(loginData);

        // POST kérés elküldése
        using (UnityWebRequest webRequest = new UnityWebRequest(url, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
            webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            webRequest.SetRequestHeader("Content-Type", "application/json");

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                onError?.Invoke(webRequest.error);
            } else
            {
                onSuccess?.Invoke(webRequest.downloadHandler.text);
            }
        }
    }
   

    public IEnumerator SaveInventory(int inventoryId, InventoryData[] plantItems, System.Action<string> onSuccess, System.Action<string> onError)
    {
        if (inventoryId <= 0)
        {
            onError?.Invoke("Invalid inventoryId.");
            yield break;
        }

        var validItems = plantItems
            .Where(item => item.Quantity > 0)
            .ToList();

        if (validItems.Count == 0)
        {
            onError?.Invoke("No valid items to save.");
            yield break;
        }
        string url = "https://astrowheelapi.onrender.com/api/inventoryMaterials";

        foreach (var item in validItems)
        {

            item.InventoryId = inventoryId;

            // JSON adat létrehozása
            string jsonData = JsonUtility.ToJson(item);
            Debug.Log("Sending inventory data: " + jsonData);

            // POST kérés elküldése
            using (UnityWebRequest webRequest = new UnityWebRequest(url, "POST"))
            {
                byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
                webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
                webRequest.downloadHandler = new DownloadHandlerBuffer();
                webRequest.SetRequestHeader("Content-Type", "application/json");

                yield return webRequest.SendWebRequest();

                if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
                {
                    onError?.Invoke(webRequest.error);
                    Debug.LogError("Inventory save failed: " + webRequest.error);
                    Debug.LogError("Server response: " + webRequest.downloadHandler.text);
                } else
                {
                    onSuccess?.Invoke(webRequest.downloadHandler.text);
                    Debug.Log("Inventory saved successfully: " + webRequest.downloadHandler.text);
                }
            }
        }
    }
    public IEnumerator SaveCraftedInventory(int inventoryId, InventoryData[] craftedItems, System.Action<string> onSuccess, System.Action<string> onError)
    {
        if (inventoryId <= 0)
        {
            onError?.Invoke("Invalid inventoryId.");
            yield break;
        }

        var validItems = craftedItems
            .Where(item => item.Quantity > 0)
            .ToList();

        if (validItems.Count == 0)
        {
            onError?.Invoke("No valid items to save.");
            yield break;
        }

        string url = "https://astrowheelapi.onrender.com/api/inventoryMaterials"; // Vagy egy másik URL, ha a crafted inventory külön végponton mentõdik

        foreach (var item in validItems)
        {
            // Beállítjuk az inventoryId-t, ha még nincs beállítva
            item.InventoryId = inventoryId;

            // JSON adat létrehozása
            string jsonData = JsonUtility.ToJson(item);
            //Debug.Log("Sending crafted inventory data: " + jsonData);

            // POST kérés elküldése
            using (UnityWebRequest webRequest = new UnityWebRequest(url, "POST"))
            {
                byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
                webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
                webRequest.downloadHandler = new DownloadHandlerBuffer();
                webRequest.SetRequestHeader("Content-Type", "application/json");

                yield return webRequest.SendWebRequest();

                if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
                {
                    onError?.Invoke(webRequest.error);
                    Debug.LogError("Crafted inventory save failed: " + webRequest.error);
                    Debug.LogError("Server response: " + webRequest.downloadHandler.text);
                    yield break; // Ha hiba történik, kilépünk a ciklusból
                } else
                {
                    onSuccess?.Invoke(webRequest.downloadHandler.text);
                    //Debug.Log("Crafted inventory saved successfully: " + webRequest.downloadHandler.text);
                }
            }
        }
    }

    // Load regular plant inventory
    public IEnumerator LoadInventory(int inventoryId, System.Action<InventoryData[]> onSuccess, System.Action<string> onError)
    {
        if (inventoryId <= 0)
        {
            onError?.Invoke("Invalid inventoryId.");
            yield break;
        }

        string url = $"https://astrowheelapi.onrender.com/api/inventoryMaterials/byInventory/{inventoryId}";

        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError ||
                webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                onError?.Invoke(webRequest.error);
            } else
            {
                string jsonResponse = webRequest.downloadHandler.text;
                try
                {
       
                    var wrapper = JsonUtility.FromJson<InventoryMaterialResponseWrapper>("{\"inventoryMaterials\":" + jsonResponse + "}");

                    // Convert to InventoryData array
                    InventoryData[] inventoryItems = wrapper.inventoryMaterials
                        .Where(item => item.materialId <= 27)
                        .Select(item => new InventoryData
                        {
                            InventoryId = item.inventoryId,
                            MaterialId = item.materialId,
                            Quantity = item.quantity
                        })
                        .ToArray();

                    onSuccess?.Invoke(inventoryItems);
                }
                catch (Exception ex)
                {
                    onError?.Invoke($"Failed to parse inventory data: {ex.Message}");
                }
            }
        }
    }

    public IEnumerator LoadCraftedInventory(int inventoryId, System.Action<InventoryData[]> onSuccess, System.Action<string> onError)
    {
        if (inventoryId <= 0)
        {
            onError?.Invoke("Invalid inventoryId.");
            yield break;
        }

        string url = $"https://astrowheelapi.onrender.com/api/inventoryMaterials/byInventory/{inventoryId}";

        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError ||
                webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                onError?.Invoke(webRequest.error);
            } else
            {
                string jsonResponse = webRequest.downloadHandler.text;
                try
                {
                  
                    var wrapper = JsonUtility.FromJson<InventoryMaterialResponseWrapper>("{\"inventoryMaterials\":" + jsonResponse + "}");

                    // Convert to InventoryData array and filter for crafted items
                    // Assuming crafted items have materialId above a certain threshold (adjust as needed)
                    InventoryData[] inventoryItems = wrapper.inventoryMaterials
                        .Where(item => item.materialId >= 28) 
                        .Select(item => new InventoryData
                        {
                            InventoryId = item.inventoryId,
                            MaterialId = item.materialId,
                            Quantity = item.quantity
                        })
                        .ToArray();

                    onSuccess?.Invoke(inventoryItems);
                }
                catch (Exception ex)
                {
                    onError?.Invoke($"Failed to parse crafted inventory data: {ex.Message}");
                }
            }
        }
    }
    public IEnumerator UpdateTotalScore(int inventoryId, int totalScore, System.Action<string> onSuccess, System.Action<string> onError)
    {
        string url = $"https://astrowheelapi.onrender.com/api/Inventory/{inventoryId}";

        // Létrehozzuk a DTO-t
        var inventoryTotScore = new InventoryTotScore
        {
            InventoryId = inventoryId,
            TotalScore = totalScore
        };

        string jsonData = JsonUtility.ToJson(inventoryTotScore);

        // PUT kérés elküldése
        using (UnityWebRequest webRequest = new UnityWebRequest(url, "PUT"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
            webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            webRequest.SetRequestHeader("Content-Type", "application/json");

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                onError?.Invoke(webRequest.error);
                Debug.LogError("Inventory update failed: " + webRequest.error);
                Debug.LogError("Server response: " + webRequest.downloadHandler.text);
            } else
            {
                onSuccess?.Invoke(webRequest.downloadHandler.text);
                //Debug.Log("Inventory updated successfully: " + webRequest.downloadHandler.text);
            }
        }
    }
    // PUT kérés a játékos adatainak frissítéséhez
    public IEnumerator UpdatePlayer(int playerId, PlayerData updatedData, System.Action<string> onSuccess, System.Action<string> onError)
    {
        string url = $"https://astrowheelapi.onrender.com/api/players/{playerId}";

        string jsonData = JsonUtility.ToJson(updatedData);
        Debug.Log("Sending player update data: " + jsonData);

        // PUT kérés elküldése
        using (UnityWebRequest webRequest = new UnityWebRequest(url, "PUT"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
            webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            webRequest.SetRequestHeader("Content-Type", "application/json");

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                onError?.Invoke(webRequest.error);
                Debug.LogError("Player update failed: " + webRequest.error);
                Debug.LogError($"UpdatePlayer: Status code: {webRequest.responseCode}");
                Debug.LogError("Server response: " + webRequest.downloadHandler.text);
            } else
            {
                onSuccess?.Invoke(webRequest.downloadHandler.text);
                Debug.Log("Player updated successfully: " + webRequest.downloadHandler.text);
                Debug.Log($"UpdatePlayer: Server response: {webRequest.downloadHandler.text}");
            }
        }
    }

    // Új metódus az IslandId frissítéséhez
    public IEnumerator UpdatePlayerIslandId(int playerId, int newIslandId, System.Action<string> onSuccess, System.Action<string> onError)
    {
        var updatedData = new PlayerData
        {
            playerId = playerId,
            islandId = newIslandId
        };
        Debug.Log($"UpdatePlayerIslandId: Updating player {playerId} with new IslandId {newIslandId}");

        yield return UpdatePlayer(playerId, updatedData, onSuccess, onError);
    }
   
}



[System.Serializable]
public class PlayerData {
    public int playerId;
    public string playerName;
    public string userId;
    public int characterId;
    public int islandId; 
    public int inventoryId;
    public int? recipeBookId; 
    public int totalScore;
    public DateTime? lastLogin; 
    public DateTime createdAt;
    public string characterName;
    public string islandName; 
}


[System.Serializable]
public class PlayerDataWrapper {
    public PlayerData[] players;
}

[System.Serializable]
public class RegisterRequest {
    public string email;
    public string password;
    public string playerName;
}

[System.Serializable]
public class LoginRequest {
    public string email;
    public string password;
}

[System.Serializable]
public class InventoryData {
    public int InventoryId;
    public int MaterialId;
    public int Quantity;
}

[System.Serializable]
public class InventoryTotScore {
    public int InventoryId;
    public int TotalScore;

}

[System.Serializable]
public class InventoryMaterialResponse {
    public int inventoryMaterialId;
    public int inventoryId;
    public int materialId;
    public int quantity;
}

[System.Serializable]
public class InventoryMaterialResponseWrapper {
    public InventoryMaterialResponse[] inventoryMaterials;
}
