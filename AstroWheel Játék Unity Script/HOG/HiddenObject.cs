using UnityEngine;

public class HiddenObject : MonoBehaviour {
    public string ObjectName;
    private bool isClickable = false;
    [SerializeField] private HOGManager manager;

    private void Start()
    {
        // Automatikus keres�s, ha nincs be�ll�tva
        if (manager == null)
        {
            manager = FindObjectOfType<HOGManager>();
            if (manager == null)
                Debug.LogError("HOGManager not found in scene!");
        }
    }
    public void SetClickable(bool state)
    {
        isClickable = state;
        GetComponent<Collider2D>().enabled = state;
    }

    private void OnMouseDown()
    {
        if (isClickable)
        {
            manager.ObjectFound(this);
            //HOGManager.Instance.ObjectFound(this);
            SetClickable(false);
            // Opcion�lis: vizu�lis effektus
            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0f);
        }
    }
}
