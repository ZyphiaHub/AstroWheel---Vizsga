using System.IO;
using UnityEngine;

public class PersistentDataManager : MonoBehaviour {
    void Start()
    {
        // Adatok betöltése a PlayerPrefs-bõl
        LoadPlayerData();
    }

    // Játékos adatainak betöltése a PlayerPrefs-bõl
    private void LoadPlayerData()
    {
        // Példa: Játékos nevének betöltése
        string playerName = PlayerPrefs.GetString("PlayerName", "Guest"); // Alapértelmezett érték: "Guest"
        Debug.Log("Player Name: " + playerName);
    }
}
