using System.IO;
using UnityEngine;

public class PersistentDataManager : MonoBehaviour {
    void Start()
    {
        // Adatok bet�lt�se a PlayerPrefs-b�l
        LoadPlayerData();
    }

    // J�t�kos adatainak bet�lt�se a PlayerPrefs-b�l
    private void LoadPlayerData()
    {
        // P�lda: J�t�kos nev�nek bet�lt�se
        string playerName = PlayerPrefs.GetString("PlayerName", "Guest"); // Alap�rtelmezett �rt�k: "Guest"
        Debug.Log("Player Name: " + playerName);
    }
}
