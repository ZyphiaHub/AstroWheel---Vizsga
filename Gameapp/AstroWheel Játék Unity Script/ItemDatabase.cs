using UnityEngine;

[CreateAssetMenu(fileName = "ItemDatabase", menuName = "ScriptableObjects/ItemDatabase", order = 4)]
public class ItemDatabase : ScriptableObject {
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

    public Item[] items; 
}