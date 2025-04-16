using UnityEngine;

public class PersistentDataManager : MonoBehaviour {
    void Start()
    {
        
        LoadPlayerData();
    }

  
    private void LoadPlayerData()
    {
  
        string playerName = PlayerPrefs.GetString("PlayerName", "Guest"); 
        Debug.Log("Player Name: " + playerName);
    }
}
