using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Melee", menuName = "Weapon/Melee")]
public class MeleeData : ScriptableObject
{
    public enum MeleeType
    {
        Single, 
        Auto,
    }

    [Header("Info")]
    public new string name;
    public bool unlocked = true;
    public MeleeType meleeType;
    
    public Vector3 offset = Vector3.zero; //position of weapon on player

    [Header("Attack")]
    public float damage;
    public float timeInBetweenSwings; //how much time needs to pass before you can swing again 
    public float hangTime; //time after swinging before putting weapon back down

    [HideInInspector]
    public bool swinging;

    
}
