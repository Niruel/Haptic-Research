using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagement : MonoBehaviour
{

    bool exitTrigger = false;

    public float timer;
    private void Update()
    {
        if (exitTrigger)
        {
            StartTimer();
        }
        
    }

    private void OnTriggerStay(Collider other)
    {
        
    }
    private void OnTriggerExit(Collider other)
    {

        if (other.name == "StartPos")
        {
           
            exitTrigger = true;
            Debug.Log(other.name + " Exit");
        }
         
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.name=="EndPos")
        {
            exitTrigger=false;
            StopTimer();
            Debug.Log(other.name + " Enter");
        }
        
    }
    void StartTimer()
    {
        timer+=Time.deltaTime;
        Debug.Log(timer);
    }
    void StopTimer()
    {
        if (timer!=0f)
        {
           // timer = 0f;
            Debug.Log(timer);
        }
    }
}
