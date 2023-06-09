using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class WeaponEvent : UnityEvent {}

public class WeaponPickUp : MonoBehaviour
{
    public WeaponEvent weaponEvent;

    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("trigger weapon event");
        weaponEvent.Invoke();
        gameObject.SetActive(false); 
    }
}
