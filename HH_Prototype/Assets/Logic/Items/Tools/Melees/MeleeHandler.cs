using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(BoxCollider))]
public class MeleeHandler : MonoBehaviour, IInteractable, ICraftable
{
    [Header("Melee Information")]
    [SerializeField] MeleeData meleeData;
    [SerializeField] GameObject gfx;
    Animator anim;
    BoxCollider hitBox;
    
    delegate void SwingInteraction();
    SwingInteraction swingInteraction;
    delegate void ReturnInteraction();
    ReturnInteraction returnInteraction;
    int comboProgression = 0;

    [SerializeField] bool equipped;

    float mouse1Current = 0, mouse2Current = 0;
    float currentTime = 0;
    float currentHang = 0;
    bool hanging = false;
    bool swung = false;
    [Header(" ")]
    [SerializeField] bool debug = true;

    void Awake()
    {
        SetUpMelee();
        anim = GetComponent<Animator>();
        hitBox = GetComponent<BoxCollider>();
    }

    private void Update()
    {
        currentTime += Time.deltaTime;

        if(meleeData != null)
        {
            if (meleeData.swinging)
            {
                if (currentTime > meleeData.timeInBetweenSwings)
                {
                    currentTime = 0;
                    meleeData.swinging = false;
                }
            }

            if (hanging)
            {
                currentHang += Time.deltaTime;

                if (currentHang > meleeData.hangTime)
                {
                    Debug.Log("stopped hanging");
                    returnInteraction();
                    hanging = false;
                    currentHang = 0;
                    comboProgression = 0;
                    gfx.transform.rotation = transform.rotation;
                }
            }



            if (equipped && !meleeData.swinging)
            {
                swingInteraction();
            }
        }
        

        //live updates localpos
        if (debug)
        {
            if (meleeData)
            {
                transform.localPosition = meleeData.offset;
            }
        }
    }

   

    /*!
     * takes in mouse input and updates which mouse key is currently being pressed down
     */
    public void Interact(float mouse1, float mouse2)
    {
        mouse1Current = mouse1; mouse2Current = mouse2;
    }

    public void Craft()
    {
        meleeData.unlocked = true;
        SetParentAndResetTransform(PlayerController.instance.transform);
        SetUpMelee();
        Equip(false);
    }

    public void SetUpMelee()
    {
        if (!meleeData)
        {
            return;
        }

        transform.localPosition = meleeData.offset;


        //sets up delegate functions
        swingInteraction -= SingleSwing;
        swingInteraction -= AutoSwing;

        returnInteraction -= SingleReturn;
        returnInteraction -= AutoReturn;

        switch(meleeData.meleeType)
        {
            case MeleeData.MeleeType.Single:
                swingInteraction += SingleSwing;
                returnInteraction += SingleReturn;
                break;
            case MeleeData.MeleeType.Auto:
                swingInteraction += AutoSwing;
                returnInteraction += AutoReturn;
                break;
        }

    }

    public void SetUpMeleeWithTransform(Transform tForm)
    {
        SetParentAndResetTransform(tForm);
        SetUpMelee();
    }

    public void SetMeleeData(MeleeData data)
    {

        meleeData = data;
        SetUpMelee();
    }


    public void SetParentAndResetTransform(Transform parent)
    {
        transform.parent = parent;

        if (meleeData)
        {
            transform.localPosition = meleeData.offset;
            transform.forward = parent.transform.forward;
        }
    }

    public void Equip(bool equip)
    {
        equipped = equip;
        gfx.SetActive(equip);
        SetUpMelee();
    }

    public bool Equipped
    {
        get
        {
            return equipped;
        }
    }

    public MeleeData MeleeData
    {
        get
        {
            return meleeData;
        }
    }

    void SingleSwing()
    {
        if ((mouse1Current > 0f))
        {
            if (!swung)
            {
                currentTime = 0;
                anim.SetTrigger("Swing");
                OnSwing();
                hanging = true;
            }
        }
        else
        {
            if (!meleeData.swinging)
            {
                swung = false;
            }
        }
    }
    void SingleReturn()
    {
        anim.SetTrigger("Return");
    }
    void AutoSwing()
    {
        if ((mouse1Current > 0f) && !meleeData.swinging)
        {
            currentTime = 0;
            anim.SetBool("Enable", true);
            OnSwing();
            hanging = true;
        }
    }
    void AutoReturn()
    {
        anim.SetBool("Enable", false);
    }

    private void OnSwing()
    {
        swung = true;
        meleeData.swinging = true;
    }

    public void OnHit(Collider other)
    {
        //will possibly be deprecated
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Physics.IgnoreCollision(other.gameObject.GetComponent<Collider>(), GetComponent<Collider>());
        }

        if (debug)
        {
            Debug.Log("enter OnHit:" + other.gameObject.name);
        }
        if (other.gameObject.CompareTag("Enemy"))
        {
            if (debug)
            {
                Debug.Log("Enemy, damaging...");
            }

            IDamageable damageableEnemy;
            if (other.transform.TryGetComponent<IDamageable>(out damageableEnemy))
            {
                damageableEnemy?.TakeDamage(meleeData.damage, DamageStateMachine.DamageType.Physical);
            }
        }
        //OnHit(other);
    }
}
