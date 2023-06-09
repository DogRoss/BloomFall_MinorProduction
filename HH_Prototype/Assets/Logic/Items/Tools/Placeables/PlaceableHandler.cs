using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceableHandler : MonoBehaviour, IInteractable
{
    [Header("Placeable Information")]
    [SerializeField] Placeable placeable;
    public MeshFilter currentStandGFX;

    [SerializeField] bool equipped;
    [SerializeField] float timeBetweenClicks = 1f;
    public bool clicked;

    float mouse1Current = 0, mouse2Current = 0;
    float currentTime = 0;
    [Header(" ")]
    [SerializeField] bool debug = true;

    // Update is called once per frame
    void Update()
    {
        //iterates timer up
        currentTime += Time.deltaTime;

        if(currentTime > timeBetweenClicks)
        {
            if (debug && clicked)
            {
                Debug.Log("click reimbursed");
            }
            clicked = false;
        }

        if (placeable != null)
        {
            //runs selected shooting alg
            if (equipped && mouse1Current > 0 && !clicked)
            {
                clicked = true;
                currentTime = 0;

                if (debug)
                {
                    Debug.Log("clicked");
                }

                if(currentStandGFX != null)
                {
                    currentStandGFX.gameObject.SetActive(true);
                }

                PlaceObject();
            }

            //TODO: counts down timer when you place
            //live updates localpos
            if (debug)
            {
                //transform.localPosition = placeableData.standOffset;
            }
        }
        else
        {
            if(currentStandGFX != null)
            {
                currentStandGFX.gameObject.SetActive(false);
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

    public void Equip(bool equip)
    {
        //Debug.Log("equip: " + equip);

        equipped = equip;

        if(currentStandGFX != null)
        {
            //Debug.Log("equip/dequip gfx");
            currentStandGFX.gameObject.SetActive(equip ? true : false);
        }
    }

    public void SetParentAndResetTransform(Transform tForm)
    {
        transform.parent = tForm;
        transform.localPosition = Vector3.zero;
    }

    public void SetUpPlaceable()
    {
        currentStandGFX = placeable.standGFX;
    }

    public void SetUpPlaceableWithTransform(Transform tForm)
    {
        SetParentAndResetTransform(tForm);
        SetUpPlaceable();
    }

    public bool Equipped
    {
        get
        {
            return equipped;
        }
    }

    public Placeable Placeable
    {
        get
        {
            return placeable;
        }
    }

    //places object
    void PlaceObject()
    {
        if (debug)
        {
            Debug.Log("enter PlaceObject");
        }

        if (Inventory.instance.TrapAmount > 0)
        {
            Inventory.instance.TickDownItem(3);
            placeable.Place(this);
        }
    }
}
