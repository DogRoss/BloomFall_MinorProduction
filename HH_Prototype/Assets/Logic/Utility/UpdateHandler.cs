using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateHandler : MonoBehaviour
{
    public delegate void OnUpdate(float deltaTime);
    OnUpdate onUpdate;

    // Update is called once per frame
    void Update()
    {
        if(onUpdate != null)
        {
            onUpdate(Time.deltaTime);
        }
    }

    public void SubscribedToUpdate(OnUpdate method)
    {
        onUpdate += method;
    }
}
