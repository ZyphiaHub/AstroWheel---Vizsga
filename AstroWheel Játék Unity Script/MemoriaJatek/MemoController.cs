using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MemoController : MonoBehaviour {
    [SerializeField]
    private Sprite backImage;

    public Sprite[] cardFaces;
    private int score = 0;
    private int remGuesses = 24;
    [SerializeField] private TMP_Text scoreText;  
    [SerializeField] private TMP_Text guessesText;
    [SerializeField] private PlantDatabase plantDatabase;
    public TMP_Text endText;
    public GameObject playAgainButton;

    public List<Sprite> cardPairs = new List<Sprite>();

    public List<Button> cardList = new List<Button>();

    private bool firstPick, secondPick;
    private int countPicks;
    
    private int countCorrectPicks;
    private int gamePicks;
    private int firstPickIndex, secondPickIndex;
    private string firstPickPuzzle, secondPickPuzzle;


    void Start()
    {
        GetButtons();
        AddListeners();
        AddCardPairs();
        Shuffle(cardPairs);
        gamePicks = cardPairs.Count / 2;
        playAgainButton.SetActive(false);
        UpdateUI();
    }
    void InitializeGame()
    {
        // Visszaállítás kezdõértékekre
        score = 0;
        remGuesses = 24;
        countCorrectPicks = 0;
        firstPick = secondPick = false;
        firstPickIndex = secondPickIndex = -1;
        firstPickPuzzle = secondPickPuzzle = "";

        // Kártyák visszaállítása
        foreach (Button card in cardList)
        {
            card.image.sprite = backImage;
            card.image.color = Color.white;
            card.interactable = true;
        }

        // Kártyapárok újragenerálása és összekeverése
        cardPairs.Clear();
        AddCardPairs();
        Shuffle(cardPairs);

        // UI frissítése
        UpdateUI();
        playAgainButton.SetActive(false);
    }
    void GetButtons()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("CardButton");

        for (int i = 0; i < objects.Length; i++)
        {
            cardList.Add(objects[i].GetComponent<Button>());
            cardList[i].image.sprite = backImage;
        }

    }

    void AddCardPairs()
    {
        int looper = cardList.Count;
        int index = 0;

        for (int i = 0; i < looper; i++)
        {
            if (index == looper / 2) { index = 0; }
            cardPairs.Add(cardFaces[index]);
            index++;
        }
    }

    void AddListeners()
    {
        foreach (Button card in cardList)
        {
            card.onClick.AddListener(() => PickACard());
        }
    }

    public void PickACard()
    {
        string name = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name;

        if (!firstPick)
        {
            firstPick = true;
            firstPickIndex = int.Parse(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name);
            firstPickPuzzle = cardPairs[firstPickIndex].name;

            cardList[firstPickIndex].image.sprite = cardPairs[firstPickIndex];

        } else if (!secondPick)
        {
            secondPick = true;
            secondPickIndex = int.Parse(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name);
            secondPickPuzzle = cardPairs[secondPickIndex].name;

            cardList[secondPickIndex].image.sprite = cardPairs[secondPickIndex];

            StartCoroutine(CheckCardMatch());


        }
    }

    IEnumerator CheckCardMatch()
    {
        yield return new WaitForSeconds(.7f);
        remGuesses--;

        if (firstPickPuzzle == secondPickPuzzle)
        {
            yield return new WaitForSeconds(.5f);
            cardList[firstPickIndex].interactable = false;
            cardList[secondPickIndex].interactable = false;

            cardList[firstPickIndex].image.color = new Color(0, 0, 0, 0);
            cardList[secondPickIndex].image.color = new Color(0, 0, 0, 0);

            //pontozás
            score += Mathf.Max(1, 10 - countPicks);
            Debug.Log("pontszám:" + score);

            
            CheckIfTheGameIsFinished();

        } else
        {
            cardList[firstPickIndex].image.sprite = backImage;
            cardList[secondPickIndex].image.sprite = backImage;
        }

        yield return new WaitForSeconds(.5f);
        firstPick = secondPick = false;

        Debug.Log("remaining guesses" + remGuesses);
        UpdateUI();
        if (remGuesses > 0)
        {
            EndGame(false);
        } else {
            Debug.Log("Vesztettél");

            }
    }

    void CheckIfTheGameIsFinished()
    {
        countCorrectPicks++;

        if (countCorrectPicks == gamePicks)
        {
            Debug.Log("game finished");
            Debug.Log(countPicks);
            Debug.Log("összpontszám:" + score);
            endText.text = ($"The game\nhas finished.\nYou have lost!");
            playAgainButton.SetActive(true);
            EndGame(true);
            
        }
    }

    void Shuffle(List<Sprite> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            Sprite temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

    void EndGame(bool won)
    {
        if (won)
        {
            playAgainButton.SetActive(true);
            int currentTotalScore = GameManager.Instance.LoadTotalScore();

            GameManager.Instance.SaveTotalScore(currentTotalScore + score/2);
            Debug.Log("current totalscore: " + currentTotalScore);
            //serverre score
            int inventoryId = PlayerPrefs.GetInt("InventoryID");
            int totalScore = GameManager.Instance.LoadTotalScore();

            Debug.Log("Nyertél! A játéknak vége");
            if (GameManager.Instance.LoadLastCompletedIsland() == 6)
            {
                GameManager.Instance.SaveLastCompletedIsland(7);

                // serverre is küldöm
                int playerId = GameManager.Instance.LoadPlayerId();
                Debug.Log("mem puzzle vége");
                int newIslandId = 8;

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

            // Hozzáadjuk a 6 indexû tárgyat az inventoryhoz
            if (plantDatabase != null && plantDatabase.items.Length > 0)
            {
                PlantDatabase.Item itemToAdd = plantDatabase.items[6];
                int quantityToAdd = score/4;
                if (quantityToAdd < 1) { quantityToAdd = 1; }

                InventoryManager.Instance.inventory.AddItem(itemToAdd, quantityToAdd);

                Debug.Log($"Item added to inventory: {itemToAdd.englishName}, Quantity: {quantityToAdd}");

                InventoryManager.Instance.inventory.PrintInventory();
                InventoryManager.Instance.SaveInventoryToServer();
                InventoryManager.Instance.SaveCraftedInventoryToServer();
                endText.text = ($"The game\nhas finished.\nYou gained\n{quantityToAdd} Cat's paw");
            } else
            {
                Debug.LogWarning("PlantDatabase nincs beállítva vagy nincsenek tárgyak!");
            }
            UpdateUI();
        } 
        
    }
    public void RestartGame()
    {
        InitializeGame();
    }
    void UpdateUI()
    {
        scoreText.text = "Score: " + score;
        guessesText.text = "Remaining Guesses: " + remGuesses;
        Debug.Log("update ui");
        }
}
