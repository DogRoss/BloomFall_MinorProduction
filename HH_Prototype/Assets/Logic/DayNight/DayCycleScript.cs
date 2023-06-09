using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DayCycleScript 
{
    public static bool daytime = true;
    public static float currentTime = 0;
    public static float dayLength = 300f;
    public static float nightLength = 420f;
    public static float lerpSmoothing = 0.3f;
    public static Light sun;

    public static void OnUpdate(float deltaTime)
    {
        currentTime += Time.deltaTime;
        if(daytime == true)
        {
            if (currentTime > dayLength)
            {
                currentTime = 0;
                daytime = !daytime;
            }
        }
        else 
        {
            if (currentTime > nightLength)
            {
                currentTime = 0;
                daytime = !daytime;
            }
        }
        if(Input.GetKeyDown(KeyCode.P))
        {
            currentTime = 0;
            daytime = !daytime;
            Debug.Log(daytime);
        }

        //End of debug
        if (daytime == false)
        {
            sun.intensity = Mathf.Lerp(0.5f, 0, currentTime / (nightLength / 2));
            if(sun.intensity == 0)
            {
                sun.intensity = Mathf.Lerp(0, 0.5f, (currentTime - (nightLength / 2)) / (nightLength / 2));
            }
            
        }
        else
        {
            sun.intensity = Mathf.Lerp(0.5f, 1f, currentTime / (dayLength / 2));
            if (sun.intensity == 1)
            {
                sun.intensity = Mathf.Lerp(1, 0.5f, (currentTime - (dayLength / 2)) / (dayLength / 2));
            }
        }
    }
}
