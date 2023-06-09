using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Placeable : MonoBehaviour, IPlaceable, IDamageable
{
    [Header("GunInfo")]
    public GunHandler gun;

    [Header("StandInfo")]
    public PlaceableData placeableData;
    public MeshFilter standGFX;

    [Header("EnemyDetection")]
    [SerializeField] List<GameObject> enemies = new List<GameObject>();

    [Header("LogicInfo")]
    public Vector3 placementOffset;
    public float health = 100f;
    public bool enabled = false;
    float currentTime = 0f;


    public bool debug = true;

    // Start is called before the first frame update
    void Awake()
    {
        gun = GetComponentInChildren<GunHandler>();
        SetUpPlaceable();
    }

    // Update is called once per frame
    void Update()
    {

        currentTime += Time.deltaTime;

        if (enabled)
        {
            gun.Equip(true);

            if (currentTime > placeableData.timeBetweenShots)
            {
                Debug.Log("shoot");
                currentTime = 0;
                gun.Interact(1, 0);
            }
            else
            {
                gun.Interact(0, 0);
            }
        }
        else
        {
            gun.target = transform.position + Vector3.down;
        }

        if (debug)
        {
            SetUpPlaceable();
            gun.target = FindTarget();
        }
    }

    private Vector3 FindTarget()
    {
        Debug.Log("enter findTarget");
        Vector3 closest = Vector3.zero;
        int count = 0;
        foreach(GameObject enemy in enemies)
        {
            if(Vector3.Distance(enemy.transform.position, transform.position) < Vector3.Distance(closest, transform.position))
            {
                count++;
                Debug.Log("found new closest: " + count);
                closest = enemy.transform.position;
            }
        }

        if (closest != null || closest != Vector3.zero)
        {
            Debug.Log("returning closest");
            return closest;
        }
        else
        {
            Debug.Log("return vZero");
            return Vector3.zero;
        }

    }

    private void SetUpPlaceable()
    {
        gun.SetGunData(placeableData.gunData);
        gun.SetUpGunWithTransform(transform);

        standGFX.transform.localPosition = placeableData.standOffset;
        gun.transform.localPosition = placeableData.gunOffset;

    }

    public void Place(PlaceableHandler handler)
    {
        Debug.Log("Enter Place Placeable Fucntion");
        RaycastHit hit;
        if (Physics.Raycast(handler.transform.position + (handler.transform.forward + placementOffset), Vector3.down, out hit))
        {
            Placeable obj = Instantiate(this, hit.point, handler.transform.rotation);
            obj.Enable(true);
            obj.gun = gun;

            obj.standGFX.mesh = obj.placeableData.standGFX.sharedMesh;
            //if(obj.gunGFX != null && obj.gunGFX.mesh != null)
            //{
            //    Debug.Log("gunGFX properly set up");
            //}
            //else
            //{
            //    Debug.LogError("GUNGFX NOT PROPERLY SET UP");
            //}

            //if (obj.gun != null && obj.gun.GunData != null)
            //{
            //    Debug.Log("Gun Handler and gun data set up correctly");
            //    if(obj.gun.GunData.gunGFX != null && obj.gun.GunData.gunGFX.sharedMesh != null)
            //    {
            //        Debug.Log("gunData mesh set up correctly");
            //    }
            //    else
            //    {
            //        Debug.LogError("GUNDATA MESH NOT SET UP CORRECTLY");
            //    }
            //}
            //else
            //{
            //    Debug.LogError("GUN HANDLER AND/OR GUN DATA NOT SET UP CORRECTLY");
            //    if(obj.gun == null)
            //    {
            //        Debug.LogError("->GUN HANDLER");
            //    }
            //    else if(obj.gun.GunData == null)
            //    {
            //        Debug.LogError("->GUN DATA");
            //        GunData gData = obj.gun.GunData;
                    
            //    }
            //}
        }
    }

    public void Enable(bool enable)
    {
        enabled = enable;
    }

    public void TakeDamage(float damage, DamageStateMachine.DamageType type)
    {
        health -= damage;
        if(health <= 0)
        {
            OnDeath();
        }
        Debug.Log("Damage Taken: " + damage + ", Health remains: " + health);
    }

    public void OnDeath()
    {
        Enable(false);
        Destroy(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            enemies.Add(other.gameObject);
        }

        gun.target = FindTarget();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            for(int i = 0; i < enemies.Count; i++)
            {
                if(enemies[i] == other.gameObject)
                {
                    enemies.RemoveAt(i);
                    break;
                }
            }
        }

        gun.target = FindTarget();
    }
}
