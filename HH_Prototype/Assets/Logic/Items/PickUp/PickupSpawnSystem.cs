using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PickupSpawnSystem
{
    public static List<Transform> spawnPoints = new List<Transform>();
    public static PickupSpawnManager spawnManager;
    public static int index;
    

    //spawns fixed amount at random number of spawn points
    public static void SpawnAmountRandomPoints(int amount, int amountOfSpawnPoints)
    {
        List<Transform> randomSpawnPoints = spawnPoints;

        for (int s = 0; s < amountOfSpawnPoints; s++)
        {
            randomSpawnPoints[s] = spawnPoints[Random.Range(0, spawnPoints.Count)];

        }

        foreach (Transform p in randomSpawnPoints)
        {
            for (int i = 0; i <= amount; i++)
            {
                ObjectPool.instance.SpawnObject(new Vector3(Random.Range(p.position.x - 5, p.position.x + 5), p.position.y, Random.Range(p.position.z - 5, p.position.z + 5)), index);
            }
        }
    }
    //spawns fixed amount at every spawn point
    public static void SpawnAmountAtEach(int amount)
    {

        foreach (Transform p in spawnPoints)
        {
            for (int i = 0; i <= amount; i++)
            {
                ObjectPool.instance.SpawnObject(new Vector3(Random.Range(p.position.x - 5, p.position.x + 5), p.position.y, Random.Range(p.position.z - 5, p.position.z + 5)), index);
            }

        }
    }
    //spawns random amount at every spawn point
    public static void SpawnRangeAtEach(int min, int max)
    {
        foreach (Transform p in spawnPoints)
        {
            for (int i = min; i <= Random.Range(min, max); i++)
            {
                ObjectPool.instance.SpawnObject(new Vector3(Random.Range(p.position.x - 5, p.position.x + 5), p.position.y, Random.Range(p.position.z - 5, p.position.z + 5)), index);

            }
        }
    }

    public static void SpawnRangeRandomPoints(int min, int max, int amountOfSpawnPoints)
    {
        List<Transform> randomSpawnPoints = spawnPoints;

        for (int s = 0; s < amountOfSpawnPoints; s++)
        {
            randomSpawnPoints[s] = spawnPoints[Random.Range(0, spawnPoints.Count)];

        }

        foreach (Transform p in randomSpawnPoints)
        {
            for (int i = min; i <= Random.Range(min, max); i++)
            {
                ObjectPool.instance.SpawnObject(new Vector3(Random.Range(p.position.x - 5, p.position.x + 5), p.position.y, Random.Range(p.position.z - 5, p.position.z + 5)), index);
            }
        }
    }


}

