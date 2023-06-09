using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*!
* Object pool class that can create multiple pools of different game objects.
* Simply serialize each prefab in the editor, and a separate pool will be created for each object
* Note the index of each prefab need to be noted, in order to spawn the correct prefab on each call.
* For example, if you have 3 different types of objects being pooled, and you want to spawn the 2nd
* type, you would call SpawnObject(1), to call the pool at index 1.
*/
public class ObjectPool : MonoBehaviour
{
    [SerializeField] GameObject[] pooledObjectPrefabs; //!< different prefabs of all of the objects to create pools of
    [SerializeField] int poolSize; //!< amount of items to instantiate in each pool

    [SerializeField] List<GameObject[]> pools = new List<GameObject[]>(); //!< collection of pools. each pool has a set collection of gameobjects to pull from
    public static ObjectPool instance;
    EnemyMovement movement;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        int n = 0;
        for (int i = 0; i < pooledObjectPrefabs.Length; i++)
        {
            pools.Add(new GameObject[poolSize]);
            for (int j = 0; j < poolSize; j++)
            {
                Debug.Log("pool index: " + i);
                pools[i][j] = Instantiate(pooledObjectPrefabs[i]);
                n++;
                pools[i][j].name += n.ToString();
                pools[i][j].gameObject.SetActive(false);
            }
        }

        n = 0;

        for (int i = 0; i < pools.Count; i++)
        {
            for(int j = 0; j < pools[i].Length; j++)
            {
                n++;
                Debug.Log(pools[i][j].name + n);
            }
        }
    }

    //public void SetObjectPool(List<GameObject[]> _Pools, GameObject[] _PooledObjectPrefabs, int _PoolSize)
    //{

    //    Debug.Log("set first pooledOb jPrefab");
    //    pooledObjectPrefabs = _PooledObjectPrefabs;
    //    Debug.Log("set first size");
    //    poolSize = _PoolSize;
    //    Debug.Log("set first pool");
    //    pools = _Pools; */
    //}

    public bool ObjectExistsInPool(GameObject o)
    {
        for (int i = 0; i < pooledObjectPrefabs.Length - 1; i++)
        {
            if (o == pooledObjectPrefabs[i])
            {
                return true;
            }
        }

        return false;
    }

    //should only be used if bool check passes
    public int FindIndexOfObject(GameObject o)
    {
        int value = 0;
        for (int i = 0; i < pooledObjectPrefabs.Length; i++)
        {
            if (o == pooledObjectPrefabs[i])
            {
                value = i;
                Debug.Log("value is: " + value);
                return value;
            }
        }

        return value;
    }

    public bool PoolHasReadyObject(int pooledObjectTypeIndex)
    {
        for (int i = 0; i < pools[pooledObjectTypeIndex].Length - 1; i++)
        {
            if (!pools[pooledObjectTypeIndex][i].activeSelf)
            {
                return true;
            }
        }

        return false;
    }

    public void SpawnObjectNoReturn(int pooledObjectTypeIndex)
    {
        SpawnObject(pooledObjectTypeIndex);
    }

    /*!
     * Spawns a new object from the pool, defaulted to the set of prefabs at index 0
     */
    public GameObject SpawnObject()
    {
        return SpawnObject(Vector3.zero, 0);
    }

    /*!
     * Spawns a new object from the pool, taken from the set of prefabs at passed index
     */
    public GameObject SpawnObject(int pooledObjectTypeIndex)
    {
        return SpawnObject(Vector3.zero, pooledObjectTypeIndex);
    }

    /*!
     * Spawns a new object from the pool at given position, defaulted to the set of prefabs at index 0
     */
    public GameObject SpawnObject(Vector3 position)
    {
        return SpawnObject(position, 0);
    }
    /*!
     * Spawns a new object from the pool at given position, taken from the set of prefabs at passed index
     */
    public GameObject SpawnObject(Vector3 position, int pooledObjectTypeIndex)
    {

        GameObject o = null;

        //Debug.Log(poolSize);
        for (int i = 0; i < poolSize - 1; i++)
        {
            //Debug.Log(o.name);
            //Debug.Log(pools.Count);
            if (!pools[pooledObjectTypeIndex][i].activeSelf)
            {
                o = pools[pooledObjectTypeIndex][i].gameObject;
                o.transform.position = position;
                o.SetActive(true);
                
                EnemyMovement temp;
                if(pools[pooledObjectTypeIndex][i].TryGetComponent(out temp))
                {
                    temp.startPos = position;
                }
                
                break;
            }
        }
        //Debug.Log(o.name);
        return o;
    }
    /*!
     * Recycles the passed game object back into its respective pool
     */
    public void Recycle(GameObject o)
    {
        for (int i = 0; i < pools.Count - 1; i++)
        {
            for (int j = 0; j < pools[i].Length - 1; j++)
            {
                if (pools[i][j] == o)
                {
                    pools[i][j].gameObject.SetActive(false);
                    return;
                }
            }
        }
    }
    /*!
     * Recycles the passed game object back into its respective pool. Passed index helps method run faster
     */
    public void Recycle(GameObject o, int pooledObjectTypeIndex)
    {
        for (int i = 0; i < pools[pooledObjectTypeIndex].Length - 1; i++)
        {
            if (pools[pooledObjectTypeIndex][i] == o)
            {
                pools[pooledObjectTypeIndex][i].gameObject.SetActive(false);
                break;
            }
        }
    }
    /*!
     * Recycles all objects in the first pool
     */
    public void RecycleAllInPool()
    {
        RecycleAllInPool(0);
    }
    /*!
     * Recycles all objects in the pool at given index
     */
    public void RecycleAllInPool(int pooledObjectTypeIndex)
    {
        foreach (GameObject o in pools[pooledObjectTypeIndex]) o.gameObject.SetActive(false);
    }
    /*!
     * Recycles all objects in all pools
     */
    public void RecycleAllPools()
    {
        foreach (GameObject[] arr in pools)
        {
            foreach (GameObject o in arr) o.gameObject.SetActive(false);
        }
    }

    public void EnableObjectNoReturn(int pooledObjectTypeIndex)
    {
        for (int i = 0; i < pools[pooledObjectTypeIndex].Length - 1; i++)
        {
            if (!pools[pooledObjectTypeIndex][i].activeSelf)
            {
                pools[pooledObjectTypeIndex][i].gameObject.SetActive(true);
                break;
            }
        }
    }

    public GameObject EnableObject(int pooledObjectTypeIndex)
    {
        GameObject o = null;
        for (int i = 0; i < pools[pooledObjectTypeIndex].Length - 1; i++)
        {
            if (!pools[pooledObjectTypeIndex][i].activeSelf)
            {
                pools[pooledObjectTypeIndex][i].gameObject.SetActive(true);
                o = pools[pooledObjectTypeIndex][i].gameObject;
                break;
            }
        }
        return o;
    }
}
