using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingObjectSpawner : MonoBehaviour
{
    public GameObject[] fallingObjectPrefabs;

    public Transform[] spawnPositions;
    public float spawnInterval = 2f;

    public float fallingSpeed = 2f;

    private bool isPaused = false;
    private GameManager gameManager;

    void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();
        StartCoroutine(SpawnObjects());
    }

    private IEnumerator SpawnObjects()
    {
        while (true)
        {
            if (!isPaused && gameManager.GetCurrentState() == GameManager.GameState.Playing)
            {
                SpawnObject();
            }
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnObject()
    {
        if (fallingObjectPrefabs == null || fallingObjectPrefabs.Length == 0)
            return;

        int positionIndex = Random.Range(0, spawnPositions.Length);
        int prefabIndex = Random.Range(0, fallingObjectPrefabs.Length);

        float fallingSpeed = 2f;

        GameObject obj = Instantiate(fallingObjectPrefabs[prefabIndex], spawnPositions[positionIndex].position, Quaternion.identity);
        FallingObject fallingObj = obj.GetComponent<FallingObject>();

        if (fallingObj != null)
        {
            fallingObj.Initialize(positionIndex + 1, fallingSpeed);
        }
    }

    public void Pause()
    {
        isPaused = true;

        FallingObject[] objects = FindObjectsByType<FallingObject>(FindObjectsSortMode.None);
        foreach (FallingObject obj in objects)
        {
            obj.Pause();
        }
    }

    public void Resume()
    {
        isPaused = false;

        FallingObject[] objects = FindObjectsByType<FallingObject>(FindObjectsSortMode.None);
        foreach (FallingObject obj in objects)
        {
            obj.Resume();
        }
    }
}