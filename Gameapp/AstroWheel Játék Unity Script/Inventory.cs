using System.Collections.Generic;
using UnityEngine;

public class Inventory {

    public Dictionary<PlantDatabase.Item, int> items = new Dictionary<PlantDatabase.Item, int>();

    public void AddItem(PlantDatabase.Item item, int quantity)
    {
        if (items.ContainsKey(item))
        {
            // Ha m�r van ilyen t�rgy az inventoryban, n�velj�k a mennyis�get
            items[item] += quantity;
        } else
        {
            // Ha nincs, hozz�adjuk az inventoryhoz
            items.Add(item, quantity);
        }

        //Debug.Log($"Item added: {item.englishName}, Quantity: {quantity}");
    }


    public void RemoveItem(PlantDatabase.Item item, int quantity)
    {
        if (items.ContainsKey(item))
        {
            // Cs�kkentj�k a mennyis�get
            items[item] -= quantity;

            // Ha a mennyis�g 0 vagy ann�l kisebb, elt�vol�tjuk a t�rgyat
            if (items[item] <= 0)
            {
                items.Remove(item);
                Debug.Log($"{item.englishName} elt�vol�tva az inventoryb�l.");
            } else
            {
                Debug.Log($"{item.englishName} mennyis�ge cs�kkentve. �j mennyis�g: {items[item]}");
            }
        } else
        {
            Debug.LogWarning($"{item.englishName} nem tal�lhat� az inventoryban.");
        }
    }

    // Inventory tartalm�nak ki�r�sa (debug c�lokra)
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

        Debug.Log($"T�rgy hozz�adva: {item.itemName}, Mennyis�g: {quantity}");
    }

    public void RemoveItem(ItemDatabase.Item item, int quantity)
    {
        if (items.ContainsKey(item))
        {
            // Cs�kkentj�k a mennyis�get
            items[item] -= quantity;

            // Ha a mennyis�g 0 vagy ann�l kisebb, elt�vol�tjuk a t�rgyat
            if (items[item] <= 0)
            {
                items.Remove(item);
                Debug.Log($"{item.itemName} elt�vol�tva az inventoryb�l.");
            } else
            {
                Debug.Log($"{item.itemName} mennyis�ge cs�kkentve. �j mennyis�g: {items[item]}");
            }
        } else
        {
            Debug.LogWarning($"{item.itemName} nem tal�lhat� az inventoryban.");
        }
    }

    // Inventory tartalm�nak ki�r�sa (debug c�lokra)
    public void PrintInventory()
    {
        Debug.Log("Lelt�r tartalma:");
        foreach (var entry in items)
        {
            Debug.Log($"{entry.Key.itemName}: {entry.Value} db");
        }
    }

    // T�rgy keres�se n�v alapj�n
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