using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
   
    private static float timeRemaining = 600;
    void Update()
    {
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
        }
    }

    public float getTimeRemaining(){
        return timeRemaining;
    }
}
