using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public static class EnemySpawnSystem
{
    public static List<Transform> spawnPoints = new List<Transform>();
    public static EnemySpawnManager spawnManager;
    //public static int timesSpawned = 0;
    public static int enemyIndex;
    

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
            
                ObjectPool.instance.SpawnObject(enemyIndex);
            
        }
    }
    //spawns fixed amount at every spawn point
    public static void SpawnAmountAtEach(int amount, int index)
    {
        //timesSpawned = 0;
        //Debug.Log("enter SpawnAmountAtEach");
        foreach (Transform p in spawnPoints)
        {
            
                
                ObjectPool.instance.SpawnObject(p.position, index);
                
                

                /*Debug.Log("Enemy Index: " + enemyIndex);
                RaycastHit hit;
                if (Physics.Raycast(p.position, Vector3.down, out hit, 200f, LayerMask.GetMask("Ground")))
                {
                    Debug.Log("Hit position: " + hit.point);
                    GameObject temp = PoolManager.SpawnObject(Vector3.zero, enemyIndex);
                    bool isActive;
                    if(temp != null)
                    {
                        isActive = true;
                        Debug.Log("Is objectToSpawn active: " + isActive);
                    }
                    Debug.Log("object name: " + temp.name);
                
                    //spawn logic
                    NavMeshHit navHit;
                    if (NavMesh.SamplePosition(hit.point, out navHit, 20f, 1 << NavMesh.GetAreaFromName("Default")))
                    {
                        Debug.Log("found navMesh, warping agent: " + temp.name);
                        temp.GetComponent<NavMeshAgent>().Warp(navHit.position + Vector3.forward);
                    }
                }
                else
                {
                    Debug.Log("failed ray + " + p.name);
                }

                Debug.Log("spawning object at: " + enemyIndex);
                GameObject temp = PoolManager.SpawnObject(enemyIndex);
                if (temp == null)
                {
                    Debug.LogError("Object doesnt exist");
                    return;
                }
                Debug.Log("object name: " + temp.name);
                temp.GetComponent<NavMeshAgent>().Warp(p.position);

                Debug.Log("Finished");*/
            

            


        }
    }
    //spawns random amount at every spawn point
    public static void SpawnRangeAtEach(int min, int max)
    {
        foreach (Transform p in spawnPoints)
        {
              ObjectPool.instance.SpawnObject(enemyIndex);

            
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
            
                ObjectPool.instance.SpawnObject(enemyIndex);
            
        }
    }

    
}
