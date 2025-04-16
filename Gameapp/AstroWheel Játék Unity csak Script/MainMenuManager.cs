using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuManager : MonoBehaviour {
    public Button[] islandButtons;
    [Header("Volume Controls")]
    public Button volumeUpButton;
    public Button volumeDownButton;
    public Button logoutButton;
    public Button quitButton;
    public TMP_Text volumeText;

    public TMP_Text playerNameText; 
    public TMP_Text playerScoreText; 
    public TMP_Text lastCompletedIslandText; 

    private void Start()
    {
        Debug.Log("Bel�pt�l a F�men�be!");
        
        volumeUpButton.onClick.AddListener(IncreaseVolume);
        volumeDownButton.onClick.AddListener(DecreaseVolume);
        //logoutButton.onClick.AddListener(Logout);
        quitButton.onClick.AddListener(OnQuitToDesktopClicked);
        UpdateVolumeUI();
        LoadAndDisplayPlayerData();
        
        for (int i = 0; i < islandButtons.Length; i++)
        {
            int islandIndex = i + 1;
            if (GameManager.Instance.IsIslandCompleted(islandIndex-1))
            {
                islandButtons[i].interactable = true; 
                //Debug.Log($"gomb {i+1} �l");
            } else
            {
                islandButtons[i].interactable = false; 
                //Debug.Log($"gomb {i+1} letiltva");
            }

            //gomb lenyom�s vizsg�lat
            int sceneIndex = islandIndex; 
            islandButtons[i].onClick.AddListener(() => LoadIslandScene(sceneIndex));
            
        }
    }
    private void IncreaseVolume()
    {
        AudioManager.Instance.IncreaseVolume();
        UpdateVolumeUI();
    }

    private void DecreaseVolume()
    {
        AudioManager.Instance.DecreaseVolume();
        UpdateVolumeUI();
    }

    private void UpdateVolumeUI()
    {
        if (volumeText != null)
        {
            float volume = AudioManager.Instance.GetCurrentVolume();
            volumeText.text = $"Volume: {Mathf.RoundToInt(volume * 100)}%";
        }
    }

    public void LoadIslandScene(int islandIndex)
    {
        SceneManager.LoadScene($"Island_{islandIndex}"); }

        private void LoadAndDisplayPlayerData()
    {

        string playerName = PlayerPrefs.GetString("PlayerUsername", "Guest");
        playerNameText.text = "Witch: \n" + playerName;

        int playerScore = GameManager.Instance.LoadTotalScore();
        playerScoreText.text = "ChronoPoints: \n" + playerScore.ToString();

        int lastCompletedIsland = PlayerPrefs.GetInt("LastCompletedIsland", 0);
        lastCompletedIslandText.text = "Last Completed \nIsland: " + lastCompletedIsland;
    }

    public void Logout()
    {
        // T�R�LJ�K MINDEN ADATOT
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();


        if (GameManager.Instance != null)
        {
            GameManager.Instance.SavePlayerId(0);
            GameManager.Instance.SaveLastCompletedIsland(0);
            GameManager.Instance.SaveTotalScore(0);
        }

        SceneManager.LoadScene("Login"); 
    }
    private void OnQuitToDesktopClicked()
    {

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif

        // Er�ltesd a kil�p�st
        System.Diagnostics.Process.GetCurrentProcess().Kill();
    }


}