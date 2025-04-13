using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Recipe {
    public string recipeName; // A recept neve
    public List<Ingredient> requiredIngredients; // Szükséges összetevõk (növények vagy craftolt tárgyak)
    public CraftedItem resultItem; // Az eredmény tárgy
    public int resultQuantity; // Az eredmény mennyisége
}

[System.Serializable]
public class CraftedItem {
    public string craftedItemName; // A tárgy neve
    public Sprite icon; // A tárgy ikonja
    public string description; // A tárgy leírása
}
[System.Serializable]
public class Ingredient {
    public PlantDatabase.Item plantItem; // Növény (opcionális)
    public CraftedItem craftedItem; // Craftolt tárgy (opcionális)
    public int quantity; // Szükséges mennyiség
}