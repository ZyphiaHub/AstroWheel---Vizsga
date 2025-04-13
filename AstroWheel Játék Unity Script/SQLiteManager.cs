using UnityEngine;
using SQLite4Unity3d; 
using System.IO;
using System.Linq;
using System.Collections.Generic;
using static UnityEngine.EventSystems.EventTrigger;
using System;


public class LocalDatabaseManager : MonoBehaviour {
    public static LocalDatabaseManager Instance { get; private set; }
    public Inventory inventory { get; set; }
    public CraftedInventory craftedInventory { get; set; }
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

    private SQLiteConnection connection;

    void Start()
    {
        InitializeDatabase();
    }

    void InitializeDatabase()
    {
        string sourcePath = Path.Combine(Application.streamingAssetsPath, "game_data.db");
        string destinationPath = Path.Combine(Application.persistentDataPath, "game_data.db");

        // Ellenõrizzük, hogy a forrásfájl létezik-e
        if (!File.Exists(sourcePath))
        {
            Debug.LogError("Source database file not found in StreamingAssets.");
            return;
        }

        // Ha a célfájl még nem létezik, másold át a fájlt
        if (!File.Exists(destinationPath))
        {
            try
            {
                File.Copy(sourcePath, destinationPath);
                Debug.Log("Database copied from StreamingAssets to persistentDataPath.");
            }
            catch (System.Exception ex)
            {
                Debug.LogError("Failed to copy database: " + ex.Message);
                return;
            }
        }

        // Adatbázis kapcsolat létrehozása
        try
        {
            connection = new SQLiteConnection(destinationPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);

            // Táblák létrehozása, ha még nem léteznek
            connection.CreateTable<PlayerTbl>();
            connection.CreateTable<CharacterTbl>();
            connection.CreateTable<InventoryTbl>();
            connection.CreateTable<IslandTbl>();
            connection.CreateTable<MaterialTbl>();
            connection.CreateTable<CraftedInventoryTbl>();

           // Debug.Log("Database initialized successfully.");
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error initializing database: " + ex.Message);
        }
    }

    public void SavePlayerData(int playerId, string playerName, string userId,string playerEmail, string playerPassword, int characterId,
        int characterIndex, int totalScore, int inventoryId, int lastCompletedIsland, string lastLogin, string createdAt)
    {
        if (connection == null)
        {
            Debug.LogError("Database connection is null.");
            return;
        }

        var playerData = new PlayerTbl
        {
            playerId = playerId,
            playerName = playerName ?? string.Empty,
            userId = userId ?? string.Empty,
            playerEmail = playerEmail ?? string.Empty,
            playerPassword = playerPassword ?? string.Empty,
            characterId = characterId,
            characterIndex = characterIndex,
            totalScore = totalScore,
            inventoryId = inventoryId,
            islandId = lastCompletedIsland,
            isActive = 1,
            lastLogin = lastLogin,
            createdAt = createdAt
        };

        try
        {
            connection.InsertOrReplace(playerData);
            Debug.Log("Player data saved to SQLite database.");
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error saving player data: " + ex.Message);
        }
    }

    public string GetLastLogin(int playerId)
    {
        if (connection == null)
        {
            Debug.LogError("Database connection is null.");
            return string.Empty;
        }

        try
        {
            // Lekérdezzük a játékos utolsó bejelentkezési idejét a playerId alapján
            var playerData = connection.Table<PlayerTbl>()
                                      .FirstOrDefault(p => p.playerId == playerId);

            if (playerData != null)
            {
                Debug.Log($"Last login for player {playerId}: {playerData.lastLogin}");
                return playerData.lastLogin;
            } else
            {
                Debug.LogWarning($"No player data found for playerId: {playerId}");
                return string.Empty;
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Error retrieving last login for player {playerId}: {ex.Message}");
            return string.Empty;
        }
    }


    public PlayerTbl LoadPlayerDataByEmailAndPassword(string email, string password)
    {
        if (connection == null)
        {
            Debug.LogError("Database connection is null in loadplayerbyemailandpass.");
            return null;
        }

        try
        {
            // Játékos keresése email és jelszó alapján
            var playerData = connection.Table<PlayerTbl>()
                                      .FirstOrDefault(p => p.playerEmail.Equals(email.Trim(), StringComparison.OrdinalIgnoreCase) 
                                      && p.playerPassword == password.Trim());
            
            if (playerData != null)
            {
                Debug.Log($"Player data loaded from SQLIte: {playerData.playerName}, {playerData.totalScore}");
                return playerData;
            } else
            {
                Debug.LogWarning("No player found with the given email and password.");
                return null;
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Error loading player data: {ex.Message}");
            return null;
        }
    }
    // Inventory mentése az adatbázisba
    public void SaveInventoryData(int inventoryId, Dictionary<PlantDatabase.Item, int> inventoryItems)
    {
        if (connection == null)
        {
            Debug.LogError("Database connection is null.");
            return;
        }

        try
        {
            foreach (var entry in inventoryItems)
            {
                var existingRecord = connection.Table<InventoryTbl>()
                                            .FirstOrDefault(x => x.InventoryId == inventoryId && x.MaterialId == entry.Key.plantId);

                if (existingRecord != null)
                {
                    // Ha már létezik a rekord, frissítsd a mennyiséget
                    existingRecord.MatQuantity = entry.Value;
                    connection.Update(existingRecord);
                } else
                {
                    // Ha nem létezik, hozz létre új rekordot
                    var inventoryData = new InventoryTbl
                    {
                        InventoryId = inventoryId,
                        MaterialId = entry.Key.plantId,
                        MatQuantity = entry.Value
                    };

                    connection.Insert(inventoryData);
                }
            }

            Debug.Log("Inventory data saved to SQLite database.");
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error saving inventory data: " + ex.Message);
        }
    }

    // Inventory betöltése az adatbázisból
    public List<MaterialDataFetchLite> LoadInventoryData(int inventoryId)
    {
        List<MaterialDataFetchLite> resultList = new List<MaterialDataFetchLite>();
        if (connection == null)
        {
            Debug.LogError("Database connection is null.");
            return resultList;
        }
        try
        {
            Debug.Log("inventoryid: " +inventoryId);
            var records = connection.Table<InventoryTbl>()
                                  .Where(x => x.InventoryId == inventoryId)
                                  .ToList();

            foreach (var record in records)
            {
                MaterialDataFetchLite data = new MaterialDataFetchLite
                {
                    materialId = record.MaterialId,
                    quantity = record.MatQuantity
                };
                resultList.Add(data);
            }

            Debug.Log("Inventory data loaded from SQLite database.");
            return resultList;
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error loading inventory data: " + ex.Message);
            return resultList;
        }

       
    }
    public void SaveCraftedInventoryData(int inventoryId, Dictionary<ItemDatabase.Item, int> craftedItems)
    {
        if (connection == null)
        {
            Debug.LogError("Database connection is null.");
            return;
        }

        try
        {
            foreach (var entry in craftedItems)
            {
                var existingRecord = connection.Table<CraftedInventoryTbl>()
                                              .FirstOrDefault(x => x.InventoryId == inventoryId && x.ItemId == entry.Key.itemId);

                if (existingRecord != null)
                {
                    // Ha már létezik a rekord, frissítsd a mennyiséget
                    existingRecord.Quantity = entry.Value;
                    connection.Update(existingRecord);
                } else
                {
                    // Ha nem létezik, hozz létre új rekordot
                    var craftedInventoryData = new CraftedInventoryTbl
                    {
                        InventoryId = inventoryId,
                        ItemId = entry.Key.itemId,
                        Quantity = entry.Value
                    };

                    connection.Insert(craftedInventoryData);
                }
            }

            Debug.Log("Crafted inventory data saved to SQLite database.");
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error saving crafted inventory data: " + ex.Message);
        }
    }
    public List<MaterialDataFetchLite> LoadCraftedInventoryData(int inventoryId)
    {
        List<MaterialDataFetchLite> resultList = new List<MaterialDataFetchLite>();

        if (connection == null)
        {
            Debug.LogError("Database connection is null.");
            return resultList;
        }
        try
        {
            var records = connection.Table<CraftedInventoryTbl>()
                                  .Where(x => x.InventoryId == inventoryId)
                                  .ToList();

            foreach (var record in records)
            {
                MaterialDataFetchLite data = new MaterialDataFetchLite
                {
                    materialId = record.ItemId,
                    quantity = record.Quantity
                };
                resultList.Add(data);
            }

            Debug.Log("Crafted inventory data loaded from SQLite database.");
            return resultList;
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error loading crafted inventory data: " + ex.Message);
            return resultList;
        }
        
    }
    void OnDestroy()
    {
        if (connection != null)
        {
            connection.Close();
        }
    }

    // Adatmodell a PlayerTbl táblához
    public class PlayerTbl {
        [PrimaryKey] public int playerId { get; set; }
        [MaxLength(255)] public string playerName { get; set; } = string.Empty;
        public string userId { get; set; } = string.Empty;
        public string playerEmail { get; set; }
        public string playerPassword { get; set; } = string.Empty;
        public int characterId { get; set; } = 0;
        public int characterIndex { get; set; } = 0;
        public int islandId { get; set; } = 0;
        public int inventoryId { get; set; } = 0;
        public int totalScore { get; set; } = 0;
        public string lastLogin { get; set; } = string.Empty;
        public string createdAt { get; set; } = string.Empty;
        public int isActive { get; set; } = 0;
    }

    public class CharacterTbl {
        [PrimaryKey] public int CharacterId { get; set; } 
        [MaxLength(50)] public string AstroSign { get; set; } 
        [MaxLength(10)] public string Gender { get; set; }
        public int CharacterIndex { get; set; } // Karakter indexe
    }

    public class InventoryTbl {
        [PrimaryKey] [AutoIncrement] public int RecordId { get; set; }
        public int InventoryId { get; set; }
        public int MaterialId { get; set; }
        public int MatQuantity  { get; set; }
        }
    public class CraftedInventoryTbl {
        [PrimaryKey, AutoIncrement] public int RecordId { get; set; }
        public int InventoryId { get; set; }
        public int ItemId { get; set; }
        public int Quantity { get; set; }
    }

    public class IslandTbl {
        [PrimaryKey] public int IslandId    { get; set; }
        public string AstroSign { get; set; }
        public int MaterialId { get; set; }
    }
    public class MaterialTbl {
        [PrimaryKey] public int MaterialId { get; set; }
        public string EnglishName { get; set; }
        public string WitchName { get; set; }
        public string LatinName { get; set; }
        public string Description { get; set; }
        public string Icon {  get; set; }
    }

    [System.Serializable]
    public class MaterialDataFetchLite {
        public int materialId;
        public int quantity;
    }
}