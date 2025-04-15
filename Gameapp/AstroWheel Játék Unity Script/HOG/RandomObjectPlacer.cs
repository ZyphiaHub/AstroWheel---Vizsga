using UnityEngine;
using System.Collections.Generic;

public class RandomObjectPlacer : MonoBehaviour {
    [Header("Settings")]
    public GameObject[] hiddenObjectPrefabs;
    public int totalObjectsToPlace = 15; 
    public Vector2 spawnAreaMin = new Vector2(-5f, -4f);
    public Vector2 spawnAreaMax = new Vector2(9f, 4f);
    public float minDistanceBetweenObjects = 2f;

    [Header("References")]
    public RandomObjectSelector selector;

    private List<Vector2> usedPositions = new List<Vector2>();

    void Start()
    {
        PlaceRandomObjects();
    }
    //képek beválogatása és randomizálása
    public void PlaceRandomObjects()
    {
        usedPositions.Clear();
        List<GameObject> spawnedObjects = new List<GameObject>();

        if (hiddenObjectPrefabs.Length < totalObjectsToPlace)
        {
            Debug.LogError("Not enough unique prefabs!");
            return;
        }

        List<GameObject> shuffledPrefabs = new List<GameObject>(hiddenObjectPrefabs);
        Shuffle(shuffledPrefabs);

        for (int i = 0; i < totalObjectsToPlace; i++)
        {
            Vector2 randomPosition = GetRandomPosition();

            if (randomPosition != Vector2.negativeInfinity)
            {
                GameObject newObj = Instantiate(
                    shuffledPrefabs[i % shuffledPrefabs.Count],
                    randomPosition,
                    Quaternion.identity,
                    transform
                );

                newObj.name = $"HiddenObject_{i}";
                spawnedObjects.Add(newObj);
            }
        }

        selector.Initialize(spawnedObjects);
    }

    private Vector2 GetRandomPosition()
    {
        int maxAttempts = 150;
        for (int i = 0; i < maxAttempts; i++)
        {
            Vector2 position = new Vector2(
                Random.Range(spawnAreaMin.x, spawnAreaMax.x),
                Random.Range(spawnAreaMin.y, spawnAreaMax.y)
            );

            if (IsPositionValid(position))
            {
                usedPositions.Add(position);
                return position;
            }
        }
        return Vector2.negativeInfinity;
    }

    private bool IsPositionValid(Vector2 position)
    {
        foreach (Vector2 usedPos in usedPositions)
        {
            if (Vector2.Distance(position, usedPos) < minDistanceBetweenObjects)
                return false;
        }
        return true;
    }

    private void Shuffle(List<GameObject> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int randomIndex = Random.Range(i, list.Count);
            GameObject temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
    //képek megjelenésének területe
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Vector3 center = (spawnAreaMin + spawnAreaMax) / 2f;
        Vector3 size = new Vector3(spawnAreaMax.x - spawnAreaMin.x, spawnAreaMax.y - spawnAreaMin.y, 0.1f);
        Gizmos.DrawWireCube(center, size);
    }
}