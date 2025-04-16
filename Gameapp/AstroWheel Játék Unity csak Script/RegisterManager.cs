using UnityEngine;
using TMPro;
using System.Text.RegularExpressions;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.Networking;
using System.Linq;




public class RegisterManager : MonoBehaviour {
    public static RegisterManager Instance; 
    public LoginManager loginManager;
    public InventoryManager inventoryManager;

    public TMP_InputField emailInputField;
    public TMP_InputField passwordInputField;
    public TMP_InputField nameInputField;
    public Button registerButton;
    public Button selectCharacterButton;
    public Button saveCharNameButton;
    public Button cancelCharacterButton;
    public Button cancelNameButton;
    public TMP_Text errorMessageText;
    public GameObject characterSelectionPanel;
    public GameObject registerNamePanel;
    public Transform characterImageContainer;

    [SerializeField] private Sprite[] characterSprites;
    public Sprite[] CharacterSprites => characterSprites;
    private int selectedCharacterIndex = 0; 

    private void Start()
    {
        registerButton.onClick.AddListener(OnRegisterButtonClicked);
        saveCharNameButton.onClick.AddListener(OnSaveCharacterNameButtonClicked);

        cancelCharacterButton.onClick.AddListener(CloseCharacterCreationPanels);
        cancelNameButton.onClick.AddListener(CloseCharacterCreationPanels);

        LoadCharacterImages();
       
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        } else
        {
            Destroy(gameObject);
        }
    }

    public void OnRegisterButtonClicked()
    {
        string email = emailInputField.text;
        string password = passwordInputField.text;

        if (!IsValidEmail(email))
        {
            errorMessageText.text = "Invalid email address!";
            return;
        }

        if (!IsPasswordValid(password, out string passwordErrorMessage))
        {
            errorMessageText.text = passwordErrorMessage;
            return;
        }

        GameManager.Instance.SaveRegisteredEmail(email);
        GameManager.Instance.SaveRegisteredPassword(password);
        characterSelectionPanel.SetActive(true);
        selectCharacterButton.onClick.AddListener(OnSelectCharacterButtonClicked);
        GameManager.Instance.SaveLastCompletedIsland(0);
        GameManager.Instance.SaveTotalScore(0);
    }

    private bool IsValidEmail(string email)
    {
        string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        return Regex.IsMatch(email, pattern);
    }

    public void OnSelectCharacterButtonClicked()
    {
        
        if (characterSelectionPanel.activeSelf)
        {
            GameManager.Instance.SaveSelectedCharacterIndex(selectedCharacterIndex);
            Debug.Log("Selected character index saved: " + selectedCharacterIndex);
        }

        characterSelectionPanel.SetActive(!characterSelectionPanel.activeSelf);
        registerNamePanel.SetActive(true);
    }

    private void LoadCharacterImages()
    {
        //karakterképek tisztítása és betöltése
        foreach (Transform child in characterImageContainer)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < characterSprites.Length; i++)
        {
            GameObject imageObject = new GameObject("CharacterImage");
            imageObject.transform.SetParent(characterImageContainer, false);

            Image image = imageObject.AddComponent<Image>();
            image.sprite = characterSprites[i];

           
            Button button = imageObject.AddComponent<Button>();
            int index = i; 
            button.onClick.AddListener(() => OnCharacterSelected(index));
        }
        //Debug.Log("Loaded " + characterSprites.Length + " character images.");
    }

    private void OnCharacterSelected(int index)
    {
        selectedCharacterIndex = index;
        Debug.Log("Selected character index: " + index);
    }

    public void OnSaveCharacterNameButtonClicked()
    {
        string playerName = nameInputField.text;

        if (string.IsNullOrEmpty(playerName))
        {
            errorMessageText.text = "Please enter a valid name!";
            return;
        }

       
        string email = GameManager.Instance.LoadRegisteredEmail();
        string password = GameManager.Instance.LoadRegisteredPassword();
        int characterIndex = PlayerPrefs.GetInt("SelectedCharacterIndex", 0);

        StartCoroutine(RegisterPlayer(playerName, email, password, characterIndex, 
            response =>
            {
                //Debug.Log("Registration successful: " + response);
                ClearPlayerPrefsForNewPlayer();
                InitializeNewPlayerInventory(inventoryManager);

                if (loginManager != null)
                {
                    
                    StartCoroutine(loginManager.Login(email, password,
                        response => {
                            //Debug.Log("Login successful: " + response);
                            //StartCoroutine(FetchPlayerData());
                        },
                        error => {
                            Debug.Log("Login failed: " + error);
                        }
                    ));
                }
                SceneManager.LoadScene("Island_1"); 
                
            },
            error =>
            {
                errorMessageText.text = "Invalid email or password!";
            }));
       
    }

    private IEnumerator RegisterPlayer(string playerName, string email, string password, int characterIndex, System.Action<string> onSuccess, System.Action<string>onError)
    {
        string url = "https://astrowheelapi.onrender.com/api/auth/register"; 

        var registrationData = new RegistrationData
        {
            UserName = email, 
            Email = email,
            Password = password,
            PlayerName = playerName,
            CharacterIndex = characterIndex
        };
        string jsonData = JsonUtility.ToJson(registrationData);

        Debug.Log("Sending registration data: " + jsonData); 

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
                Debug.LogError("Registration failed: " + webRequest.error);
                Debug.LogError("Server response: " + webRequest.downloadHandler.text); 
            } else
            {
                onSuccess?.Invoke(webRequest.downloadHandler.text);
                Debug.Log("Registration successful: " + webRequest.downloadHandler.text);
                

            }
        }
    }


    public void CloseCharacterCreationPanels()
    {
        Debug.Log("Character creation cancelled.");
        characterSelectionPanel.SetActive(false);
        registerNamePanel.SetActive(false);

        // Ideiglenesen beírt adatok törlése, hogy ne léptessen be a fõmenübe!
        PlayerPrefs.DeleteKey("RegisteredEmail");
        PlayerPrefs.DeleteKey("RegisteredPassword");
        PlayerPrefs.DeleteKey("PlayerName");
        PlayerPrefs.DeleteKey("SelectedCharacterIndex");
        PlayerPrefs.Save();


        SceneManager.LoadScene("Login");
    }

    private bool IsPasswordValid(string password, out string errorMessage)
    {
        errorMessage = "";

        if (password.Length < 6)
        {
            errorMessage = "The password should be at least 6 characters long!";
            return false;
        }

        if (!password.Any(char.IsDigit))
        {
            errorMessage = "The password must contain at least one digit!";
            return false;
        }

        if (!password.Any(char.IsLower))
        {
            errorMessage = "The password must contain at least one lowercase letter!";
            return false;
        }

        if (!password.Any(char.IsUpper))
        {
            errorMessage = "The password must contain at least one uppercase letter!";
            return false;
        }

        if (!password.Any(ch => !char.IsLetterOrDigit(ch)))
        {
            errorMessage = "The password must contain at least one special character!";
            return false;
        }

        if (password.Distinct().Count() < 1)
        {
            errorMessage = "The password must use at least 1 different characters!";
            return false;
        }

        return true;
    }


    private void ClearPlayerPrefsForNewPlayer()
    {

        foreach (var item in InventoryManager.Instance.plantDatabase.items)
        {
            string key = "plant_" + item.englishName.Replace(" ", "");
            PlayerPrefs.DeleteKey(key);
        }

        foreach (var item in InventoryManager.Instance.itemDatabase.items)
        {
            string key = "crafted_" + item.itemName.Replace(" ", "");
            PlayerPrefs.DeleteKey(key);
        }

        PlayerPrefs.Save();
        Debug.Log("PlayerPrefs cleared for new player.");
    }

    private void InitializeNewPlayerInventory(InventoryManager inventoryManager)
    {
        // Inicializáljuk az új inventorykat
        InventoryManager.Instance.inventory = new Inventory();
        InventoryManager.Instance.craftedInventory = new CraftedInventory();

        foreach (var item in InventoryManager.Instance.plantDatabase.items)
        {
            InventoryManager.Instance.inventory.AddItem(item, 0);
        }

        foreach (var item in InventoryManager.Instance.itemDatabase.items)
        {
            InventoryManager.Instance.craftedInventory.AddItem(item, 0);
        }


        InventoryManager.Instance.SaveInventory();
        InventoryManager.Instance.SaveCraftedInventory();
        Debug.Log("New player inventory initialized.");
        
    }



    [System.Serializable]
    public class PlayerData {
        public int PlayerId;
        public string PlayerName;
        public string Email;
        public int CharacterId;
        public int IslandId;
        public int TotalScore;
        public string LastLogin;
        public string CreatedAt;
        public bool IsActive;
    }

[System.Serializable]
    public class RegistrationData {
        public string UserName;
        public string Email;
        public string Password;
        public string PlayerName;
        public int CharacterIndex;
    }

    [System.Serializable]
    public class LoginData {
        public string Email;
        public string Password;
    }

    [System.Serializable]
    public class LoginResponse {
        public string token;
    }
}