using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageStateMachine : MonoBehaviour
{
    
    public enum DamageType
    {
        Fire,
        Physical
    }

    private DamageType dmgType;

    public void SetDMGType(int i)
    {
        switch (i)
        {
            case 0:
                dmgType = DamageType.Fire;
                break;

            case 1:
                dmgType = DamageType.Physical;
                break;

            default:
                dmgType = DamageType.Physical;
                break;
        }
    }

    public DamageType DmgType
    {
        get
        {
            return dmgType;
        }
    }
}
