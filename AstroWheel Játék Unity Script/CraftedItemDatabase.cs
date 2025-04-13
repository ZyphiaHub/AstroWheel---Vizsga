using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CraftedItemDatabase", menuName = "ScriptableObjects/CraftedItemDatabase", order = 3)]
public class CraftedItemDatabase : ScriptableObject {

    [System.Serializable]
    public class Item {
        public int itemId;
        public string itemName;
        public string description;
        public Sprite icon;
        public override bool Equals(object obj)
        {
            if (obj is Item other)
            {
                return itemName == other.itemName; 
            }
            return false;
        }

        public override int GetHashCode()
        {
            return itemName.GetHashCode();
        }
    }
    public CraftedItem[] items; // Az összes craftolt tárgy
}