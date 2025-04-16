using UnityEngine;

public class AddCards : MonoBehaviour
{
    [SerializeField]
    private Transform gameBoard;

    [SerializeField]
    private GameObject memCard;

    private void Awake()
    {
       for(int i = 0; i < 16; i++)
        {
            GameObject card = Instantiate(memCard);
            card.name = "" + i ;
            card.transform.SetParent(gameBoard, false);
        } 

        
    }
}
