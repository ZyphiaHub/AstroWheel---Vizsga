using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class ItemInitializer : MonoBehaviour {
    public ItemDatabase itemDatabase; // Referencia az ItemDatabase-re

#if UNITY_EDITOR
    [ContextMenu("Initialize Items")]
    public void InitializeItems()
    {
        if (itemDatabase == null)
        {
            Debug.LogError("ItemDatabase nincs be�ll�tva!");
            return;
        }

        // T�rgyak inicializ�l�sa
        itemDatabase.items = new ItemDatabase.Item[]
        {
            new ItemDatabase.Item
            {
                itemName = "Egy",
                description = "A sharp blade for close combat.",
                icon = LoadSprite("Assets/Art/Items/Avarvir�g.png") // P�lda el�r�si �t
            },
            new ItemDatabase.Item
            {
                itemName = "Shield",
                description = "Protects against enemy attacks.",
                icon = LoadSprite("Assets/Art/Items/Korpaf�.png") // P�lda el�r�si �t
            },
            new ItemDatabase.Item
            {
                itemName = "H�rom",
                description = "Protects against enemy attacks.",
                icon = LoadSprite("Assets/Art/Items/Korpaf�.png") // P�lda el�r�si �t
            },
            new ItemDatabase.Item
            {
                itemName = "Shield",
                description = "Protects against enemy attacks.",
                icon = LoadSprite("Assets/Art/Items/Korpaf�.png") // P�lda el�r�si �t
            },
            new ItemDatabase.Item
            {
                itemName = "�t",
                description = "Protects against enemy attacks.",
                icon = LoadSprite("Assets/Art/Items/Korpaf�.png") // P�lda el�r�si �t
            },
            new ItemDatabase.Item
            {
                itemName = "Shield",
                description = "Protects against enemy attacks.",
                icon = LoadSprite("Assets/Art/Items/Korpaf�.png") // P�lda el�r�si �t
            },
            new ItemDatabase.Item
            {
                itemName = "Shield",
                description = "Protects against enemy attacks.",
                icon = LoadSprite("Assets/Art/Items/Korpaf�.png") // P�lda el�r�si �t
            },
            new ItemDatabase.Item
            {
                itemName = "Potion",
                description = "Restores 20 HP.",
                icon = LoadSprite("Assets/Art/Items/Tarl�here.png") // P�lda el�r�si �t
            }
        };

        // Mentj�k a v�ltoztat�sokat
        EditorUtility.SetDirty(itemDatabase);
        AssetDatabase.SaveAssets();
        Debug.Log("ItemDatabase inicializ�lva �s mentve!");
    }

    // Sprite bet�lt�se az el�r�si �t alapj�n (csak Editorban)
    private Sprite LoadSprite(string path)
    {
        return AssetDatabase.LoadAssetAtPath<Sprite>(path);
    }
#endif
}
