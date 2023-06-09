using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    public Transform goal;
    public Transform loot;
    public Vector3 runEnd = Vector3.zero;
    public Vector3 goalPos;
    Vector3 lastPos = Vector3.zero;
    RaycastHit hit;
    public NavMeshAgent agent;
    bool wander = true;
    public EnemyStatus state;
    public Animator anim;
    public static float wanderDistance = 20f;
    public Vector3 wanderPos;
    public Vector3 startPos;
    bool waiting = true;
    GunHandler gun;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        state = GetComponent<EnemyStatus>();
        anim = GetComponent<Animator>();
        
        
    }

    private void OnEnable()
    {
        waiting = true;
        GetComponent<NavMeshAgent>().isStopped = false;
        lastPos = transform.position;
        wanderPos = transform.position;
        startPos = transform.position;
        if (FindObjectOfType<PlayerController>() != null)
        {
            goal = FindObjectOfType<PlayerController>().transform;
        }
        wander = true;
        StartCoroutine(WanderSet());
        
    }

    private void Update()
    {
        
        Vision();
        
    }

    private void OnDisable()
    {
        wander = false;
    }

    IEnumerator WanderSet()
    {
        while (wander)
        {


            goalPos = goal.position;

            
            //else
            //{
            //    bool onNavMesh = agent.isOnNavMesh;
            //    bool isntNull = agent != null;
            //    bool activeNEnabled = agent.isActiveAndEnabled;
            //    Debug.LogError("AGENT UNABLE TO BE ACCESSED, isOnNavMesh, agent isnt null, activeNEnabled: " + onNavMesh + " // " + isntNull + " // " + activeNEnabled);
            //}

            if (DayCycleScript.daytime == true)
            {
                if (agent != null && agent.isActiveAndEnabled && agent.isOnNavMesh)
                {
                    if ((state.sighted == false) && (state.provoked == false))
                    {
                        Wander();

                    }
                    else
                    {
                        
                        //Debug.Log("angry deer");
                        agent.SetDestination(goal.position);
                        lastPos = goal.position;
                        
                    }

                }
                
            }
            else
            {
                
                agent.SetDestination(lastPos);

                switch (state.enemy)
                {
                    case EnemyStatus.EnemyType.Wolf:
                        WanderThenAttack();
                        break;
                    case EnemyStatus.EnemyType.Deer:
                        RunAway();
                        break;
                    case EnemyStatus.EnemyType.Bear:
                        SleepUntilProvoked();
                        break;
                    case EnemyStatus.EnemyType.Raccoon:
                        WanderProvoked();
                        //attack in groups
                        break;
                    case EnemyStatus.EnemyType.Fox:
                        RunAway();
                        break;
                    case EnemyStatus.EnemyType.Bloom:
                        
                        break;
                }
            }
            yield return new WaitForSeconds(1f);
        }
    }
    //bear night behavior, stay still unless damage taken, then chase player
    void SleepUntilProvoked()
    {
        if(state.provoked == true)
        {
            if (agent != null && agent.isActiveAndEnabled && agent.isOnNavMesh)
            {
                //Debug.Log("angry bear");
                agent.SetDestination(goal.position);
                lastPos = goal.position;
            }
        }
    }
    
    void Wander()
    {
        agent.SetDestination(wanderPos);
        if(waiting)
        {
            StartCoroutine(DestinationTimer());
        }
        
        
    }

    IEnumerator DestinationTimer()
    {
        waiting = false;
        yield return new WaitForSeconds(3f);
        if(wanderPos != startPos)
        {
            if ((((wanderPos.x - 1f) <= (transform.position.x)) && ((transform.position.x) <= (wanderPos.x + 1f)))
            && (((wanderPos.z - 1f) <= (transform.position.z)) && ((transform.position.z) <= (wanderPos.z + 1f))))
            {
                wanderPos = new Vector3(Random.Range(transform.position.x - wanderDistance, transform.position.x + wanderDistance), 0, Random.Range(transform.position.z - wanderDistance, transform.position.z + wanderDistance));
            }
            else
            {
                wanderPos = startPos;
            }
        }
        else
        {
            wanderPos = new Vector3(Random.Range(transform.position.x - wanderDistance, transform.position.x + wanderDistance), 0, Random.Range(transform.position.z - wanderDistance, transform.position.z + wanderDistance));
        }
        waiting = true;
    }

    //deer night behavior, wander randomly within area unless attacked, then run
    void RunAway()
    {
        if (state.provoked == false)
        {
            Wander();
        }
       
        if ((transform.position.x == lastPos.x) && (transform.position.z == lastPos.z))
        {
            state.provoked = false;
        }

        
        if (state.provoked == true)
        {
            if (agent != null && agent.isActiveAndEnabled && agent.isOnNavMesh)
            {
                
                runEnd = transform.position + (transform.position - goal.transform.position);
;                
                agent.SetDestination(runEnd);
                lastPos = runEnd;
            }
        }
        
    }

    //wolf night behavior, raycast
    void WanderThenAttack()
    {
        if(state.sighted == false)
        {
            Wander();
            
        }
        
        
        
        if((state.sighted == true) || (state.provoked == true))
        {
            if (agent != null && agent.isActiveAndEnabled && agent.isOnNavMesh)
            {
                Debug.Log("angry wolf");
                agent.SetDestination(goal.position);
                lastPos = goal.position;
            }
        }

    }

    //raycast to see the player
    void Vision()
    {
        if(state.sighted != true)
        {
            if (DayCycleScript.daytime == false)
            {
                if (state.enemy == EnemyStatus.EnemyType.Wolf || state.enemy == EnemyStatus.EnemyType.Fox)
                {
                    if (Physics.Raycast(transform.position, transform.forward, out hit, state.enemySight) && hit.transform.gameObject.CompareTag("Player"))
                    {

                        Debug.Log("hit");
                        state.sighted = true;

                    }

                }
            }
            else
            {
                Debug.DrawRay(new Vector3(transform.position.x, transform.position.y + 2, transform.position.z), transform.forward * state.enemySight, Color.red);
                if (Physics.Raycast(new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), transform.forward, out hit, state.enemySight))
                {
                    
                    if (hit.transform.gameObject.CompareTag("Player"))
                    {
                        Debug.Log("hit");
                        state.sighted = true;
                    }
                    
                        

                    



                }

            }
        }
        
        
        
    }

    //raccoon night behavior, wanders until attacked
    void WanderProvoked()
    {
        if(EnemyStatus.racProvoked == false)
        {
            Wander();
        }
        
        if(EnemyStatus.racProvoked == true)
        {
            if (agent != null && agent.isActiveAndEnabled && agent.isOnNavMesh)
            {
                Debug.Log("angry raccoons");
                agent.SetDestination(goal.position);
                lastPos = goal.position;
            }
        }
        
    }

    //fox night behavior, goes to loot after seeing player
    void LeadToLoot()
    {
        
        if (state.sighted == false)
        {
            Wander();

        }
        
        if(loot != null)
        {
            if ((transform.position.x == loot.position.x) && (transform.position.z == loot.position.z))
            {
                state.sighted = false;
            }

            if (state.sighted == true)
            {

                Debug.Log("saw player");

                agent.SetDestination(loot.position);
                lastPos = loot.position;



            }
        }
        else
        {
            Wander();
        }

        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            
            anim.SetTrigger("OnAttack");
            
        }
    }

    
    

    void StopMove()
    {
        GetComponent<NavMeshAgent>().isStopped = true;
    }

    void MoveAgain()
    {
        GetComponent<NavMeshAgent>().isStopped = false;
    }
}
