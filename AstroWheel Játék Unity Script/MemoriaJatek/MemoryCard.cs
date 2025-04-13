using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MemoryCard : MonoBehaviour {
    public Sprite frontImage; // Az elõlap képe
    public Sprite backImage;  // A hátlap képe
    public Image cardImage;  // Az aktuális kép
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

