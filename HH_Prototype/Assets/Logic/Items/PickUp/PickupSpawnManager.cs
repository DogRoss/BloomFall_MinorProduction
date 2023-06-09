using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupSpawnManager : MonoBehaviour
{
    //spawn manager should have a list of spawn points, section by node
    [SerializeField] List<Transform> spawnPoints = new List<Transform>();
    public int pickupsToSpawn = 4;
    public int index = 0;
    bool spawn = true;
    

    void Awake()
    {
        PickupSpawnSystem.index = index;
    }

    void Update()
    {
        PickupSpawnSystem.spawnPoints = spawnPoints;
        PickupSpawnSystem.spawnManager = this;
        if (spawn == true)
        {
            PickupSpawnSystem.SpawnAmountAtEach(pickupsToSpawn);
            spawn = false;
        }


    }

    
}
