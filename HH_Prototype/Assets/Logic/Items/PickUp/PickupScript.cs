using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupScript : MonoBehaviour
{
    public enum PickupType
    {
        pBullet, fBullet, molotov, fTrap, gasoline, metal, bottle, cloth, wood
    }
    public PickupType pickupType;
    public int pickupTypeIndex;
    public bool pickupMultiple = true;
    public int pickupAmount;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (pickupType)
        {
            case PickupType.pBullet:
                pickupTypeIndex = 0;
                break;
            case PickupType.fBullet:
                pickupTypeIndex = 1;
                break;
            case PickupType.molotov:
                pickupTypeIndex = 2;
                break;
            case PickupType.fTrap:
                pickupTypeIndex = 3;
                break;
            case PickupType.gasoline:
                pickupTypeIndex = 4;
                break;
            case PickupType.metal:
                pickupTypeIndex = 5;
                break;
            case PickupType.bottle:
                pickupTypeIndex = 6;
                break;
            case PickupType.cloth:
                pickupTypeIndex = 7;
                break;
            case PickupType.wood:
                pickupTypeIndex = 8;
                break;
        }
    }

    
}
