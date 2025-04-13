using System.Collections.Generic;
using UnityEngine;

public class RandomObjectSelector : MonoBehaviour {
    public int clickableObjectCount = 10;
    [SerializeField] private HOGManager manager;

    public void Initialize(List<GameObject> allObjects)
    {
        
        List<HiddenObject> targets = new List<HiddenObject>();
        List<HiddenObject> availableObjects = new List<HiddenObject>();
        manager.SetTargetObjects(targets);

        // Összes elérhetõ objektum gyûjtése
        foreach (GameObject obj in allObjects)
        {
            HiddenObject hiddenObj = obj.GetComponent<HiddenObject>();
            if (hiddenObj != null)
            {
                availableObjects.Add(hiddenObj);
                hiddenObj.SetClickable(false); // Alapból mindenki nem kattintható
            }
        }

        // Véletlenszerû kiválasztás
        while (targets.Count < clickableObjectCount && availableObjects.Count > 0)
        {
            int randomIndex = Random.Range(0, availableObjects.Count);
            HiddenObject selected = availableObjects[randomIndex];
            targets.Add(selected);
            availableObjects.RemoveAt(randomIndex);
        }

        // Beállítások alkalmazása
        foreach (HiddenObject obj in targets)
        {
            obj.SetClickable(true);
        }

        // A nem kiválasztottak maradjanak nem kattinthatóak
        foreach (HiddenObject obj in availableObjects)
        {
            obj.SetClickable(false);
        }

        //HOGManager.Instance.SetTargetObjects(targets);
        manager.SetTargetObjects(targets);
    }
}