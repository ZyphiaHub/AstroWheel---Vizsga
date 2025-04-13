using UnityEngine;
using TMPro;
using System.Text.RegularExpressions;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/*public class RegisterManager : MonoBehaviour {
    public static RegisterManager Instance; 

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
    private int selectedCharacterIndex = 0; // Kiválasztott karakter indexe

    private void Start()
    {
        // Gomb események
        registerButton.onClick.AddListener(OnRegisterButtonClicked);
        saveCharNameButton.onClick.AddListener(OnSaveCharacterNameButtonClicked);

        cancelCharacterButton.onClick.AddListener(CloseCharacterCreationPanels);
        cancelNameButton.onClick.AddListener(CloseCharacterCreationPanels);

        // Karakterképek megjelenítése a panelen
        LoadCharacterImages();
    }

    private void Awake()
    {
        // Singleton minta implementációja
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Ne törlõdjön a scene váltáskor
        } else
        {
            Destroy(gameObject); // Ha már van példány, töröld ezt
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

        if (password.Length < 6)
        {
            errorMessageText.text = "The password should be at least 6 character long!";
            return;
        }

        // Adatok mentése a PlayerPrefs-be
        PlayerPrefs.SetString("RegisteredEmail", email);
        PlayerPrefs.SetString("RegisteredPassword", password);
        characterSelectionPanel.SetActive(true);
        selectCharacterButton.onClick.AddListener(OnSelectCharacterButtonClicked);
        PlayerPrefs.SetInt("SelectedCharacterIndex", selectedCharacterIndex);
        GameManager.Instance.SaveLastCompletedIsland(0);
        GameManager.Instance.SaveTotalScore(0);
        PlayerPrefs.Save();

        Debug.Log("Sikeres regisztráció!");
        //errorMessageText.text = "Player registered!";
    }

    private bool IsValidEmail(string email)
    {
        // Egyszerû regex az email cím ellenõrzésére
        string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        return Regex.IsMatch(email, pattern);
    }

    public void OnSelectCharacterButtonClicked()
    {
        // Panel aktiválása vagy deaktiválása
        if (characterSelectionPanel.activeSelf)
        {
            
            PlayerPrefs.SetInt("SelectedCharacterIndex", selectedCharacterIndex);
            PlayerPrefs.Save(); 
            Debug.Log("Selected character index saved: " + selectedCharacterIndex);
        }

        characterSelectionPanel.SetActive(!characterSelectionPanel.activeSelf);
        
        registerNamePanel.SetActive(true);
    }

    private void LoadCharacterImages()
    {
        // Töröljük a korábbi képeket (ha vannak)
        foreach (Transform child in characterImageContainer)
        {
            Destroy(child.gameObject);
        }

        // Karakterképek megjelenítése a panelen
        for (int i = 0; i < characterSprites.Length; i++)
        {
            GameObject imageObject = new GameObject("CharacterImage");
            imageObject.transform.SetParent(characterImageContainer, false);

            Image image = imageObject.AddComponent<Image>();
            image.sprite = characterSprites[i];

            // Gomb hozzáadása a képhez
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

        // Az összes regisztrációs adatot összegyûjtjük
        string email = PlayerPrefs.GetString("RegisteredEmail", "");
        string password = PlayerPrefs.GetString("RegisteredPassword", "");
        int characterIndex = selectedCharacterIndex;

        

        // Adatok mentése és elküldése a szerverre
        //SavePlayerData(playerName, email, password, characterIndex);

        Debug.Log("Player name saved: " + playerName);
        errorMessageText.text = "Name saved successfully!";

        // Név regisztrációs panel bezárása
        registerNamePanel.SetActive(false);
        SceneManager.LoadScene("Island_1");
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

        // Visszaléptetés a login képernyõre (ha szükséges)
        UnityEngine.SceneManagement.SceneManager.LoadScene("Login");
    }

    private void SavePlayerData(string playerName, string email, string password, int selectedCharacterIndex)
    {
        // Adatok mentése a PlayerPrefs-be
        PlayerPrefs.SetString("PlayerName", playerName);
        PlayerPrefs.SetString("RegisteredEmail", email);
        PlayerPrefs.SetString("RegisteredPassword", password);
        PlayerPrefs.SetInt("SelectedCharacterIndex", selectedCharacterIndex);
        PlayerPrefs.Save();

        // Adatok elküldése a szerverre
       /* StartCoroutine(APIClient.Instance.PostData(playerName, email, password, selectedCharacterIndex));*/
   /* }*/
/*}*/