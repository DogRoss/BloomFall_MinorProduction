using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    //spawn manager should have a list of spawn points, section by node
    [SerializeField] List<Transform> spawnPoints = new List<Transform>();
    [SerializeField] List<GameObject> enemyPrefabs = new List<GameObject>();
    int enemyIndex;
    public int enemiesToSpawn = 4;
    public float spawnTime = 15f;
    public bool spawn = true;
    EnemyStatus status;

    void Awake()
    {
        spawn = true;
        EnemySpawnSystem.enemyIndex = enemyIndex;
        
    }

    void Update()
    {
        EnemySpawnSystem.spawnPoints = spawnPoints;
        EnemySpawnSystem.spawnManager = this;




        if (spawn == true)
        {
            
            enemyIndex = Random.Range(0, enemyPrefabs.Count);
            
            enemyIndex = ObjectPool.instance.FindIndexOfObject(enemyPrefabs[enemyIndex]);
            foreach (Transform s in spawnPoints)
            {
                status = s.GetComponent<EnemyStatus>();
                if (status.canSpawn)
                {
                    Debug.Log("boom");
                    if (status.timesSpawned < enemiesToSpawn)
                    {

                        ObjectPool.instance.SpawnObject(s.position, enemyIndex);
                        status.timesSpawned++;
                    }
                        
                }
            }
            StartCoroutine(WaitToSpawn());

        }
    } 

    IEnumerator WaitToSpawn()
    {
        spawn = false;
        yield return new WaitForSeconds(spawnTime);
        
        
        spawn = true;
    }
}
