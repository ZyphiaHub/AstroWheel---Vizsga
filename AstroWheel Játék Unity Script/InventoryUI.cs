using UnityEngine;
using UnityEngine.UI;
using TMPro; // TextMeshPro n�vt�r import�l�sa

public class InventoryUI : MonoBehaviour {
    [Header("Plant Inventory References")]
    public PlantDatabase plantDatabase; // ItemDatabase referenci�ja
    public GameObject plantSlotPrefab;     // Slot prefab referenci�ja
    public Transform plantPanelParent;

    [Header("Crafted Inventory References")]
    public ItemDatabase itemDatabase;   // ItemDatabase referenci�ja
    public GameObject craftedSlotPrefab; 
    public Transform craftedPanelParent;

    [Header("Crafting UI References")]
    public GameObject craftPanel;       
    public Transform recipeListParent;  
    public GameObject recipeButtonPrefab; 
    public TextMeshProUGUI ingredientText; 
    public Button craftButton;          

    private CraftingRecipe selectedRecipe; // Kiv�lasztott recept


    private void Start()
    {
        
        // Friss�tj�k az UI-t az inventory tartalma alapj�n
        RefreshInventoryUI();
        InitializeCraftPanel();
    }

    private void RefreshInventoryUI()
    {
        // T�r�lj�k a kor�bbi slotokat a plant inventoryb�l
        foreach (Transform child in plantPanelParent)
        {
            Destroy(child.gameObject);
        }

        // L�trehozzuk a slotokat a plant inventory tartalma alapj�n
        foreach (var entry in InventoryManager.Instance.inventory.items)
        {
            CreateSlot(entry.Key.icon, entry.Key.englishName, entry.Value.ToString(), plantSlotPrefab, plantPanelParent);
        }

        // T�r�lj�k a kor�bbi slotokat a crafted inventoryb�l
        foreach (Transform child in craftedPanelParent)
        {
            Destroy(child.gameObject);
        }

        // L�trehozzuk a slotokat a crafted inventory tartalma alapj�n
        foreach (var entry in InventoryManager.Instance.craftedInventory.items)
        {
            CreateSlot(entry.Key.icon, entry.Key.itemName, entry.Value.ToString(), craftedSlotPrefab, craftedPanelParent);
        }
    }

    // Egy �j slot l�trehoz�sa �s be�ll�t�sa
    private void CreateSlot(Sprite icon, string itemName, string quantity, GameObject slotPrefab, Transform parent)
    {
        // L�trehozunk egy �j slotot
        GameObject slot = Instantiate(slotPrefab, parent);

        // Be�ll�tjuk az ikont
        Image iconImage = slot.transform.Find("Icon").GetComponent<Image>();
        if (iconImage != null)
        {
            iconImage.sprite = icon;
        } else
        {
            Debug.LogError("Icon object not found in slot prefab!");
        }

        // Be�ll�tjuk a t�rgy nev�t (TMP komponens haszn�lata)
        Transform nameTransform = slot.transform.Find("Label/Name");
        if (nameTransform != null)
        {
            TextMeshProUGUI nameLabel = nameTransform.GetComponent<TextMeshProUGUI>();
            if (nameLabel != null)
            {
                nameLabel.text = itemName;
            } else
            {
                Debug.LogError("TextMeshProUGUI component not found on Name object!");
            }
        } else
        {
            Debug.LogError("Name object not found in slot prefab!");
        }

        // Be�ll�tjuk a mennyis�get (TMP komponens haszn�lata)
        Transform stackTransform = slot.transform.Find("StackBg/Stack");
        if (stackTransform != null)
        {
            TextMeshProUGUI quantityText = stackTransform.GetComponent<TextMeshProUGUI>();
            if (quantityText != null)
            {
                quantityText.text = quantity;
            } else
            {
                Debug.LogError("TextMeshProUGUI component not found on Stack object!");
            }
        } else
        {
            Debug.LogError("Stack object not found in slot prefab!");
        }
    }

    /*InventoryUI.Instance.RefreshInventoryUI(); // Friss�ti az UI-t*/



    // Craft panel inicializ�l�sa
    private void InitializeCraftPanel()
    {
        // Ellen�rizz�k, hogy l�tezik-e a recipeListParent
        if (recipeListParent == null)
        {
            Debug.LogError("recipeListParent nincs be�ll�tva!");
            return;
        }
        // T�r�lj�k a kor�bbi recept gombokat
        foreach (Transform child in recipeListParent)
        {
            Destroy(child.gameObject);
        }
        if (InventoryManager.Instance.craftingRecipe == null || InventoryManager.Instance.craftingRecipe.Count == 0)
        {
            Debug.LogWarning("A craftingRecipe lista �res vagy null!");
            return;
        }

        Debug.Log($"Receptek sz�ma: {InventoryManager.Instance.craftingRecipe.Count}");

        // L�trehozzuk a recept gombokat
        foreach (var recipe in InventoryManager.Instance.craftingRecipe)
        {
            if (recipe == null || recipe.outputItem == null)
            {
                Debug.LogWarning("Hib�s recept tal�lhat� a list�ban!");
                continue;
            }
            GameObject recipeButton = Instantiate(recipeButtonPrefab, recipeListParent);

            LayoutElement layoutElement = recipeButton.AddComponent<LayoutElement>();
            layoutElement.minHeight = 50;

            if (recipeButton == null)
            {
                Debug.LogError("Nem siker�lt l�trehozni a recept gombot!");
                continue;
            }

            TextMeshProUGUI buttonText = recipeButton.GetComponentInChildren<TextMeshProUGUI>();
            if (buttonText != null)
            {
                buttonText.text = recipe.outputItem.itemName; 
            } else
            {
                Debug.LogError("TextMeshProUGUI komponens nem tal�lhat� a recept gombon!");
            }

            // Gomb esem�nykezel�je
            Button button = recipeButton.GetComponent<Button>();
            if (button != null)
            {
                button.onClick.AddListener(() => SelectRecipe(recipe));
            } else
            {
                Debug.LogError("Button komponens nem tal�lhat� a recept gombon!");
            }
            button.onClick.AddListener(() => SelectRecipe(recipe));
        }

        // Friss�tsd a layoutot
        LayoutRebuilder.ForceRebuildLayoutImmediate(recipeListParent.GetComponent<RectTransform>());

        // Craft gomb esem�nykezel�je
        if (craftButton != null)
        {
            craftButton.onClick.AddListener(CraftSelectedRecipe);
        } else
        {
            Debug.LogError("Craft gomb nincs be�ll�tva!");
        }
    }

    // Recept kiv�laszt�sa
    private void SelectRecipe(CraftingRecipe recipe)
    {
        selectedRecipe = recipe;
        UpdateIngredientText(recipe);
    }

    // Alapanyagok sz�veg�nek friss�t�se
    private void UpdateIngredientText(CraftingRecipe recipe)
    {
        string ingredientList = "Ingredients needed:\n";
        foreach (var ingredient in recipe.ingredients)
        {
            if (ingredient.plantItem != null) // N�v�ny alapanyag
            {
                int availableQuantity = InventoryManager.Instance.inventory.items.ContainsKey(ingredient.plantItem)
                    ? InventoryManager.Instance.inventory.items[ingredient.plantItem]
                    : 0;
                ingredientList += $"{ingredient.plantItem.englishName}: {ingredient.quantity} (You have: {availableQuantity})\n";
            } else if (ingredient.craftedItem != null) // Crafted item alapanyag
            {
                int availableQuantity = InventoryManager.Instance.craftedInventory.items.ContainsKey(ingredient.craftedItem)
                    ? InventoryManager.Instance.craftedInventory.items[ingredient.craftedItem]
                    : 0;
                ingredientList += $"{ingredient.craftedItem.itemName}: {ingredient.quantity} (You have: {availableQuantity})\n";
            }
        }
        ingredientText.text = ingredientList;
    }

    // Craftol�s ind�t�sa
    private void CraftSelectedRecipe()
    {
        if (selectedRecipe != null)
        {
            bool success = InventoryManager.Instance.CraftItem(selectedRecipe);
            if (success)
            {
                Debug.Log("Craftol�s sikeres!");
                RefreshInventoryUI(); 
                UpdateIngredientText(selectedRecipe); 
            } else
            {
                Debug.Log("Crafting is not possible: not enough ingredients.");
            }
        } else
        {
            Debug.LogWarning("Nincs kiv�lasztva recept!");
        }
    }

    


}