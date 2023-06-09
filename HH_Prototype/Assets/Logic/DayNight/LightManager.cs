using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightManager : MonoBehaviour
{
    public Light sun;
    public float dayCycleLength = 300f;
    public float nightCycleLength = 420f;
    public float lerpSmoothing = 0.3f;
    public float currentTime = 0;
    public bool day;

    // Start is called before the first frame update
    void Awake()
    {
        DayCycleScript.sun = sun;
        GetComponent<UpdateHandler>().SubscribedToUpdate(DayCycleScript.OnUpdate);
        DayCycleScript.dayLength = dayCycleLength;
        DayCycleScript.nightLength = nightCycleLength;
        DayCycleScript.lerpSmoothing = lerpSmoothing;
    }

    private void Update()
    {
        day = DayCycleScript.daytime;
        currentTime = DayCycleScript.currentTime;
    }
}
