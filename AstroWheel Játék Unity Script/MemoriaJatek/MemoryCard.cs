using UnityEngine;
using UnityEngine.UI;

public class MemoryCard : MonoBehaviour {
    public Sprite frontImage; 
    public Sprite backImage;  
    public Image cardImage;  
    private bool isFlipped = false;

    private void Start()
    {
        cardImage = GetComponent<Image>();
    }


    public void FlipCard()
    {
        isFlipped = !isFlipped;
        if (isFlipped)
        {
            ShowFront();
        } else
        {
            ShowBack();
        }
    }

    public void ShowFront()
    {
        cardImage.sprite = frontImage;
    }

    public void ShowBack()
    {
        cardImage.sprite = backImage;
    }
}

