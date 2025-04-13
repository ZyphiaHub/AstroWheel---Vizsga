using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PuzzleGameManager : MonoBehaviour {
    [Header("Game Elements")]
    [Range(2, 6)]
    [SerializeField] private int difficulty = 4;
    [SerializeField] private Transform gameHolder;
    [SerializeField] private Transform piecePrefab;

    [Header("UI Element")]
    [SerializeField] private List<Texture2D> imageTexture;
    [SerializeField] private Transform levelSelectPanel;
    [SerializeField] private Image levelSelectPrefab;
    [SerializeField] private GameObject playAgainButton;
    

    [Header("Score")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI descText;
    private int score = 0;
    private int moves = 0;

    [Header("Inventory")]
    [SerializeField] private PlantDatabase plantDatabase;     


    private List<Transform> pieces;
    private Vector2Int dimensions;
    private float width;
    private float height;

    private Transform draggingPiece = null;
    private Vector3 offset;

    private int piecesCorrect;


    void Start()
    {
        

        // Ellenõrizzük, hogy az utolsó teljesített sziget 0-e
        if (GameManager.Instance.LoadLastCompletedIsland() == 0)
        {
            // Ha 0, akkor egybõl elindítjuk az elsõ képet
            if (imageTexture.Count > 0)
            {
                StartGame(imageTexture[0]); // Az elsõ kép betöltése
            } else
            {
                Debug.LogWarning("Nincs kép az imageTexture listában!");
            }
        } else   //a játékos már járt itt, választhat más képet
        {


            //create the UI
            foreach (Texture2D texture in imageTexture)
            {
                Image image = Instantiate(levelSelectPrefab, levelSelectPanel);
                image.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);

                image.GetComponent<Button>().onClick.AddListener(delegate { StartGame(texture); });
            }
            
                
                

        }
    }
    public void StartGame(Texture2D jigsawTexture)
    {
        // Reset the score
        score = 0;
        UpdateScoreUI();

        //hide the UI
        levelSelectPanel.gameObject.SetActive(false);

        //store a list of the transform for each jigsaw piece so we can track them
        pieces = new List<Transform>();

        //calculate the size of each
        dimensions = GetDimensions(jigsawTexture, difficulty);

        //create jigsaw pieces
        CreateJigsawPieces(jigsawTexture);

        //place the pieces randomly into the visible area
        Scatter();

        UpdateBorder();
        piecesCorrect = 0;
        descText.gameObject.SetActive(true);
        Vector2Int GetDimensions(Texture2D texture, int difficulty)
        {
            Vector2Int dimensions = Vector2Int.zero;
            //difficulty is the number of pieces on the smallest texture dimension
            //this helps ensure the pieces are square as possible
            if (jigsawTexture.width < jigsawTexture.height)
            {
                dimensions.x = difficulty;
                dimensions.y = (difficulty * jigsawTexture.height) / jigsawTexture.width;
            } else
            {
                dimensions.x = (difficulty * jigsawTexture.width) / jigsawTexture.height;
                dimensions.y = difficulty;
            }

            return dimensions;
        }
    }

    void CreateJigsawPieces(Texture2D jigsawTexture)
    {
        //calculate piece sizesbased on the dimensions
        height = 1f / dimensions.y;
        float aspect = (float)jigsawTexture.width / jigsawTexture.height;
        width = aspect / dimensions.x;

        for (int row = 0; row < dimensions.y; row++)
        {
            for (int col = 0; col < dimensions.x; col++)
            {
                //create the piece in the right location of the right size
                Transform piece = Instantiate(piecePrefab, gameHolder);
                piece.localPosition = new Vector3(
                    (-width * dimensions.x / 2) + (width * col) + (width / 2),
                    (-height * dimensions.y / 2) + (height * row) + (height / 2),
                    -1);
                piece.localScale = new Vector3(width, height, 1f);

                //name pieces fro our sanity (and debugging)
                piece.name = $"Piece{(row * dimensions.x) + col}";
                pieces.Add(piece);

                //assign the correct parts of the texture for this jigsaw piece
                float width1 = 1f / dimensions.x;
                float height1 = 1f / dimensions.y;
                Vector2[] uv = new Vector2[4];
                uv[0] = new Vector2(width1 * col, height1 * row);
                uv[1] = new Vector2(width1 * (col + 1), height1 * row);
                uv[2] = new Vector2(width1 * col, height1 * (row + 1));
                uv[3] = new Vector2(width1 * (col + 1), height1 * (row + 1));

                Mesh mesh = piece.GetComponent<MeshFilter>().mesh;
                mesh.uv = uv;
                //update the texture on the piece
                piece.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", jigsawTexture);

                // Add a BoxCollider2D to the piece
                BoxCollider2D collider = piece.gameObject.AddComponent<BoxCollider2D>();
                collider.size = new Vector2(width, height); // Set collider size to match the piece size

            }

        }
    }

    private void Scatter()
    {
        float orthoHeight = Camera.main.orthographicSize;
        float screenAspect = (float)Screen.width / Screen.height;
        float orthoWidth = (screenAspect * orthoHeight);

        float pieceWidth = width * gameHolder.localScale.x;
        float pieceHeight = height * gameHolder.localScale.y;

        orthoHeight -= pieceHeight;
        orthoWidth -= pieceWidth;

        //place each peces randomly
        foreach (Transform piece in pieces)
        {
            float x = Random.Range(-orthoWidth, orthoWidth);
            float y = Random.Range(-orthoHeight, orthoHeight);
            piece.position = new Vector3(x, y, -1);
        }
    }

    // Update the border to fit the chosen puzzle.
    private void UpdateBorder()
    {
        LineRenderer lineRenderer = gameHolder.GetComponent<LineRenderer>();

        // Calculate half sizes to simplify the code.
        float halfWidth = (width * dimensions.x) / 2f;
        float halfHeight = (height * dimensions.y) / 2f;

        // We want the border to be behind the pieces.
        float borderZ = 0f;

        // Set border vertices, starting top left, going clockwise.
        lineRenderer.SetPosition(0, new Vector3(-halfWidth, halfHeight, borderZ));
        lineRenderer.SetPosition(1, new Vector3(halfWidth, halfHeight, borderZ));
        lineRenderer.SetPosition(2, new Vector3(halfWidth, -halfHeight, borderZ));
        lineRenderer.SetPosition(3, new Vector3(-halfWidth, -halfHeight, borderZ));

        // Set the thickness of the border line.
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;

        // Show the border line.
        lineRenderer.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Convert mouse position to world coordinates
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPos.z = 0; // Ensure z is 0 for 2D

            RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos, Vector2.zero);
            if (hit)
            {
                // Increase move count
                moves++;
                //Debug.Log("Moves: " + moves);
                // Everything is moveable, so we don't need to check it's a Piece.
                draggingPiece = hit.transform;
                offset = draggingPiece.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
                offset += Vector3.back;
                //Debug.Log("Piece clicked: " + hit.transform.name); // Debug log
            } else
            {
                Debug.Log("No piece clicked"); // Debug log
            }
        }

        // When we release the mouse button stop dragging.
        if (draggingPiece && Input.GetMouseButtonUp(0))
        {
            SnapAndDisableIfCorrect();
            draggingPiece.position += Vector3.forward;
            draggingPiece = null;
        }

        // Set the dragged piece position to the position of the mouse.
        if (draggingPiece)
        {
            Vector3 newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            
            newPosition += offset;
            draggingPiece.position = newPosition;
        }
    }

    private void SnapAndDisableIfCorrect()
    {
        // We need to know the index of the piece to determine it's correct position.
        int pieceIndex = pieces.IndexOf(draggingPiece);

        // The coordinates of the piece in the puzzle.
        int col = pieceIndex % dimensions.x;
        int row = pieceIndex / dimensions.x;

        // The target position in the non-scaled coordinates.
        Vector2 targetPosition = new((-width * dimensions.x / 2) + (width * col) + (width / 2),
                                     (-height * dimensions.y / 2) + (height * row) + (height / 2));

        // Check if we're in the correct location.
        if (Vector2.Distance(draggingPiece.localPosition, targetPosition) < (width / 2))
        {
            // Snap to our destination.
            draggingPiece.localPosition = targetPosition;

            // Disable the collider so we can't click on the object anymore.
            draggingPiece.GetComponent<BoxCollider2D>().enabled = false;

            // Increase the number of correct pieces, and check for puzzle completion.
            piecesCorrect++;
            score += 3; 
            UpdateScoreUI();

            if (piecesCorrect == pieces.Count)
            {
                playAgainButton.SetActive(true);
                score = score - moves;
                if (score <= 0) {
                    score = 3;
                }
                
                Debug.Log(score);
                UpdateScoreUI();

                OnPuzzleSolved();
            }
        }
    }
    public void RestartGame()
    {
        // Destroy all the puzzle pieces.
        foreach (Transform piece in pieces)
        {
            Destroy(piece.gameObject);
        }
        pieces.Clear();

        // Reset the score
        score = 0;
        UpdateScoreUI();

        // Hide the outline
        gameHolder.GetComponent<LineRenderer>().enabled = false;
        // Show the level select UI.
        playAgainButton.SetActive(false);
        levelSelectPanel.gameObject.SetActive(true);
    }
    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score + "    Moves: " + moves; 
            
        }
    }
    private void OnPuzzleSolved()
    {
        // Frissítjük a játékos összpontszámát a GameManager-ben
        int currentTotalScore = GameManager.Instance.LoadTotalScore();
        
        GameManager.Instance.SaveTotalScore(currentTotalScore + score);
        Debug.Log("current totalscore: "+currentTotalScore);

        //serverre score
        int inventoryId = PlayerPrefs.GetInt("InventoryID");
        int totalScore = GameManager.Instance.LoadTotalScore();

        StartCoroutine( APIClient.Instance.UpdateTotalScore(
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
        // Beállítjuk, hogy a puzzle megoldva van
        GameManager.Instance.SetPuzzleSolved(true);

        if (GameManager.Instance.LoadLastCompletedIsland() == 0) {
            GameManager.Instance.SaveLastCompletedIsland(1);

            // serverre is küldöm
            int playerId = GameManager.Instance.LoadPlayerId();
            Debug.Log("puzzle vége:"+ playerId);
            int newIslandId = 1; 

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

        // Hozzáadjuk a 0 indexû tárgyat az inventoryhoz
        if (plantDatabase != null && plantDatabase.items.Length > 0)
        {
            PlantDatabase.Item itemToAdd = plantDatabase.items[0];
            int quantityToAdd = score / 2;
            if (quantityToAdd < 1) { quantityToAdd = 1; }          
                
            InventoryManager.Instance.inventory.AddItem(itemToAdd, quantityToAdd);

            Debug.Log($"Item added to inventory: {itemToAdd.englishName}, Quantity: {quantityToAdd}");
            
            InventoryManager.Instance.inventory.PrintInventory();
            InventoryManager.Instance.SaveInventory();
            InventoryManager.Instance.SaveCraftedInventoryToServer();
        } else
            {
            Debug.LogWarning("PlantDatabase nincs beállítva vagy nincsenek tárgyak!");
        }
        
        if (plantDatabase == null)
        {
            Debug.LogError("PlantDatabase nincs beállítva a PuzzleGameManager-ben!");
            return;
        }

       

    }

    

}