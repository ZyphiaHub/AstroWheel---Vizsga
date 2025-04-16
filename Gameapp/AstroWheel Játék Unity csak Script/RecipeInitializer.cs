using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

/*public class RecipeInitializer : MonoBehaviour {
    public PlantDatabase plantDatabase; // A PlantDatabase referenci�ja (m�r bet�ltve)
    public CraftingSystem craftingSystem; // A CraftingSystem referenci�ja
    public CraftedItemDatabase craftedItemDatabase;

    void Start()
    {
        // Ellen�rizz�k, hogy a PlantDatabase bet�lt�d�tt-e
        if (plantDatabase == null)
        {
            Debug.LogError("A PlantDatabase nincs be�ll�tva!");
            return;
        }

        // Receptek l�trehoz�sa a PlantDatabase alapj�n
        List<Recipe> recipes = new List<Recipe> {
            new Recipe {
                recipeName = "Cross-stone",
                requiredIngredients = new List<Ingredient> {
                    new Ingredient { 
                        plantItem = plantDatabase.items[1],
                        quantity = 1
                        },
                    new Ingredient {
                        plantItem = plantDatabase.items[5],
                        quantity = 1
                        },
                    new Ingredient {
                        plantItem = plantDatabase.items[9],
                        quantity = 1
                        },
                    new Ingredient {
                        plantItem = plantDatabase.items[13],
                        quantity = 1
                        }
                    
                },
                 
                resultItem = new CraftedItem {
                    craftedItemName = "Cross-stone",
                    icon =  LoadSprite("Assets/Art/OtherMats/Cross-stone.png"),
                    description = "It protects against malice and curses. It transforms conflict into harmony. Represents the earth element."
                },
                resultQuantity = 1 
            },

            new Recipe {
                recipeName = "Cross-stone",
                requiredIngredients = new List<Ingredient> {
                    new Ingredient {
                        plantItem = plantDatabase.items[12],
                        quantity = 1
                        },
                    new Ingredient {
                        craftedItem = craftedItemDatabase.craftedItems[0], 
                        quantity = 1
                        }

                },

                resultItem = new CraftedItem {
                    craftedItemName = "Cross-stone",
                    icon =  LoadSprite("Assets/Art/OtherMats/Cross-stone.png"),
                    description = "It protects against malice and curses. It transforms conflict into harmony. Represents the earth element."
                },
                resultQuantity = 1
            },

        };

        // Receptek hozz�ad�sa a CraftingSystem-hez
        craftingSystem.recipes = recipes;
    }

    private Sprite LoadSprite(string path)
    {
#if UNITY_EDITOR
        return AssetDatabase.LoadAssetAtPath<Sprite>(path);
#else
        return null;
#endif
    }
}

/*
new Recipe
{
    recipeName = "Var�zskrist�ly Er�s�t�",
    requiredIngredients = new List<Ingredient> {
        new Ingredient {
            plantItem = plantDatabase.items[1], // Gy�gyf�
            quantity = 2
        },
        new Ingredient {
            craftedItem = existingCraftedItems[0], // Var�zskrist�ly (craftolt t�rgy)
            quantity = 1
        }
    },
    resultItem = new CraftedItem
    {
        craftedItemName = "Er�s�tett Var�zskrist�ly",
        icon = LoadSprite("Assets/Art/OtherMats/EnhancedCrystal.png"),
        description = "Egy er�s�tett var�zskrist�ly, amely nagyobb var�zser�vel rendelkezik."
    },
    resultQuantity = 1
*/