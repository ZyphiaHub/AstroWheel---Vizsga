using UnityEngine;

[CreateAssetMenu(fileName = "PlantDatabase", menuName = "ScriptableObjects/PlantDatabase", order = 1)]
public class PlantDatabase : ScriptableObject {
    [System.Serializable]
    public class Item {
        public int plantId;
        public string englishName; 
        public string witchName;
        public string latinName;
        public string description;
        public Sprite icon;
        public override bool Equals(object obj)
        {
            if (obj is Item other)
            {
                return englishName == other.englishName; // Azonosítás név alapján
            }
            return false;
        }

        public override int GetHashCode()
        {
            return englishName.GetHashCode();
        }
    }

    public Item[] items; // Tárgyak listája
}