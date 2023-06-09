using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Inventory : MonoBehaviour
{
    //[Range(0, 200)]
    //[SerializeField] int slotAmount = 15;
    [Range(0, 200)]
    [SerializeField] int startAmount = 15;
    //int spaceTaken = 0;
    PickupScript pickup;
    PlayerController player;

    [SerializeField] TextMeshProUGUI bulletUI; 
    [SerializeField] TextMeshProUGUI metalUI; 
    [SerializeField] TextMeshProUGUI reloadUI; 

    //reference static object pool here

    [SerializeField] int bulletAmount = 0; //0
    [SerializeField] int molotovAmount = 0; //1
    [SerializeField] int trapAmount= 0; //2
    [SerializeField] int gasolineAmount = 0; //3
    [SerializeField] int metalAmount = 0; //4
    [SerializeField] int bottleAmount = 0; //5
    [SerializeField] int clothAmount = 0; //6
    [SerializeField] int woodAmount = 0; //7

    public static Inventory instance;

    private void Start()
    {
        instance = this;

        player = GetComponent<PlayerController>();

        bulletAmount = startAmount;
        molotovAmount = startAmount;
        trapAmount = startAmount;
        gasolineAmount = startAmount;
        metalAmount = startAmount;
        bottleAmount = startAmount;
        clothAmount = startAmount;
        woodAmount = startAmount;

        UpdateUI();
    }

    //pBullet = 0, molotovAmount = 1, trapAmount= 2, gasolineAmount = 3
    //metalAmount = 4, bottleAmount = 5, clothAmount = 6, woodAmount = 7
    public int GrabAmountOfIndex(int amount, int typeOfObjectIndex)
    {
        int returnNum = 0;
        switch (typeOfObjectIndex)
        {
            case 0:
                if (bulletAmount > amount)
                {
                    bulletAmount -= amount;
                    returnNum = amount;
                }
                else
                {
                    returnNum = bulletAmount;
                    bulletAmount = 0;
                }
                break;

            case 1:
                if (molotovAmount > amount)
                {
                    molotovAmount -= amount;
                    returnNum = amount;
                }
                else
                {
                    returnNum = molotovAmount;
                    molotovAmount = 0;
                }
                break;

            case 2:
                if (trapAmount > amount)
                {
                    trapAmount -= amount;
                    returnNum = amount;
                }
                else
                {
                    returnNum = trapAmount;
                    trapAmount = 0;
                }
                break;

            case 3:
                if (gasolineAmount > amount)
                {
                    gasolineAmount -= amount;
                    returnNum = amount;
                }
                else
                {
                    returnNum = gasolineAmount;
                    gasolineAmount = 0;
                }
                break;

            case 4:
                if (metalAmount > amount)
                {
                    metalAmount -= amount;
                    returnNum = amount;
                }
                else
                {
                    returnNum = metalAmount;
                    metalAmount = 0;
                }
                break;
            case 5:
                if (bottleAmount > amount)
                {
                    bottleAmount -= amount;
                    returnNum = amount;
                }
                else
                {
                    returnNum = bottleAmount;
                    bottleAmount = 0;
                }
                break;
            case 6:
                if (clothAmount > amount)
                {
                    clothAmount -= amount;
                    returnNum = amount;
                }
                else
                {
                    returnNum = clothAmount;
                    clothAmount = 0;
                }
                break;
            case 7:
                if (woodAmount > amount)
                {
                    woodAmount -= amount;
                    returnNum = amount;
                }
                else
                {
                    returnNum = woodAmount;
                    woodAmount = 0;
                }
                break;
            default:
                break;
        }
        UpdateUI();

        return returnNum;
    }

    public int ReadAmountOfIndex(int amount, int typeOfObjectIndex)
    {
        int returnNum = 0;
        switch (typeOfObjectIndex)
        {
            case 0:
                if (bulletAmount > amount)
                {
                    Debug.Log("ba");
                    return amount;

                }
                else
                {
                    Debug.Log("ca");
                    returnNum = bulletAmount;
                }
                break;

            case 1:
                if (molotovAmount > amount)
                {
                    return amount;
                }
                else
                {
                    returnNum = molotovAmount;
                }
                break;

            case 2:
                if (trapAmount > amount)
                {
                    return amount;
                }
                else
                {
                    returnNum = trapAmount;
                }
                break;

            case 3:
                if (gasolineAmount > amount)
                {
                    return amount;
                }
                else
                {
                    returnNum = gasolineAmount;
                }
                break;

            case 4:
                if (metalAmount > amount)
                {
                    return amount;
                }
                else
                {
                    returnNum = metalAmount;
                }
                break;
            case 5:
                if (bottleAmount > amount)
                {
                    return amount;
                }
                else
                {
                    returnNum = bottleAmount;
                }
                break;
            case 6:
                if (clothAmount > amount)
                {
                    return amount;
                }
                else
                {
                    returnNum = clothAmount;
                }
                break;
            case 7:
                if (woodAmount > amount)
                {
                    return amount;
                }
                else
                {
                    returnNum = woodAmount;
                }
                break;
        }
        return returnNum;
    }

    public int BulletAmount
    {
        get
        {
            return bulletAmount;
        }
    }

    public int MolotovAmount
    {
        get
        {
            return molotovAmount;
        }
    }

    public int TrapAmount
    {
        get
        {
            return trapAmount;
        }
    }

    public int GasolineAmount
    {
        get
        {
            return gasolineAmount;
        }
    }

    public int MetalAmount
    {
        get
        {
            return metalAmount;
        }
    }

    public int BottleAmount
    {
        get
        {
            return bottleAmount;
        }
    }

    public int ClothAmount
    {
        get
        {
            return clothAmount;
        }
    }

    public int WoodAmount
    {
        get
        {
            return woodAmount;
        }
    }

    //pBullet = 0, molotovAmount = 1, trapAmount= 2, gasolineAmount = 3
    //metalAmount = 4, bottleAmount = 5, clothAmount = 6, woodAmount = 7
    public void TickUpItem(int typeOfObjectIndex)
    {
        switch (typeOfObjectIndex)
        {
            case 0:
                bulletAmount++;
                break;

            case 1:
                molotovAmount++;
                break;

            case 2:
                trapAmount++;
                break;

            case 3:
                gasolineAmount++;
                break;

            case 4:
                metalAmount++;
                break;
            case 5:
                bottleAmount++;
                break;
            case 6:
                clothAmount++;
                break;
        }

        UpdateUI();
    }

    //pBullet = 0, molotovAmount = 1, trapAmount= 2, gasolineAmount = 3
    //metalAmount = 4, bottleAmount = 5, clothAmount = 6, woodAmount = 7
    public void TickDownItem(int typeOfObjectIndex)
    {
        switch (typeOfObjectIndex)
        {
            case 0:
                bulletAmount = Mathf.Max(bulletAmount - 1,0);
                break;

            case 1:
                molotovAmount = Mathf.Max(molotovAmount - 1,0);
                break;

            case 2:
                trapAmount = Mathf.Max(trapAmount - 1,0);
                break;

            case 3:
                gasolineAmount = Mathf.Max(gasolineAmount - 1,0);
                break;

            case 4:
                metalAmount = Mathf.Max(metalAmount - 1,0);
                break;
            case 5:
                bottleAmount = Mathf.Max(bottleAmount - 1,0);
                break;
            case 6:
                clothAmount = Mathf.Max(clothAmount - 1,0);
                break;
        }

        UpdateUI();
    }

    public void TickBy5Up()
    {
        if (metalAmount <= 0)
            return;

        bulletAmount += 5;
        metalAmount -= 1;

        UpdateUI();
    }

    //pBullet = 0, molotovAmount = 1, trapAmount= 2, gasolineAmount = 3
    //metalAmount = 4, bottleAmount = 5, clothAmount = 6, woodAmount = 7
    public void AddAmountToItem(int amount, int typeOfObjectIndex)
    {

        Debug.Log("Enter AddAmountToItem, amount to add: " + amount + " // index: " + typeOfObjectIndex);
        switch (typeOfObjectIndex)
        {
            case 0:
                bulletAmount += amount;
                break;

            case 1:
                molotovAmount += amount;
                break;

            case 2:
                trapAmount += amount;
                break;

            case 3:
                gasolineAmount += amount;
                break;

            case 4:
                metalAmount += amount;
                break;
            case 5:
                bottleAmount += amount;
                break;
            case 6:
                clothAmount += amount;
                break;
            case 7:
                clothAmount += amount;
                break;
        }
        Debug.Log("End of AddAmountToObject, metal resource: " + metalAmount);

        UpdateUI();
    }

    //updates ui objects
    public void UpdateUI()
    {
        bulletUI.text = "- " + bulletAmount.ToString();
        metalUI.text = "- " + metalAmount.ToString();
        if (reloadUI)
        {
            if (!PlayerController.instance.Gun)
            {
                reloadUI.gameObject.SetActive(false);
            }
            else
            {
                if (PlayerController.instance.Gun.GunData.currentAmmo <= 0)
                {
                    reloadUI.gameObject.SetActive(true);
                }
                else
                {
                    reloadUI.gameObject.SetActive(false);
                }

                if (PlayerController.instance.Gun.GunData.reloading)
                {
                    reloadUI.text = "Reloading...";
                    reloadUI.gameObject.SetActive(true);
                }
                else
                {
                    reloadUI.text = "Reload!";
                }
            }

            
        }
    }

    //for pickups with objects
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);

        
        if(other.CompareTag("Pickup"))
        {
            Debug.Log("enter Pickup Compare");
            pickup = other.GetComponent<PickupScript>();

            if(pickup.pickupTypeIndex == 6)
            {
                Debug.Log("bottleAmount");
                if (player.health < player.maxHealth)
                {
                    player.HealAmount(10f);
                    if(player.health > player.maxHealth)
                    {
                        player.Heal();
                    }
                }
                
            }


            if(pickup.pickupMultiple == true)
            {
                Debug.Log("adding multiple: " + pickup.pickupAmount);
                AddAmountToItem(pickup.pickupAmount, pickup.pickupTypeIndex);
                UpdateUI();
            }
            else
            {
                Debug.Log("adding single"); 
                TickUpItem(pickup.pickupTypeIndex);
                UpdateUI();
            }
            if(other.gameObject != null)
            {
                Debug.Log("successful recycle");
                ObjectPool.instance.Recycle(other.gameObject);
                other.gameObject.SetActive(false);
                UpdateUI();
            }
            
        }

        Debug.Log("end of trigger");
        UpdateUI();

    }
}

