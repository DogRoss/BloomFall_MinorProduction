using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;


/*!
 * 
 * Code Written by Edgar Gunther
 * 
 * class to handle player input and interactions with worldspace
 * item interactions and inventory system are stored and referenced here
 * 
 */

[RequireComponent(typeof(Inventory))]
public class PlayerController : ThirdPersonMovementCC, IDamageable
{
    //Interactabl Objcts
    GunHandler gun;
    MeleeHandler melee;
    IInteractable equippedObject;
    [SerializeField] TextMeshProUGUI currentWeaponText;
    [SerializeField] TextMeshProUGUI currentAmmoText;
    [SerializeField] GameObject deathScreen;

    Vector2 mouseInput = Vector2.zero;

    [Header("Health")]
    public float maxHealth = 100f;
    public float health = 100f;
    public Image healthFillBar;
    public float healthSmoothing = 1;

    bool paused = false;

    public static PlayerController instance;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        anim = GetComponent<Animator>();

        controller = GetComponent<CharacterController>();
        
        SetUpGunWeapon(false);
        SetUpMeleeWeapon(false);

        onUpdate += OnUpdate;

        //StartCoroutine(UpdateUI(0));
        UpdateUI();
    }
    private void OnUpdate()
    {
        if(gun != null)
        {
            gun.target = GetHit().point;
            gun.target.y = gun.transform.position.y;
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log(DayCycleScript.daytime);
        }

        if(health <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    public GunHandler Gun
    {
        get
        {
            return gun;
        }
    }

    public void SetUpGunWeapon(bool equip)
    {
        Debug.Log("PC: set up gun");

        if(gun != null)
        {
            gun.SetUpGunWithTransform(transform);
            gun.Equip(equip);
        }
    }

    public void SetUpMeleeWeapon(bool equip)
    {
        Debug.Log("enter setup for melee");

        if(melee != null)
        {
            melee.SetUpMeleeWithTransform(transform);
            melee.Equip(equip);
        }
    }

    //Equipped tool management
    private void OnNum1() //switch to or between guns
    {
        Debug.Log("Enter num1");

        if (!paused)
        {
            if (equippedObject != gun || gun == null)
            {
                Debug.Log("equipped object isnt equal to gun");
                if (gun != null)
                {
                    Debug.Log("gun data is present within controller, switch to it");
                    equippedObject = gun;
                    gun.Equip(true);
                }
                else if (ToolPooling.instance.TryGetNextUnlockedGun(out gun))
                {
                    Debug.Log("gun data wasnt present, but was grabbed");
                    equippedObject = gun;
                    gun.Equip(true);
                }
                else
                {
                    Debug.Log("Could not grab gun object");
                    return;
                }

                //dequip other objects
                melee?.Equip(false);
            }
            else
            {
                Debug.Log("cycling guns");
                if (gun != null)
                    gun.Equip(false);

                GunHandler temp = ToolPooling.instance.GetNextUnlockedGun();
                if (temp != gun)
                {
                    gun = temp;
                }
                else
                {
                    //TODO: make sure to reset gun stuff
                    gun = null;
                }
                equippedObject = gun;
            }

            SetUpGunWeapon(true);
        }

        UpdateUI();
        
    }
    private void OnNum2() //switch to or between melee weapons
    {


        /*Debug.Log("bruyh");
        
        if (!paused)
        {
            Debug.Log("enter, not paused");
            if (equippedObject != melee || melee == null)
            {
                Debug.Log("melee isnt equipped");
                if (melee != null)
                {
                    Debug.Log("melee isnt null");
                    equippedObject = melee;
                    melee.Equip(true);
                }
                else if (ToolPooling.instance.TryGetNextUnlockedMelee(out melee))
                {
                    Debug.Log("melee is null, grabbing melee object");
                    equippedObject = melee;
                    melee.Equip(true);
                }
                else
                {
                    Debug.Log("melee is null, cannot grab a melee obj");
                    return;
                }

                //dequip
                gun?.Equip(false);
            }
            else
            {
                if (melee != null)
                    melee.Equip(false);

                MeleeHandler temp = ToolPooling.instance.GetNextUnlockedMelee();
                if (temp != melee)
                {
                    melee = temp;
                }
                else
                {
                    //TODO: make sure to reset melee stuff
                    melee = null;
                }
                equippedObject = melee;
            }
        
            SetUpMeleeWeapon(true);
        }
        
        UpdateUI();*/


        equippedObject = null;

        melee?.Equip(false);
        gun?.Equip(false);

        UpdateUI();
    }
    private void OnNum3() //unequip weapons
    {
        equippedObject = null;

        melee?.Equip(false);
        gun?.Equip(false);

        UpdateUI();
    }

    /*!
     * saved left and right mouse button input via Vector2;
     */
    private void OnMouseClick(InputValue value)
    {
        mouseInput = value.Get<Vector2>();

        if(equippedObject != null)
        {
            if (equippedObject == gun && gun.GunData.currentAmmo <= 0)
                OnReload();

            if (!paused)
                equippedObject.Interact(mouseInput.x, mouseInput.y);
        }

        

        //Debug.Log(DayCycleScript.daytime);
    }

    private void OnReload()
    {
        if(gun.GunData != null && equippedObject == gun && !gun.Reloading)
            gun.Reload();
    }

    private void OnInteract()
    {

    }


    private void OnMenu()
    {
        //InventoryUILinker.EnableMenuUI(InventoryUILinker.MenuUI.activeSelf ? false : true);
        if (!UIManager.instance.MenuUI.activeSelf)
        {
            UIManager.instance.EnableMenuUI(true);
            paused = true;
        }
        else
        {
            UIManager.instance.EnableMenuUI(false);
            paused = false;
        }

        UpdateUI();
    }

    public void TakeDamage(float damage, DamageStateMachine.DamageType type)
    {
        health -= damage;
        if(health <= 0)
        {
            OnDeath();
        }
        //StartCoroutine(UpdateUI(damage));
        UpdateUI();
    }

    [ContextMenu("TakeDamage")]
    public void TakeDamage()
    {
        TakeDamage(maxHealth / 2 , DamageStateMachine.DamageType.Physical);
        //StartCoroutine(UpdateUI(maxHealth / 2));
        UpdateUI();
    }

    public void OnDeath()
    {
        Debug.Log("Death");
        deathScreen.SetActive(true);
        UIManager.instance.EnableMenuUI(false);
    }

    public void HealAmount(float amount)
    {
        health += amount;
        UpdateUI();
    }

    [ContextMenu("HealPlayer")]
    public void Heal()
    {
        health = maxHealth;
        UpdateUI();
    }


    public void UpdateUI()
    {
        healthFillBar.fillAmount = health / maxHealth;

        if(gun != null && equippedObject != null)
        {
            currentAmmoText.gameObject.SetActive(true);
            currentAmmoText.text = gun.GunData.currentAmmo + " / " + gun.GunData.magSize;
        }
        else
        {
            currentAmmoText.gameObject.SetActive(false);
        }

        if(equippedObject == gun && gun)
        {
            currentWeaponText.text = gun.GunData.name;
        }
        else if(equippedObject == melee && melee)
        {
            currentWeaponText.text = melee.MeleeData.name;
        }
        else
        {
            currentWeaponText.text = " ";
        }
    }

}
