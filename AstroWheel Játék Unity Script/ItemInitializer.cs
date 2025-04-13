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
            Debug.LogError("ItemDatabase nincs beállítva!");
            return;
        }

        // Tárgyak inicializálása
        itemDatabase.items = new ItemDatabase.Item[]
        {
            new ItemDatabase.Item
            {
                itemName = "Egy",
                description = "A sharp blade for close combat.",
                icon = LoadSprite("Assets/Art/Items/Avarvirág.png") // Példa elérési út
            },
            new ItemDatabase.Item
            {
                itemName = "Shield",
                description = "Protects against enemy attacks.",
                icon = LoadSprite("Assets/Art/Items/Korpafû.png") // Példa elérési út
            },
            new ItemDatabase.Item
            {
                itemName = "Három",
                description = "Protects against enemy attacks.",
                icon = LoadSprite("Assets/Art/Items/Korpafû.png") // Példa elérési út
            },
            new ItemDatabase.Item
            {
                itemName = "Shield",
                description = "Protects against enemy attacks.",
                icon = LoadSprite("Assets/Art/Items/Korpafû.png") // Példa elérési út
            },
            new ItemDatabase.Item
            {
                itemName = "Öt",
                description = "Protects against enemy attacks.",
                icon = LoadSprite("Assets/Art/Items/Korpafû.png") // Példa elérési út
            },
            new ItemDatabase.Item
            {
                itemName = "Shield",
                description = "Protects against enemy attacks.",
                icon = LoadSprite("Assets/Art/Items/Korpafû.png") // Példa elérési út
            },
            new ItemDatabase.Item
            {
                itemName = "Shield",
                description = "Protects against enemy attacks.",
                icon = LoadSprite("Assets/Art/Items/Korpafû.png") // Példa elérési út
            },
            new ItemDatabase.Item
            {
                itemName = "Potion",
                description = "Restores 20 HP.",
                icon = LoadSprite("Assets/Art/Items/Tarlóhere.png") // Példa elérési út
            }
        };

        // Mentjük a változtatásokat
        EditorUtility.SetDirty(itemDatabase);
        AssetDatabase.SaveAssets();
        Debug.Log("ItemDatabase inicializálva és mentve!");
    }

    // Sprite betöltése az elérési út alapján (csak Editorban)
    private Sprite LoadSprite(string path)
    {
        return AssetDatabase.LoadAssetAtPath<Sprite>(path);
    }
#endif
}
