using System.Collections;
using TMPro;
using UnityEngine;

public enum GameState{
    wait,
    move
}


public class Board : MonoBehaviour {

    public GameState currentState = GameState.move;
    public int width;
    public int height;
    public int offset;
    public int match3Point = 0;
    public GameObject[] dots;
    public GameObject tilePrefab;
    
    private BackgroundTile[,] allTiles;
    public GameObject[,] allDots;
    private FindMatches findMatches;

    public int remMoves = 12;
    public TMP_Text scoreText;
    public TMP_Text movesText;
    public TMP_Text descText;

    [Header("Inventory")]
    [SerializeField] private PlantDatabase plantDatabase;
    [Header("UI Elements")]
    public GameObject playAgainButton;

    void Start () {
        findMatches = FindObjectOfType<FindMatches>();
        allTiles = new BackgroundTile[width, height];
        allDots = new GameObject[width, height];
        SetUp();
        UpdateUI();
	}
	
    private void SetUp(){
        for (int i = 0; i < width; i++){
            for (int j = 0; j < height; j++){
                Vector2 tempPosition = new Vector2(i, j + offset);
                GameObject backgroundTile = Instantiate(tilePrefab, tempPosition,Quaternion.identity) as GameObject;
                backgroundTile.transform.parent = this.transform;
                backgroundTile.name = "tile:( " + i + ", " + j + " )";

                int dotToUse = Random.Range(0, dots.Length);

                //a tábla építésekor nem hagyom, hogy 3-as egyezés létrejöjjön, másik random dot-ot választok, ha matchesAt igaz
                int maxIterations = 0;
                while (MatchesAt(i, j, dots[dotToUse]) && maxIterations < 100)
                {
                    dotToUse = Random.Range(0, dots.Length);
                    maxIterations++;
                    Debug.Log(maxIterations);
                }
                maxIterations = 0;

                GameObject dot = Instantiate(dots[dotToUse], tempPosition, Quaternion.identity);
                dot.GetComponent<Dot>().row = j;
                dot.GetComponent<Dot>().column = i; 
                dot.transform.parent = this.transform;
                dot.name = "dot:( " + i + ", " + j + " )";
                allDots[i, j] = dot;
            }
        }
    }

    private bool MatchesAt(int column, int row, GameObject piece)
    {
        if (column > 1 && row > 1)
        {
            if (allDots[column - 1, row].tag == piece.tag && allDots[column - 2, row].tag == piece.tag)
            {     return true;    }

            if (allDots[column, row - 1].tag == piece.tag && allDots[column, row - 2].tag == piece.tag)
            {     return true;    }

        } else if (column <= 1 || row <= 1)
        {
            if (row > 1)
            {
                if (allDots[column, row - 1].tag == piece.tag && allDots[column, row - 2].tag == piece.tag)
                {
                    return true;
                }
            }
            if (column > 1)
            {
                if (allDots[column - 1, row].tag == piece.tag && allDots[column - 2, row].tag == piece.tag)
                {
                    return true;
                }
            }
        }

        return false;
    }

    private void DestroyMatchesAt(int column, int row)
    {
        if (allDots[column, row].GetComponent<Dot>().isMatched)
        {
            
            match3Point ++;
            Debug.Log("timePoint:" + match3Point);
            //mennyi elem van a match-ben?
            findMatches.currentMatches.Remove(allDots[column, row]);
            Destroy(allDots[column, row]);
            allDots[column, row] = null; 
        }
        UpdateUI();
    }

    public void DestroyMatches()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (allDots[i, j] != null)
                {
                    
                    DestroyMatchesAt(i, j);
                }
            }
        }
        StartCoroutine(DecreaseRowCo());
    }

    private IEnumerator DecreaseRowCo()
    {
        int nullCount = 0;
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (allDots[i, j] == null)
                {
                    nullCount++;
                } else if (nullCount > 0)
                {
                    allDots[i, j].GetComponent<Dot>().row -= nullCount;
                    allDots[i, j] = null;
                }
            }
            nullCount = 0;
        }
        yield return new WaitForSeconds(.4f);
        StartCoroutine(FillBoardCo());
    }

    private void RefillBoard()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (allDots[i, j] == null)
                {
                    Vector2 tempPosition = new Vector2(i, j + offset);
                    int dotToUse = Random.Range(0, dots.Length);
                    GameObject piece = Instantiate(dots[dotToUse], tempPosition, Quaternion.identity);
                    allDots[i, j] = piece;
                    piece.GetComponent<Dot>().row = j;
                    piece.GetComponent<Dot>().column = i;

                }
            }
        }
    }
    //dotok eltörlése és feltöltése után keletkezett új egyezéseketeket keres
    private bool MatchesOnBoard()         
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (allDots[i, j] != null)
                {
                    if (allDots[i, j].GetComponent<Dot>().isMatched)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    private IEnumerator FillBoardCo()
    {
        RefillBoard();
        yield return new WaitForSeconds(.5f);

        while (MatchesOnBoard())
        {
            yield return new WaitForSeconds(.5f);
            DestroyMatches();
        }
        yield return new WaitForSeconds(.5f);
        currentState = GameState.move;

    }
    public void DecreaseMoveCount()
    {
        remMoves--;

        if (remMoves <= 0)
        {
            EndGame();
        }
        UpdateUI();
    }
    void EndGame()
    {
        Debug.Log("A játék véget ért!");
        int currentTotalScore = GameManager.Instance.LoadTotalScore();

        GameManager.Instance.SaveTotalScore(currentTotalScore + match3Point);
        Debug.Log("current totalscore: " + currentTotalScore);
        //serverre score feltöltés
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


        GameManager.Instance.SetPuzzleSolved(true);

        if (GameManager.Instance.LoadLastCompletedIsland() == 1)
        {
            GameManager.Instance.SaveLastCompletedIsland(2);

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
        // Hozzáadjuk a 1 indexű tárgyat az inventoryhoz
        if (plantDatabase != null && plantDatabase.items.Length > 0)
        {
            PlantDatabase.Item itemToAdd = plantDatabase.items[1];
            int quantityToAdd = match3Point / 4;
            if (quantityToAdd < 1) { quantityToAdd = 1; }

            InventoryManager.Instance.inventory.AddItem(itemToAdd, quantityToAdd);

            Debug.Log($"Item added to inventory: {itemToAdd.englishName}, Quantity: {quantityToAdd}");

            InventoryManager.Instance.inventory.PrintInventory();
            InventoryManager.Instance.SaveInventoryToServer();
            InventoryManager.Instance.SaveCraftedInventoryToServer();
            descText.text = ($"The game\nhas finished.\nYou gained\n{quantityToAdd} Velvet bean");
        } else
        {
            Debug.LogWarning("PlantDatabase nincs beállítva vagy nincsenek tárgyak!");
        }

        if (plantDatabase == null)
        {
            Debug.LogError("PlantDatabase nincs beállítva a PuzzleGameManager-ben!");
            return;
        }
        playAgainButton.SetActive(true);
        currentState = GameState.wait;
        StartCoroutine(SetDotsInActive());
        
        UpdateUI();
    }

    public void RestartGame()
    {
        // Tisztítás
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (allDots[i, j] != null)
                {
                    Destroy(allDots[i, j]);
                }
            }
        }

        match3Point = 0;
        remMoves = 15; 
        currentState = GameState.move;
        //újraépítés

        SetUp();
        playAgainButton.SetActive(false);
        descText.text = "Every icon you match, worth 1 point.\n\nAutomatic matches doesn't count against your steps.";
        UpdateUI();
    }


    void UpdateUI()
    {
        scoreText.text = "Collected \nScore: \n" + match3Point;
        movesText.text = "Remaining\nMoves:\n " + remMoves;
    }
    //amíg újratöltjük a dotokat ne lehessen belekattintani mert összezavarjuk a játékot!
    private IEnumerator SetDotsInActive()
    {
        yield return StartCoroutine(FillBoardCo());
        yield return new WaitForSeconds(3f);
        
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (allDots[i, j] != null)
                {
                    allDots[i, j].SetActive(false);
                }
            }
        }
    }
}
