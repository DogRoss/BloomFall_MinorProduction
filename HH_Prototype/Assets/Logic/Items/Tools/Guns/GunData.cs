using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Gun",menuName="Weapon/Gun") ]
public class GunData : ScriptableObject
{
    public enum GunType
    {
        Auto, //hold = continuous fire
        Semi, //fire every mouse click
        Charge, //hold for set time then fire
    }


    [Header("Info")]
    public new string name;
    public bool unlocked = true;
    public GunType gunType;
    public Vector3 gunOffset = Vector3.zero;

    [Header("Shooting")]
    public float damage;
    public float maxDistance;
    public float spread;
    public float bulletsPerShot;
    public float fireRate; //rounds per minute
    public bool silenced;

    [Header("Ammo")]
    public int currentAmmo; //how many bullets are still expendable
    public int magSize; //max amount of ammo currentAmmo can hold
    public float reloadTime;

    [HideInInspector]
    public bool reloading;

}
