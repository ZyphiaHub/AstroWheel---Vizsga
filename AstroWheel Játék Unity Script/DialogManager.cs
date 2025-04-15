using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class DialogManager : MonoBehaviour {
    public GameObject dialogPanel; 
    public TMP_Text dialogText;    
    public Button nextButton;      
    public Button closeButton;     

    private List<string> dialogLines; 
    private int currentLineIndex;     

    private void Start()
    {
        
        nextButton.onClick.AddListener(ShowNextLine);
        closeButton.onClick.AddListener(CloseDialog);

        if (GameManager.Instance.LoadLastCompletedIsland() == 0)
        {
            dialogPanel.SetActive(true);
        }
    }

    public void ShowDialog(List<string> lines)
    {
        dialogLines = lines; 
        currentLineIndex = 0; 

        dialogText.text = dialogLines[currentLineIndex];
        dialogPanel.SetActive(true);

        // "Tovább" gomb engedélyezése, ha több sor van
        nextButton.gameObject.SetActive(dialogLines.Count > 1);
        closeButton.gameObject.SetActive(false); 
    }

    private void ShowNextLine()
    {
        currentLineIndex++;

        if (currentLineIndex < dialogLines.Count)
        {
            dialogText.text = dialogLines[currentLineIndex];
        } else
        {
            CloseDialog();
        }

        // "Tovább" gomb elrejtése, ha ez az utolsó sor
        if (currentLineIndex == dialogLines.Count - 1)
        {
            nextButton.gameObject.SetActive(false);
            closeButton.gameObject.SetActive(true); 
        }
    }

    private void CloseDialog()
    {
        dialogPanel.SetActive(false); 
    }
}