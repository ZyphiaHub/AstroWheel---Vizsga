using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class HOGManager : MonoBehaviour {

    [Header("UI Elements")]
    public TMP_Text scoreText;
    public TMP_Text targetListText;
    public Button playAgainButton;
    [SerializeField] private PlantDatabase plantDatabase;

    [Header("Game Settings")]
    public int totalObjectsToFind = 10;

    private List<HiddenObject> foundObjects = new List<HiddenObject>();
    private List<HiddenObject> targetObjects = new List<HiddenObject>();
    private int score;

    private void Start()
    {
        playAgainButton.onClick.AddListener(ResetGame);
        playAgainButton.gameObject.SetActive(false); 
    }
    public void SetTargetObjects(List<HiddenObject> targets)
    {
        targetObjects = targets;
        UpdateUI();
    }

    public void ObjectFound(HiddenObject obj)
    {
        if (foundObjects.Contains(obj)) return;

        foundObjects.Add(obj);
        UpdateUI();

        if (foundObjects.Count >= totalObjectsToFind)
        {
            Debug.Log("You Win!");
            targetListText.text = "All objects found! Congratulations!";
            playAgainButton.gameObject.SetActive(true);


            //pont feltoltes
            int currentTotalScore = GameManager.Instance.LoadTotalScore();

            GameManager.Instance.SaveTotalScore(currentTotalScore + score);
            Debug.Log("current totalscore: " + currentTotalScore);
            //serverre score
            int inventoryId = PlayerPrefs.GetInt("InventoryID");
            int totalScore = GameManager.Instance.LoadTotalScore();

            StartCoroutine(APIClient.Instance.UpdateTotalScore(
           inventoryId,
           totalScore,
           onSuccess: response =>
           {
               //Debug.Log("Inventory updated successfully: " + response);
           },
           onError: error =>
           {
               Debug.LogError("Failed to update inventory: " + error);
           }
       ));

            if (GameManager.Instance.LoadLastCompletedIsland() == 5)
            {
                GameManager.Instance.SaveLastCompletedIsland(6);

                int playerId = GameManager.Instance.LoadPlayerId();
                Debug.Log("puzzle vége:" + playerId);
                int newIslandId = 2;

                StartCoroutine(APIClient.Instance.UpdatePlayerIslandId(
                    playerId,
                    newIslandId,
                    onSuccess: response =>
                    {
                        Debug.Log("IslandId updated successfully: " + response);
                    },
                    onError: error =>
                    {
                        Debug.LogError("Failed to update IslandId: " + error);
                    }
                ));
            }
            // Hozzáadjuk a 5 indexû tárgyat az inventoryhoz
            if (plantDatabase != null && plantDatabase.items.Length > 0)
            {
                PlantDatabase.Item itemToAdd = plantDatabase.items[5];
                int quantityToAdd = score ;
                if (quantityToAdd < 1) { quantityToAdd = 1; }

                InventoryManager.Instance.inventory.AddItem(itemToAdd, quantityToAdd);

                Debug.Log($"Item added to inventory: {itemToAdd.englishName}, Quantity: {quantityToAdd}");

                InventoryManager.Instance.inventory.PrintInventory();
                InventoryManager.Instance.SaveInventoryToServer();
                InventoryManager.Instance.SaveCraftedInventoryToServer();
                targetListText.text = ($"The game\nhas finished.\nYou gained\n{quantityToAdd} Bulbous Buttercup");
            } else
            {
                Debug.LogWarning("PlantDatabase nincs beállítva vagy nincsenek tárgyak!");
            }

            if (plantDatabase == null)
            {
                Debug.LogError("PlantDatabase nincs beállítva a HOGManager-ben!");
                return;
            }
        }
    }

    private void UpdateUI()
    {
        playAgainButton.gameObject.SetActive(false);

        if (scoreText != null)
            
            scoreText.text = $"Score: {score} points";

        if (targetListText != null)
        {
            string targetNames = "";
            int remainingCount = 0;

            foreach (var obj in targetObjects)
            {
                if (!foundObjects.Contains(obj))
                {
                    targetNames += $"- {obj.ObjectName}\n";
                    remainingCount++;
                }
            }

            if (remainingCount > 0)
            {
                targetListText.text = $"Find these objects ({remainingCount} left):\n{targetNames}";
                score = (totalObjectsToFind - remainingCount) * 3;
                scoreText.text = $"Score: {score} points";
            }
        }
    }

    public void ResetGame()
    {
        foundObjects.Clear();
        foreach (var obj in FindObjectsOfType<HiddenObject>())
        {
            Destroy(obj.gameObject);
        }
        FindObjectOfType<RandomObjectPlacer>().PlaceRandomObjects();
        playAgainButton.gameObject.SetActive(false);
        score = 0;
        targetObjects.Clear();
        UpdateUI();
    }
}