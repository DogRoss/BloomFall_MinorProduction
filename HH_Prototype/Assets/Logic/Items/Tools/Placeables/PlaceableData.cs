using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Placeable", menuName = "Weapon/Placeable")]
public class PlaceableData : ScriptableObject
{
    [Header("Info")]
    public new string name;
    public bool canBeEquipped = true;
    public DamageStateMachine.DamageType dmgType;


    [Header("Gun")]
    public GunData gunData;
    public float timeBetweenShots;

    [Header("GFX")]
    public MeshFilter standGFX;
    public Vector3 standOffset;
    public Vector3 gunOffset;

}
