using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class EnemyStatus : MonoBehaviour, IDamageable
{
    public enum EnemyType
    {
        Wolf, Deer, Bear, Raccoon, Fox, Bloom, Boss
    }
    public EnemyType enemy;

    //fire and physical damage multipliers
    private float dmgMultiplierF;
    private float dmgMultiplierPh;
    public float damage = 5f;
    public float enemyHealth = 10f;
    public float maxEnemyHealth = 10f;
    public float enemySpeed;
    public float enemySight = 12f;
    public bool sighted = false;
    public bool provoked = false;
    public static bool racProvoked = false;
    public NavMeshAgent navAgent;
    public bool bloomActive = true;
    public bool canSpawn = false;
    public static int killDropIndex;
    public EnemySpawnManager enemySpawnManager;
    private PlayerController playerPos;
    public int enemyTypeIndex;
    public int timesSpawned = 0;
    public Renderer[] renderer;
    

    //timer variables
    float currentTime = 0f;
    [SerializeField] float damageCooldown = 2f;

    //deal damage to enemy based on type of damage
    public void TakeDamage(float damage, DamageStateMachine.DamageType type)
    {
        Debug.Log("enter, damage: " + damage);
        if(type == DamageStateMachine.DamageType.Fire)
        {
            damage *= dmgMultiplierF;
        }
        else if(type == DamageStateMachine.DamageType.Physical)
        {
            damage *= dmgMultiplierPh;
        }

        enemyHealth -= damage;

        
        StartCoroutine(AnimateDamage());
        
        
        if(enemy == EnemyType.Raccoon)
        {
            racProvoked = true;
        }
        else
        {
            provoked = true;
        }
    }

    public void OnDeath()
    {
        
        if (gameObject != null)
        {
            if (enemy == EnemyType.Boss)
            {
                Debug.Log("winner");
                SceneManager.LoadScene("VictorY!");
            }
            Debug.Log("Recycle");
            if((enemy == EnemyType.Fox) || (enemy == EnemyType.Bloom))
            {
                ObjectPool.instance.SpawnObject(new Vector3(transform.position.x, (transform.position.y + 0.5f), transform.position.z), 7);
            }
            else
            {
                ObjectPool.instance.SpawnObject(new Vector3(transform.position.x, (transform.position.y + 0.5f), transform.position.z), 6);
            }
           
            for (int i = 0; i < renderer.Length; i++)
            {
                Debug.Log("Turning back to white");
                renderer[i].material.SetColor("_Color", Color.white);
            }
            gameObject.SetActive(false);
            Debug.Log("object shouldnt be active");
            enemyHealth = maxEnemyHealth;
        }
    }

    public void IsDead()
    {
        if(enemyHealth <= 0)
        {
            Debug.Log("dead");
            OnDeath();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
        enemySpawnManager = FindObjectOfType<EnemySpawnManager>();
        playerPos = FindObjectOfType<PlayerController>();
        renderer = GetComponentsInChildren<Renderer>(true);
        
        

        //properties based on enemy type
        switch (enemy)
        {
            case EnemyType.Wolf:
                enemyTypeIndex = 0;
                dmgMultiplierF = 2f;
                dmgMultiplierPh = 0.5f;
                damage = 50f;
                maxEnemyHealth = 80f;
                enemySpeed = 4f;
                break;
            case EnemyType.Deer:
                enemyTypeIndex = 1;
                dmgMultiplierF = 0.5f;
                dmgMultiplierPh = 2f;
                damage = 40f;
                maxEnemyHealth = 150f;
                enemySpeed = 7f;
                break;
            case EnemyType.Bear:
                enemyTypeIndex = 2;
                dmgMultiplierF = 0.5f;
                dmgMultiplierPh = 2f;
                damage = 80f;
                maxEnemyHealth = 300f;
                enemySpeed = 3f;
                break;
            case EnemyType.Raccoon:
                enemyTypeIndex = 3;
                dmgMultiplierF = 0.5f;
                dmgMultiplierPh = 2f;
                damage = 20f;
                maxEnemyHealth = 25f;
                enemySpeed = 5f;
                break;
            case EnemyType.Fox:
                enemyTypeIndex = 4;
                dmgMultiplierF = 0.5f;
                dmgMultiplierPh = 2f;
                damage = 30f;
                maxEnemyHealth = 50f;
                enemySpeed = 5f;
                break;
            case EnemyType.Bloom:
                enemyTypeIndex = 5;
                dmgMultiplierF = 0.5f;
                dmgMultiplierPh = 2f;
                damage = 0;
                maxEnemyHealth = 400f;
                enemySpeed = 0;
                break;
            case EnemyType.Boss:
                enemyTypeIndex = 6;
                dmgMultiplierF = 0.5f;
                dmgMultiplierPh = 2f;
                damage = 0;
                maxEnemyHealth = 1000f;
                enemySpeed = 0;
                break;
        }
        enemyHealth = maxEnemyHealth;
        if(navAgent != null)
        {
            navAgent.speed = enemySpeed;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
        IsDead();
        
        BloomActive();
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("enter collision");

        if(currentTime < damageCooldown)
        {
            currentTime = 0;
            return;
        }

        DamageStateMachine.DamageType dmgType = DamageStateMachine.DamageType.Physical;
        
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("hitPlayer");
            IDamageable damageable = other.transform.GetComponent<IDamageable>();
            damageable?.TakeDamage(damage, dmgType);
        }
    }

    void BloomActive()
    {

        if (enemy == EnemyType.Bloom)
        {
            if (gameObject.activeInHierarchy)
            {


                Debug.DrawRay(transform.position, (playerPos.transform.position - transform.position).normalized * 75f, Color.cyan);
                Debug.DrawRay(transform.position, (playerPos.transform.position - transform.position).normalized * 30f, Color.green);
                //if spawner close enough to player

                if (((transform.position.x + 75f > playerPos.transform.position.x) &&
                (transform.position.x - 75f < playerPos.transform.position.x)) && ((transform.position.z + 75f > playerPos.transform.position.z) &&
                (transform.position.z - 75f < playerPos.transform.position.z)))
                {

                    if (DayCycleScript.daytime)
                    {
                        //Debug.Log("spawn");
                        canSpawn = true;
                    }
                    //if spawner offscreen distance
                    else if ((!DayCycleScript.daytime) && ((transform.position.x > playerPos.transform.position.x + 30f) ||
                    (transform.position.x < playerPos.transform.position.x - 30f) || (transform.position.z > playerPos.transform.position.z + 30f) ||
                    (transform.position.z < playerPos.transform.position.z - 30f)))
                    {

                        canSpawn = true;
                    }
                    else
                    {
                        //Debug.Log("too close");
                        canSpawn = false;
                    }

                }
                else
                {
                    //Debug.Log("Notspawn");
                    canSpawn = false;
                }

                if (DayCycleScript.daytime && (bloomActive == false))
                {

                    GetComponent<Collider>().enabled = true;
                    GetComponent<Renderer>().enabled = true;
                    bloomActive = true;
                }
                else if ((!DayCycleScript.daytime) && (bloomActive == true))
                {
                    GetComponent<Collider>().enabled = false;
                    GetComponent<Renderer>().enabled = false;
                    bloomActive = false;
                }
            }
            else
            {
                canSpawn = false;
                bloomActive = false;
            }
        }
        
    }

    IEnumerator AnimateDamage()
    {
        for (int f = 0; f < 4; f++)
        {
            for(int i = 0; i < renderer.Length; i++)
            {
                Debug.Log("flash damage");
                if (renderer[i].material.color == Color.white)
                {
                    
                    renderer[i].material.SetColor("_Color", Color.red);
                    
                }
                else
                {
                    renderer[i].material.SetColor("_Color", Color.white);
                    
                }
            }
            yield return new WaitForSeconds(0.2f);

        }
        for (int i = 0; i < renderer.Length; i++)
        {
            Debug.Log("Turning back to white");
            renderer[i].material.SetColor("_Color", Color.white);
        }
        Debug.Log("End of Enumerator");
    }
    
}



