using System.Collections.Generic;


[System.Serializable]
public class CraftingRecipe {
    [System.Serializable]
    public class Ingredient {
        public PlantDatabase.Item plantItem; 
        public ItemDatabase.Item craftedItem; 
        public int quantity; 
    }

    public List<Ingredient> ingredients; 
    public ItemDatabase.Item outputItem; 
    public int outputQuantity; 
}