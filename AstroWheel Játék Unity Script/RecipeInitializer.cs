using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

/*public class RecipeInitializer : MonoBehaviour {
    public PlantDatabase plantDatabase; // A PlantDatabase referenciája (már betöltve)
    public CraftingSystem craftingSystem; // A CraftingSystem referenciája
    public CraftedItemDatabase craftedItemDatabase;

    void Start()
    {
        // Ellenõrizzük, hogy a PlantDatabase betöltõdött-e
        if (plantDatabase == null)
        {
            Debug.LogError("A PlantDatabase nincs beállítva!");
            return;
        }

        // Receptek létrehozása a PlantDatabase alapján
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

        // Receptek hozzáadása a CraftingSystem-hez
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
    recipeName = "Varázskristály Erõsítõ",
    requiredIngredients = new List<Ingredient> {
        new Ingredient {
            plantItem = plantDatabase.items[1], // Gyógyfû
            quantity = 2
        },
        new Ingredient {
            craftedItem = existingCraftedItems[0], // Varázskristály (craftolt tárgy)
            quantity = 1
        }
    },
    resultItem = new CraftedItem
    {
        craftedItemName = "Erõsített Varázskristály",
        icon = LoadSprite("Assets/Art/OtherMats/EnhancedCrystal.png"),
        description = "Egy erõsített varázskristály, amely nagyobb varázserõvel rendelkezik."
    },
    resultQuantity = 1
*/