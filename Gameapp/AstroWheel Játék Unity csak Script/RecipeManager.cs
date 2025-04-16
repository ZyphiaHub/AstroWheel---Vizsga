using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Recipe {
    public string recipeName; 
    public List<Ingredient> requiredIngredients; 
    public CraftedItem resultItem; 
    public int resultQuantity; 
}

[System.Serializable]
public class CraftedItem {
    public string craftedItemName; 
    public Sprite icon; 
    public string description; 
}
[System.Serializable]
public class Ingredient {
    public PlantDatabase.Item plantItem; 
    public CraftedItem craftedItem; 
    public int quantity; 
}