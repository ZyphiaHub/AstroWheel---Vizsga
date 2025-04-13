using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MemoryCard : MonoBehaviour {
    public Sprite frontImage; // Az el�lap k�pe
    public Sprite backImage;  // A h�tlap k�pe
    public Image cardImage;  // Az aktu�lis k�p
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

