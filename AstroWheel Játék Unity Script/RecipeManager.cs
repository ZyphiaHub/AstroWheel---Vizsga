using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Recipe {
    public string recipeName; // A recept neve
    public List<Ingredient> requiredIngredients; // Sz�ks�ges �sszetev�k (n�v�nyek vagy craftolt t�rgyak)
    public CraftedItem resultItem; // Az eredm�ny t�rgy
    public int resultQuantity; // Az eredm�ny mennyis�ge
}

[System.Serializable]
public class CraftedItem {
    public string craftedItemName; // A t�rgy neve
    public Sprite icon; // A t�rgy ikonja
    public string description; // A t�rgy le�r�sa
}
[System.Serializable]
public class Ingredient {
    public PlantDatabase.Item plantItem; // N�v�ny (opcion�lis)
    public CraftedItem craftedItem; // Craftolt t�rgy (opcion�lis)
    public int quantity; // Sz�ks�ges mennyis�g
}