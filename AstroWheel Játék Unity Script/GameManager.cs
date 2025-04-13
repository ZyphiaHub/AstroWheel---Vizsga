using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    private IGameState currentState;

    public static GameManager Instance { get; private set; }

    //public const string LastCompletedIslandKey = "LastCompletedIsland";
    public const string TotalScoreKey = "TotalScore";
    //public const int PlayerIdKey = "PlayerId";
    public const string CharacterIdKey = "CharacterId";

    private int islandIndex; // Új változó az aktuális sziget indexének tárolására

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            //Debug.Log("GameManager inicializálva.");
        } else
        {
            Debug.LogWarning("Második GameManager példány törölve!");
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        //Debug.Log("Beléptél a Login Menübe");
        SceneManager.LoadScene("Login");
    }

    private void Update()
    {
        if (currentState != null)
        {
            currentState.UpdateState();
        }
    }

    public void ChangeState(IGameState newState)
    {
        if (currentState != null)
        {
            currentState.ExitState();
        }

        currentState = newState;
        currentState.EnterState();
    }

    public void SaveLastCompletedIsland(int islandIndex)
    {
        PlayerPrefs.SetInt("LastCompletedIsland", islandIndex);
        PlayerPrefs.Save();
    }

    public int LoadLastCompletedIsland()
    {
        int proba = PlayerPrefs.GetInt("LastCompletedIsland", 0);
        return PlayerPrefs.GetInt("LastCompletedIsland", 0);
    }

    // Ellenõrzés: Egy adott sziget teljesítve van-e
    public bool IsIslandCompleted(int islandIndex)
    {
        return islandIndex <= LoadLastCompletedIsland();
    }

    public void SaveTotalScore(int totalScore)
    {
        PlayerPrefs.SetInt("TotalScore", totalScore);
        PlayerPrefs.Save();
    }

    public int LoadTotalScore()
    {
        return PlayerPrefs.GetInt("TotalScore", 0);
    }
    public bool IsInternetAvailable()
    {
        return Application.internetReachability != NetworkReachability.NotReachable;
    }

    /* public void SavePlayerId(int playerId)
     {
         PlayerPrefs.SetInt(PlayerIdKey, playerId);
         PlayerPrefs.Save(); 
     }*/
    public void SavePlayerId(int playerId)
    {
        PlayerPrefs.SetInt("PlayerId", playerId);
        PlayerPrefs.Save();
    }

    public int LoadPlayerId()
    {
        if (PlayerPrefs.HasKey("PlayerId"))
        {
            return PlayerPrefs.GetInt("PlayerId");
        } else
        {
            Debug.LogError("PlayerId not found in PlayerPrefs!");
            return 0; // Vagy dobj egy kivételt, ha szükséges
        }
    }
    public void SaveCharId(int charId)
    {
        PlayerPrefs.SetInt("CharacterIndex", charId);
        PlayerPrefs.Save();
    }
    public int LoadCharId()
    {
        if (PlayerPrefs.HasKey("CharacterId"))
        {
            return PlayerPrefs.GetInt("CharacterId");
        } else
        {
            Debug.LogError("PlayerId not found in PlayerPrefs!");
            return 0; // Vagy dobj egy kivételt, ha szükséges
        }
    }

    public void SetPuzzleSolved(bool isSolved)
    {
        PlayerPrefs.SetInt($"Island_{islandIndex}_Solved", isSolved ? 1 : 0);
        PlayerPrefs.Save();
    }

    public bool IsPuzzleSolved(int islandIndex)
    {
        return PlayerPrefs.GetInt($"Island_{islandIndex}_Solved", 0) == 1;
    }

    public int GetCurrentIslandIndex()
    {
        return islandIndex; }

    public void SetCurrentIslandIndex(int index)
    {
        islandIndex = index;
    }

    public void SaveRegisteredEmail(string email)
    {
        PlayerPrefs.SetString("RegisteredEmail", email);
        PlayerPrefs.Save();
    }
    public string LoadRegisteredEmail(){
        return PlayerPrefs.GetString("RegisteredEmail", "");
    }

    public void SaveRegisteredPassword(string password)
    {
        PlayerPrefs.SetString("RegisteredPassword", password);
        PlayerPrefs.Save();
    }

    public string LoadRegisteredPassword()
    {
        return PlayerPrefs.GetString("RegisteredPassword", "");
    }

    public void SaveSelectedCharacterIndex(int selectedCharacterIndex)
    {
        PlayerPrefs.SetInt("SelectedCharacterIndex", selectedCharacterIndex);
        PlayerPrefs.Save();
    }

}
