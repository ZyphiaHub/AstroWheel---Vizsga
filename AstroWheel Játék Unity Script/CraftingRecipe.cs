using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CraftingRecipe {
    [System.Serializable]
    public class Ingredient {
        public PlantDatabase.Item plantItem; 
        public ItemDatabase.Item craftedItem; 
        public int quantity; // Szükséges mennyiség
    }

    public List<Ingredient> ingredients; // Szükséges alapanyagok
    public ItemDatabase.Item outputItem; // Kimeneti crafted item
    public int outputQuantity; // Kimeneti mennyiség
}