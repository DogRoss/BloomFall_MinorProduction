using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Script By Edgar
 * 
 * Desc: holds all equipable tools, manages tools with class ToolPooling
 */
public class ToolPooling : MonoBehaviour
{
    [SerializeField] List<GunHandler> guns = new List<GunHandler>();
    int lastGunInd = 0;
    [SerializeField] List<MeleeHandler> melees = new List<MeleeHandler>();
    int lastMeleeInd = 0;

    public int firstGunUnlockIndex = 0;
    public bool unlockGunAsFirst = false;

    public static ToolPooling instance;

    // Start is called before the first frame update
    private void Start()
    {
        instance = this;
        LockAll();

        guns[firstGunUnlockIndex].GunData.unlocked = unlockGunAsFirst ? true : false;
        
        //DisableAll();
    }


    public GunHandler GetNextGun()
    {
        if (guns == null)
        {
            return null;
        }


        if (lastGunInd + 1 < guns.Count)
        {
            Debug.Log("next one selected");
            lastGunInd++;
        }
        else
        {
            Debug.Log("Starting from the bottom");
            lastGunInd = 0;
        }

        return guns[lastGunInd];
    }

    public MeleeHandler GetNextMelee()
    {
        if (melees.Capacity > 0)
        {
            MeleeHandler melee;

            if (lastMeleeInd + 1 < melees.Count)
            {
                Debug.Log("next one selected");
                melee = melees[lastMeleeInd + 1];
                lastMeleeInd++;
            }
            else
            {
                Debug.Log("Starting from the bottom");
                melee = melees[0];
                lastMeleeInd = 0;
            }

            return melee;
        }

        return null;
    }

    //gets next gun that the player can hold
    public GunHandler GetNextUnlockedGun()
    {
        Debug.Log("enter");

        //if the guns list doesnt exist or no guns
        if (guns == null || guns.Count == 0 || !EquippableGunExists())
        {
            Debug.Log("returning gun doesnt exist");
            return null;
        }

        int startingInd = lastGunInd;

        Debug.Log("Amount of Guns in list: " + guns.Count);
        //finds next useable gun or hits end and restarts to first gun
        for (int i = lastGunInd; i < guns.Count; i++)
        {
            //if a gun has been found
            if (guns[i] != null && guns[i].GunData.unlocked)
            {
                Debug.Log("found gun: " + guns[i].GunData.name);
                lastGunInd = i;
                break;
            }
            //if end of the list is reached
            if (i >= guns.Count)
            {
                Debug.Log("reached list end, starting over");
                i = 0;
            }
            if (i == startingInd)
            {
                Debug.Log("couldnt find any weapon, requip normal if possible");
                lastGunInd = i;
                break;
            }
        }

        //increments to next in guns
        if (lastGunInd < guns.Count - 1)
            lastGunInd++;
        else
            lastGunInd = 0;

        if (guns[lastGunInd].GunData.unlocked)
        {
            //if the gun is equippable, return with it
            return guns[lastGunInd];
        }

        return null;
    }

    public MeleeHandler GetNextUnlockedMelee()
    {
        Debug.Log("enter");

        //if the guns list doesnt exist or no guns
        if (melees == null || melees.Count == 0 || !EquippableMeleeExists())
        {
            Debug.Log("returning melee doesnt exist");
            return null;
        }

        int startingInd = lastGunInd;
        

        Debug.Log("Amount of melees in list: " + melees.Count);
        //finds next useable gun or hits end and restarts to first gun
        for (int i = lastMeleeInd; i < melees.Count; i++)
        {
            //if a gun has been found
            if (melees[i] != null && melees[i])
            {
                Debug.Log("found melee");
                lastMeleeInd = i;
                break;
            }
            //if end of the list is reached
            if (i >= melees.Count)
            {
                Debug.Log("reached list end, starting over");
                i = 0;
            }
            if (i == startingInd)
            {
                Debug.Log("couldnt find any weapon, requip normal if possible");
                lastMeleeInd = i;
                break;
            }
        }

        //increments to next in guns
        if (lastMeleeInd < melees.Count - 1)
            lastMeleeInd++;
        else
            lastMeleeInd = 0;

        if (melees[lastMeleeInd].MeleeData.unlocked)
        {
            //if the gun is equippable, return with it
            return melees[lastMeleeInd];
        }

        return null;
    }

    public bool TryGetNextUnlockedGun(out GunHandler gun)
    {
        gun = GetNextUnlockedGun();

        if (gun != null)
        {
            return true;
        }
        return false;
    }

    public bool TryGetNextUnlockedMelee(out MeleeHandler melee)
    {
        melee = GetNextUnlockedMelee();

        if (melee != null)
        {
            return true;
        }
        return false;
    }

    public GunHandler GetGunOfIndex(int index)
    {
        lastGunInd = index;
        return guns[index];
    }

    public MeleeHandler GetMeleeOfIndex(int index)
    {
        lastMeleeInd = index;
        return melees[index];
    }

    public bool EquippableGunExists()
    {
        bool value = false;
        foreach (GunHandler gun in guns)
        {
            if (gun.GunData.unlocked)
            {
                value = true;
                break;
            }
            Debug.Log(gun.name);
        }
        Debug.Log("is equippable: " + value);
        return value;
    }

    public bool EquippableMeleeExists()
    {
        bool value = false;
        foreach (MeleeHandler melee in melees)
        {
            if (melee.MeleeData.unlocked)
            {
                value = true;
                break;
            }
        }

        return value;
    }

    public List<GunHandler> Guns
    {
        get
        {
            return guns;
        }
    }





    public void LockAll()
    {
        foreach (GunHandler gun in guns)
        {
            gun.GunData.unlocked = false;
        }
        foreach (MeleeHandler melee in melees)
        {
            melee.MeleeData.unlocked = false;
        }
    }

    public void CheckAndUnlockMelee()
    {
        if (Inventory.instance.MetalAmount >= 2)
        {
            melees[0].MeleeData.unlocked = true;
            Inventory.instance.AddAmountToItem(-2, 5);
        }
    }
    public void CheckAndUnlockSemi()
    {
        if (Inventory.instance.MetalAmount >= 4)
        {
            guns[0].GunData.unlocked = true;
            Inventory.instance.AddAmountToItem(-4, 5);
        }
    }
    public void CheckAndUnlockAuto()
    {
        if (Inventory.instance.MetalAmount >= 6)
        {
            guns[1].GunData.unlocked = true;
            Inventory.instance.AddAmountToItem(-6, 5);
        }
    }
}
