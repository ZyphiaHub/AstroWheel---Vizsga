using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class IslandManager : MonoBehaviour, IGameState {
    public int islandIndex; 
    public DialogManager dialogManager;
    public Button bactToMainMenuBtn;
    public Button previousIslandButton;
    public Button nextIslandButton;
    public Image characterImage;

    private void Start()
    {
        if (bactToMainMenuBtn != null)
        {
            bactToMainMenuBtn.onClick.AddListener(OnBackToMainMenuClicked);
        }
        if (previousIslandButton != null)
        {
            previousIslandButton.onClick.AddListener(OnPreviousIslandClicked);
        }
        if (nextIslandButton != null)
        {
            nextIslandButton.onClick.AddListener(OnNextIslandClicked);
        }

        EnterState();

        LoadSelectedCharacterImage();
    }

    public void EnterState()
    {
        Debug.Log($"Bel�pt�l a(z) {islandIndex}. szigetre!");
        int lastCompletedIsland = GameManager.Instance.LoadLastCompletedIsland();

        Debug.Log($"Last completed island: {lastCompletedIsland}, Current island: {islandIndex}");
        if (lastCompletedIsland < islandIndex)
        {
            List<string> dialogList = new List<string>(DialogDatabase.GetDialogForIsland(islandIndex));
            dialogManager.ShowDialog(dialogList);
        } else
        {
            //dialogManager.dialogPanel.SetActive(false);
        }
        UpdateNavigationButtons();
    }

    public void UpdateState()
    {
        if (IsPuzzleSolved())
        {
            ExitState();
        }
    }

    public void ExitState()
    {
        Debug.Log($"Kil�pt�l a(z) {islandIndex}. szigetr�l!");

        GameManager.Instance.SaveLastCompletedIsland(islandIndex);

    }
    private void OnBackToMainMenuClicked()
    {
        SceneManager.LoadScene("Main_Menu");
    }

    private void OnPreviousIslandClicked()
    {
        NavigateToIsland(islandIndex - 1);
    }

    private void OnNextIslandClicked()
    {
        NavigateToIsland(islandIndex + 1);
    }

    private void NavigateToIsland(int targetIslandIndex)
    {
        if (targetIslandIndex >= 1 && targetIslandIndex <= 12)
        {
            Debug.Log($"Navig�l�s a(z) {targetIslandIndex}. szigetre!");
            SceneManager.LoadScene($"Island_{targetIslandIndex}");
        } else
        {
            Debug.LogWarning($"�rv�nytelen sziget index: {targetIslandIndex}");
        }
    }

    private void UpdateNavigationButtons()
    {
        // El�z� sziget gomb letilt�sa, ha az els� szigeten vagyunk
        if (previousIslandButton != null)
        {
            previousIslandButton.interactable = (islandIndex > 1);
        }

        // K�vetkez� sziget gomb letilt�sa, ha az utols� szigeten vagyunk
        if (nextIslandButton != null)
        {
            int lastCompletedIsland = GameManager.Instance.LoadLastCompletedIsland();
            bool isNextIslandWithinLimit = (islandIndex + 1 <= lastCompletedIsland + 1) && (islandIndex + 1 <= 12);
            nextIslandButton.interactable = isNextIslandWithinLimit;
        }
    }


    private bool IsPuzzleSolved()
    {
        return GameManager.Instance.IsPuzzleSolved(islandIndex);
    }

    private void LoadSelectedCharacterImage()
    {

        int selectedCharacterIndex = PlayerPrefs.GetInt("CharacterIndex", 0);

        if (RegisterManager.Instance != null && RegisterManager.Instance.CharacterSprites != null)
        {
            if (selectedCharacterIndex >= 0 && selectedCharacterIndex < RegisterManager.Instance.CharacterSprites.Length)
            {
                characterImage.sprite = RegisterManager.Instance.CharacterSprites[selectedCharacterIndex];
            } else
            {
                Debug.LogWarning("�rv�nytelen karakterk�p index: " + selectedCharacterIndex);
            }
        } else
        {
            Debug.LogWarning("RegisterManager vagy characterSprites t�mb nincs be�ll�tva!");
        }
    }
}