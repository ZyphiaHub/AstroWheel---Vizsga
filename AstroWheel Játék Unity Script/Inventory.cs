using System.Collections.Generic;
using UnityEngine;

public class Inventory {

    public Dictionary<PlantDatabase.Item, int> items = new Dictionary<PlantDatabase.Item, int>();

    public void AddItem(PlantDatabase.Item item, int quantity)
    {
        if (items.ContainsKey(item))
        {
            // Ha már van ilyen tárgy az inventoryban, növeljük a mennyiséget
            items[item] += quantity;
        } else
        {
            // Ha nincs, hozzáadjuk az inventoryhoz
            items.Add(item, quantity);
        }

        //Debug.Log($"Item added: {item.englishName}, Quantity: {quantity}");
    }


    public void RemoveItem(PlantDatabase.Item item, int quantity)
    {
        if (items.ContainsKey(item))
        {
            // Csökkentjük a mennyiséget
            items[item] -= quantity;

            // Ha a mennyiség 0 vagy annál kisebb, eltávolítjuk a tárgyat
            if (items[item] <= 0)
            {
                items.Remove(item);
                Debug.Log($"{item.englishName} eltávolítva az inventoryból.");
            } else
            {
                Debug.Log($"{item.englishName} mennyisége csökkentve. Új mennyiség: {items[item]}");
            }
        } else
        {
            Debug.LogWarning($"{item.englishName} nem található az inventoryban.");
        }
    }

    // Inventory tartalmának kiírása (debug célokra)
    public void PrintInventory()
    {
        /*Debug.Log("Inventory contents:");
        foreach (var entry in items)
        {
            Debug.Log($"{entry.Key.englishName}: {entry.Value} db");
        }*/
    }
}

public class CraftedInventory {
    public Dictionary<ItemDatabase.Item, int> items = new Dictionary<ItemDatabase.Item, int>();

    public void AddItem(ItemDatabase.Item item, int quantity)
    {
        if (items.ContainsKey(item))
        {
            items[item] += quantity;
        } else
        {
            items.Add(item, quantity);
        }

        Debug.Log($"Tárgy hozzáadva: {item.itemName}, Mennyiség: {quantity}");
    }

    public void RemoveItem(ItemDatabase.Item item, int quantity)
    {
        if (items.ContainsKey(item))
        {
            // Csökkentjük a mennyiséget
            items[item] -= quantity;

            // Ha a mennyiség 0 vagy annál kisebb, eltávolítjuk a tárgyat
            if (items[item] <= 0)
            {
                items.Remove(item);
                Debug.Log($"{item.itemName} eltávolítva az inventoryból.");
            } else
            {
                Debug.Log($"{item.itemName} mennyisége csökkentve. Új mennyiség: {items[item]}");
            }
        } else
        {
            Debug.LogWarning($"{item.itemName} nem található az inventoryban.");
        }
    }

    // Inventory tartalmának kiírása (debug célokra)
    public void PrintInventory()
    {
        Debug.Log("Leltár tartalma:");
        foreach (var entry in items)
        {
            Debug.Log($"{entry.Key.itemName}: {entry.Value} db");
        }
    }

    // Tárgy keresése név alapján
    public ItemDatabase.Item FindItemByName(string itemName)
    {
        foreach (var item in items.Keys)
        {
            if (item.itemName == itemName)
            {
                return item;
            }
        }
        return null; 
    }


    public int GetItemQuantity(ItemDatabase.Item item)
    {
        if (items.ContainsKey(item))
        {
            return items[item];
        }
        return 0; 
    }
}