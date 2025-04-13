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

        // Dial�gus elrejt�se ind�t�skor
        if (GameManager.Instance.LoadLastCompletedIsland() == 0)
        {
            dialogPanel.SetActive(true);
        }
    }

    // Dial�gus megjelen�t�se t�bb sorral
    public void ShowDialog(List<string> lines)
    {
        dialogLines = lines; 
        currentLineIndex = 0; 

        // Az els� sor megjelen�t�se
        dialogText.text = dialogLines[currentLineIndex];
        dialogPanel.SetActive(true);

        // "Tov�bb" gomb enged�lyez�se, ha t�bb sor van
        nextButton.gameObject.SetActive(dialogLines.Count > 1);
        closeButton.gameObject.SetActive(false); 
    }

    // K�vetkez� sor megjelen�t�se
    private void ShowNextLine()
    {
        currentLineIndex++;

        // Ha van m�g sor, akkor megjelen�tj�k
        if (currentLineIndex < dialogLines.Count)
        {
            dialogText.text = dialogLines[currentLineIndex];
        } else
        {
            // Ha nincs t�bb sor, akkor bez�rjuk a dial�gust
            CloseDialog();
        }

        // "Tov�bb" gomb elrejt�se, ha ez az utols� sor
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