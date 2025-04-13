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
    private int selectedCharacterIndex = 0; // Kiv�lasztott karakter indexe

    private void Start()
    {
        // Gomb esem�nyek
        registerButton.onClick.AddListener(OnRegisterButtonClicked);
        saveCharNameButton.onClick.AddListener(OnSaveCharacterNameButtonClicked);

        cancelCharacterButton.onClick.AddListener(CloseCharacterCreationPanels);
        cancelNameButton.onClick.AddListener(CloseCharacterCreationPanels);

        // Karakterk�pek megjelen�t�se a panelen
        LoadCharacterImages();
    }

    private void Awake()
    {
        // Singleton minta implement�ci�ja
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Ne t�rl�dj�n a scene v�lt�skor
        } else
        {
            Destroy(gameObject); // Ha m�r van p�ld�ny, t�r�ld ezt
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

        // Adatok ment�se a PlayerPrefs-be
        PlayerPrefs.SetString("RegisteredEmail", email);
        PlayerPrefs.SetString("RegisteredPassword", password);
        characterSelectionPanel.SetActive(true);
        selectCharacterButton.onClick.AddListener(OnSelectCharacterButtonClicked);
        PlayerPrefs.SetInt("SelectedCharacterIndex", selectedCharacterIndex);
        GameManager.Instance.SaveLastCompletedIsland(0);
        GameManager.Instance.SaveTotalScore(0);
        PlayerPrefs.Save();

        Debug.Log("Sikeres regisztr�ci�!");
        //errorMessageText.text = "Player registered!";
    }

    private bool IsValidEmail(string email)
    {
        // Egyszer� regex az email c�m ellen�rz�s�re
        string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        return Regex.IsMatch(email, pattern);
    }

    public void OnSelectCharacterButtonClicked()
    {
        // Panel aktiv�l�sa vagy deaktiv�l�sa
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
        // T�r�lj�k a kor�bbi k�peket (ha vannak)
        foreach (Transform child in characterImageContainer)
        {
            Destroy(child.gameObject);
        }

        // Karakterk�pek megjelen�t�se a panelen
        for (int i = 0; i < characterSprites.Length; i++)
        {
            GameObject imageObject = new GameObject("CharacterImage");
            imageObject.transform.SetParent(characterImageContainer, false);

            Image image = imageObject.AddComponent<Image>();
            image.sprite = characterSprites[i];

            // Gomb hozz�ad�sa a k�phez
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

        // Az �sszes regisztr�ci�s adatot �sszegy�jtj�k
        string email = PlayerPrefs.GetString("RegisteredEmail", "");
        string password = PlayerPrefs.GetString("RegisteredPassword", "");
        int characterIndex = selectedCharacterIndex;

        

        // Adatok ment�se �s elk�ld�se a szerverre
        //SavePlayerData(playerName, email, password, characterIndex);

        Debug.Log("Player name saved: " + playerName);
        errorMessageText.text = "Name saved successfully!";

        // N�v regisztr�ci�s panel bez�r�sa
        registerNamePanel.SetActive(false);
        SceneManager.LoadScene("Island_1");
    }

    public void CloseCharacterCreationPanels()
    {
        Debug.Log("Character creation cancelled.");
        characterSelectionPanel.SetActive(false);
        registerNamePanel.SetActive(false);

        // Ideiglenesen be�rt adatok t�rl�se, hogy ne l�ptessen be a f�men�be!
        PlayerPrefs.DeleteKey("RegisteredEmail");
        PlayerPrefs.DeleteKey("RegisteredPassword");
        PlayerPrefs.DeleteKey("PlayerName");
        PlayerPrefs.DeleteKey("SelectedCharacterIndex");
        PlayerPrefs.Save();

        // Visszal�ptet�s a login k�perny�re (ha sz�ks�ges)
        UnityEngine.SceneManagement.SceneManager.LoadScene("Login");
    }

    private void SavePlayerData(string playerName, string email, string password, int selectedCharacterIndex)
    {
        // Adatok ment�se a PlayerPrefs-be
        PlayerPrefs.SetString("PlayerName", playerName);
        PlayerPrefs.SetString("RegisteredEmail", email);
        PlayerPrefs.SetString("RegisteredPassword", password);
        PlayerPrefs.SetInt("SelectedCharacterIndex", selectedCharacterIndex);
        PlayerPrefs.Save();

        // Adatok elk�ld�se a szerverre
       /* StartCoroutine(APIClient.Instance.PostData(playerName, email, password, selectedCharacterIndex));*/
   /* }*/
/*}*/