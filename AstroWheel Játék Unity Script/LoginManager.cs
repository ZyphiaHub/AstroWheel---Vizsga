using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Text.RegularExpressions;
using System.Collections;
using UnityEngine.Networking;
using static RegisterManager;
using System;
using static LocalDatabaseManager;
using System.Collections.Generic;
using static LoginManager;

public class LoginManager : MonoBehaviour {
    public TMP_InputField emailInputField;
    public TMP_InputField passwordInputField;
    public Button loginButton;
    public Button registerButton;
    public Button quitButton;
    public TMP_Text errorMessageText;

    private bool isFetchPlayerDataCompleted = false;
    private Coroutine loginRoutine;

    private void Start()
    {

        loginButton.onClick.AddListener(OnLoginButtonClicked);
        quitButton.onClick.AddListener(OnQuitToDesktopClicked);

    }
    /*private void OnEnable()
    {
        ResetLoginUI();
        loginButton.onClick.RemoveAllListeners(); // Régi események törlése
        loginButton.onClick.AddListener(OnLoginButtonClicked);
        registerButton.onClick.RemoveAllListeners();
        //registerButton.onClick.AddListener(OnRegisterButtonClicked); // HIÁNYZIK A JELENLEGI KÓDBÓL!
    }*/
    public void OnLoginButtonClicked()
    {
        ClearSessionData();
        string email = emailInputField.text;
        string password = passwordInputField.text;
        Debug.Log("Login gomb megnyomva!");
        if (!IsValidEmail(email))
        {
            errorMessageText.text = "Invalid email address!";
            return;
        }


        // Bejelentkezési kérés indítása
        if (loginRoutine != null)
        {
            StopCoroutine(loginRoutine);
        }
        loginRoutine = StartCoroutine(LoginAndFetchData(emailInputField.text, passwordInputField.text));
    }



    private void ClearSessionData()
    {
        PlayerPrefs.DeleteKey("AuthToken");
        PlayerPrefs.DeleteKey("PlayerUsername");
        PlayerPrefs.DeleteKey("InventoryID");
        PlayerPrefs.Save();

        // Reset game state
        if (GameManager.Instance != null)
        {
            GameManager.Instance.SavePlayerId(0);
            GameManager.Instance.SaveLastCompletedIsland(0);
            GameManager.Instance.SaveTotalScore(0);
        }
    }

    public void ResetLoginUI()
    {
        emailInputField.text = "";
        passwordInputField.text = "";
        errorMessageText.text = "";

        // Fontos: engedélyezzük a gombot
        loginButton.interactable = true;
    }
    private bool IsValidEmail(string email)
    {
        // Egyszerû regex az email cím ellenõrzésére
        string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        return Regex.IsMatch(email, pattern);
    }

    private void OnQuitToDesktopClicked()
    {
        // Adjunk hozzá extra ellenõrzéseket
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif

        // Erõltesd a kilépést
        System.Diagnostics.Process.GetCurrentProcess().Kill();
    }

    
    public IEnumerator Login(string email, string password, System.Action<string> onSuccess, System.Action<string> onError)
    {
        string url = "https://astrowheelapi.onrender.com/api/auth/login"; 

        // Bejelentkezési adatok összeállítása
        var loginData = new LoginData
        {
            Email = email,
            Password = password
        };
        PlayerPrefs.SetString("LoginEmail", email);
        PlayerPrefs.Save();

        string jsonData = JsonUtility.ToJson(loginData);
        Debug.Log("Sending login data: " + jsonData);
        errorMessageText.text = "Connecting, please wait...";
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
                Debug.LogError("Login failed: " + webRequest.error);
                Debug.LogError("Server response: " + webRequest.downloadHandler.text);

                
                StartCoroutine(FetchPlayerSQLite(email, password));
                int lastCompletedIsland = GameManager.Instance.LoadLastCompletedIsland();
                //Debug.Log("lite  last island: " + lastCompletedIsland);

                if (lastCompletedIsland >= 1)
                { // Ha az elsõ sziget teljesítve van
                    SceneManager.LoadScene("Main_Menu"); // Fõmenü betöltése
                } else
                {
                    SceneManager.LoadScene("Island_1"); // Elsõ sziget betöltése
                }

                int inventoryId = PlayerPrefs.GetInt("InventoryID", 0);
                Debug.Log("inventroy kívül " + inventoryId);
                List<MaterialDataFetchLite> loadedPlants = LocalDatabaseManager.Instance.LoadInventoryData(inventoryId);
                InventoryManager.Instance.LoadFetchedLiteToPlantDatabase(loadedPlants);

                List<MaterialDataFetchLite> loadedItems = LocalDatabaseManager.Instance.LoadCraftedInventoryData(inventoryId);
                InventoryManager.Instance.LoadFetchedLiteToItemDatabase(loadedItems);

            } else
            {
                onSuccess?.Invoke(webRequest.downloadHandler.text);
                Debug.Log("Login successful: " + webRequest.downloadHandler.text);
                var response = JsonUtility.FromJson<LoginResponse>(webRequest.downloadHandler.text);
                PlayerPrefs.SetString("Password", password);
                PlayerPrefs.SetString("AuthToken", response.token);
                PlayerPrefs.Save();


                // Bejelentkezési kérés indítása
                yield return StartCoroutine(FetchPlayerData());


            }
        }
    }
    private IEnumerator FetchPlayerSQLite(string email, string password)
    {
        Debug.Log("Fetching player data from SQLite...");
        Debug.Log("email és pass liteban" + email + password);
        PlayerTbl playerData = LocalDatabaseManager.Instance.LoadPlayerDataByEmailAndPassword(email, password);

        if (playerData != null)
        {
            Debug.Log($"Local Player Data: ID = {playerData.playerId}, Username = {playerData.playerName}, " +
                $"Email = {playerData.userId}, CharacterId = {playerData.characterId}," +
                $"CharacterIndex = {playerData.characterIndex}, Score = {playerData.totalScore}, " +
                $"InventoryID = {playerData.inventoryId}, LastCompletedIsland = {playerData.islandId}");

            // Adatok betöltése a GameManager-be
            GameManager.Instance.SavePlayerId(playerData.playerId);
            GameManager.Instance.SaveLastCompletedIsland(playerData.islandId);

            PlayerPrefs.SetString("PlayerUsername", playerData.playerName ?? string.Empty);
            PlayerPrefs.Save();
            PlayerPrefs.SetString("PlayerEmail", playerData.userId ?? string.Empty);
            PlayerPrefs.Save();
            GameManager.Instance.SaveTotalScore(playerData.totalScore);
            Debug.Log($"InventoryID before save: {playerData.inventoryId} (Type: {playerData.inventoryId.GetType()})");
            PlayerPrefs.SetInt("InventoryID", playerData.inventoryId);
            PlayerPrefs.Save();
            Debug.Log($"InventoryID after save: {PlayerPrefs.GetInt("InventoryID", -1)}");

            Debug.Log("Local player data loaded successfully.");
        } else
        {
            Debug.LogError("No player data found in SQLite for the given email and password.");
        }

        //isFetchPlayerDataCompleted = true;
        yield break;
    }

    private IEnumerator LoginAndFetchData(string email, string password)
    {
        yield return StartCoroutine(Login(email, password,
            response =>
            {
                Debug.Log("Login successful: " + response);
            },
            error =>
            {
                errorMessageText.text = "Invalid email or password!";
            }
        ));

        // Várjuk meg, amíg a FetchPlayerData befejezõdik
        yield return new WaitUntil(() => isFetchPlayerDataCompleted);

        int lastCompletedIsland = GameManager.Instance.LoadLastCompletedIsland();
        Debug.Log("corutin last island: " + lastCompletedIsland);

        if (lastCompletedIsland >= 1)
        { // Ha az elsõ sziget teljesítve van
            SceneManager.LoadScene("Main_Menu"); // Fõmenü betöltése
        } else
        {
            SceneManager.LoadScene("Island_1"); // Elsõ sziget betöltése
        }
    }
    // Játékos adatainak lekérése
    private IEnumerator FetchPlayerData()
    {
        string url = "https://astrowheelapi.onrender.com/api/players/me";
        string authToken = PlayerPrefs.GetString("AuthToken", ""); 

        if (string.IsNullOrEmpty(authToken))
        {
            Debug.LogError("No authentication token found.");
            isFetchPlayerDataCompleted = true;
            yield break;
        }

        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            webRequest.SetRequestHeader("Authorization", "Bearer " + authToken); 
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || 
                webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error fetching player data: " + webRequest.error);
                Debug.LogError("Server response: " + webRequest.downloadHandler.text);
                isFetchPlayerDataCompleted = true;
                yield break;
            } else
            {
                Debug.Log("Player data fetched successfully: " + webRequest.downloadHandler.text);
                PlayerDataFetch playerData = null;
                try
                {
                    playerData = JsonUtility.FromJson<PlayerDataFetch>(webRequest.downloadHandler.text);
                }
                catch (System.Exception ex)
                {
                    Debug.LogError("Error deserializing player data: " + ex.Message);
                    isFetchPlayerDataCompleted = true;
                    yield break;
                }
                if (playerData == null)
                {
                    Debug.LogError("Failed to deserialize player data.");
                    isFetchPlayerDataCompleted = true;
                    yield break;
                }


                // MySQL szerveren lévõ lastLogin
                string serverLastLogin = playerData.lastLogin;
                Debug.Log("serverlastloginkérés" + serverLastLogin);

                // Helyi lastLogin lekérése
                string localLastLogin = LocalDatabaseManager.Instance.GetLastLogin(playerData.playerId);
                Debug.Log("locallastloginkérés" + localLastLogin);

                // Adatok mentése PlayerPrefs-be
                        
                if (string.Compare(serverLastLogin, localLastLogin, StringComparison.Ordinal) > 0) {
                   GameManager.Instance.SavePlayerId(playerData.playerId);
                   GameManager.Instance.SaveLastCompletedIsland(playerData.islandId);
                            
                   PlayerPrefs.SetString("PlayerUsername", playerData.playerName ?? string.Empty);
                   PlayerPrefs.Save();
                   GameManager.Instance.SaveTotalScore(playerData.totalScore);
                   //GameManager.Instance.SaveCharId(playerData.characterId-6);
                   GameManager.Instance.SaveCharId(playerData.characterIndex);
                    PlayerPrefs.SetInt("InventoryID", playerData.inventoryId);
                   PlayerPrefs.Save();

                   Debug.Log("Player data saved to PlayerPrefs.");

                   playerData.playerPassword = PlayerPrefs.GetString("Password", "");
                   playerData.playerEmail = PlayerPrefs.GetString("LoginEmail", "");

                   // Adatok mentése SQLite adatbázisba
                   LocalDatabaseManager.Instance.SavePlayerData(
                      playerData.playerId,
                      playerData.playerName ?? string.Empty,
                    playerData.userId ?? string.Empty,
                    playerData.playerEmail,
                    playerData.playerPassword,
                    playerData.characterId,
                    playerData.characterIndex,
                    playerData.totalScore,
                     playerData.inventoryId,
                     playerData.islandId,
                    playerData.lastLogin,
                     playerData.createdAt       );

                    if (playerData.materials != null && playerData.materials.Count > 0)
                    {
                        InventoryManager.Instance.LoadFetchedMatToInventory(playerData.materials.ToArray());
                        InventoryManager.Instance.SaveInventory();
                        InventoryManager.Instance.SaveCraftedInventory();

                    }
                    } else
                    {
                        Debug.LogError("Failed to deserialize player data.");
                    }
                

            }
        }
        // Jelzés, hogy a FetchPlayerData befejezõdött
        isFetchPlayerDataCompleted = true;
        
    }


    [System.Serializable]
    public class LoginData
    {
        public string Email;
        public string Password;
    }

    [System.Serializable]
    public class PlayerDataFetch {
        public int playerId; // A szerver "playerId" mezõje
        public string playerName; 
        public string userId; // A szerver "userId" mezõje
        public string playerPassword;
        public string playerEmail;
        public int characterId;
        public int characterIndex;
        public int totalScore; 
        public int inventoryId; 
        public int islandId; // A szerver "islandId" mezõje (nullable)
        public string characterName; 
        public string lastLogin; 
        public string createdAt; 
        public List<MaterialDataFetch> materials;
    }

    [System.Serializable]
    public class MaterialDataFetch {
        public int materialId;
        public string witchName;
        public string englishName;
        public string latinName;
        public int quantity;
    }
}