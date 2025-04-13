using UnityEngine;
using UnityEngine.UI;
using TMPro; // TextMeshPro névtér importálása

public class InventoryUI : MonoBehaviour {
    [Header("Plant Inventory References")]
    public PlantDatabase plantDatabase; // ItemDatabase referenciája
    public GameObject plantSlotPrefab;     // Slot prefab referenciája
    public Transform plantPanelParent;

    [Header("Crafted Inventory References")]
    public ItemDatabase itemDatabase;   // ItemDatabase referenciája
    public GameObject craftedSlotPrefab; 
    public Transform craftedPanelParent;

    [Header("Crafting UI References")]
    public GameObject craftPanel;       
    public Transform recipeListParent;  
    public GameObject recipeButtonPrefab; 
    public TextMeshProUGUI ingredientText; 
    public Button craftButton;          

    private CraftingRecipe selectedRecipe; // Kiválasztott recept


    private void Start()
    {
        
        // Frissítjük az UI-t az inventory tartalma alapján
        RefreshInventoryUI();
        InitializeCraftPanel();
    }

    private void RefreshInventoryUI()
    {
        // Töröljük a korábbi slotokat a plant inventorybõl
        foreach (Transform child in plantPanelParent)
        {
            Destroy(child.gameObject);
        }

        // Létrehozzuk a slotokat a plant inventory tartalma alapján
        foreach (var entry in InventoryManager.Instance.inventory.items)
        {
            CreateSlot(entry.Key.icon, entry.Key.englishName, entry.Value.ToString(), plantSlotPrefab, plantPanelParent);
        }

        // Töröljük a korábbi slotokat a crafted inventorybõl
        foreach (Transform child in craftedPanelParent)
        {
            Destroy(child.gameObject);
        }

        // Létrehozzuk a slotokat a crafted inventory tartalma alapján
        foreach (var entry in InventoryManager.Instance.craftedInventory.items)
        {
            CreateSlot(entry.Key.icon, entry.Key.itemName, entry.Value.ToString(), craftedSlotPrefab, craftedPanelParent);
        }
    }

    // Egy új slot létrehozása és beállítása
    private void CreateSlot(Sprite icon, string itemName, string quantity, GameObject slotPrefab, Transform parent)
    {
        // Létrehozunk egy új slotot
        GameObject slot = Instantiate(slotPrefab, parent);

        // Beállítjuk az ikont
        Image iconImage = slot.transform.Find("Icon").GetComponent<Image>();
        if (iconImage != null)
        {
            iconImage.sprite = icon;
        } else
        {
            Debug.LogError("Icon object not found in slot prefab!");
        }

        // Beállítjuk a tárgy nevét (TMP komponens használata)
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

        // Beállítjuk a mennyiséget (TMP komponens használata)
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

    /*InventoryUI.Instance.RefreshInventoryUI(); // Frissíti az UI-t*/



    // Craft panel inicializálása
    private void InitializeCraftPanel()
    {
        // Ellenõrizzük, hogy létezik-e a recipeListParent
        if (recipeListParent == null)
        {
            Debug.LogError("recipeListParent nincs beállítva!");
            return;
        }
        // Töröljük a korábbi recept gombokat
        foreach (Transform child in recipeListParent)
        {
            Destroy(child.gameObject);
        }
        if (InventoryManager.Instance.craftingRecipe == null || InventoryManager.Instance.craftingRecipe.Count == 0)
        {
            Debug.LogWarning("A craftingRecipe lista üres vagy null!");
            return;
        }

        Debug.Log($"Receptek száma: {InventoryManager.Instance.craftingRecipe.Count}");

        // Létrehozzuk a recept gombokat
        foreach (var recipe in InventoryManager.Instance.craftingRecipe)
        {
            if (recipe == null || recipe.outputItem == null)
            {
                Debug.LogWarning("Hibás recept található a listában!");
                continue;
            }
            GameObject recipeButton = Instantiate(recipeButtonPrefab, recipeListParent);

            LayoutElement layoutElement = recipeButton.AddComponent<LayoutElement>();
            layoutElement.minHeight = 50;

            if (recipeButton == null)
            {
                Debug.LogError("Nem sikerült létrehozni a recept gombot!");
                continue;
            }

            TextMeshProUGUI buttonText = recipeButton.GetComponentInChildren<TextMeshProUGUI>();
            if (buttonText != null)
            {
                buttonText.text = recipe.outputItem.itemName; 
            } else
            {
                Debug.LogError("TextMeshProUGUI komponens nem található a recept gombon!");
            }

            // Gomb eseménykezelõje
            Button button = recipeButton.GetComponent<Button>();
            if (button != null)
            {
                button.onClick.AddListener(() => SelectRecipe(recipe));
            } else
            {
                Debug.LogError("Button komponens nem található a recept gombon!");
            }
            button.onClick.AddListener(() => SelectRecipe(recipe));
        }

        // Frissítsd a layoutot
        LayoutRebuilder.ForceRebuildLayoutImmediate(recipeListParent.GetComponent<RectTransform>());

        // Craft gomb eseménykezelõje
        if (craftButton != null)
        {
            craftButton.onClick.AddListener(CraftSelectedRecipe);
        } else
        {
            Debug.LogError("Craft gomb nincs beállítva!");
        }
    }

    // Recept kiválasztása
    private void SelectRecipe(CraftingRecipe recipe)
    {
        selectedRecipe = recipe;
        UpdateIngredientText(recipe);
    }

    // Alapanyagok szövegének frissítése
    private void UpdateIngredientText(CraftingRecipe recipe)
    {
        string ingredientList = "Ingredients needed:\n";
        foreach (var ingredient in recipe.ingredients)
        {
            if (ingredient.plantItem != null) // Növény alapanyag
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

    // Craftolás indítása
    private void CraftSelectedRecipe()
    {
        if (selectedRecipe != null)
        {
            bool success = InventoryManager.Instance.CraftItem(selectedRecipe);
            if (success)
            {
                Debug.Log("Craftolás sikeres!");
                RefreshInventoryUI(); 
                UpdateIngredientText(selectedRecipe); 
            } else
            {
                Debug.Log("Crafting is not possible: not enough ingredients.");
            }
        } else
        {
            Debug.LogWarning("Nincs kiválasztva recept!");
        }
    }

    


}