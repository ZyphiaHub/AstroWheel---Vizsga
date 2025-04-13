using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CraftingRecipe {
    [System.Serializable]
    public class Ingredient {
        public PlantDatabase.Item plantItem; 
        public ItemDatabase.Item craftedItem; 
        public int quantity; // Sz�ks�ges mennyis�g
    }

    public List<Ingredient> ingredients; // Sz�ks�ges alapanyagok
    public ItemDatabase.Item outputItem; // Kimeneti crafted item
    public int outputQuantity; // Kimeneti mennyis�g
}