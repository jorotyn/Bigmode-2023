using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceCreamTruckMusic : MonoBehaviour
{

    private FMOD.Studio.EventInstance Truckinstance;
    public string TruckEvent;

    private float pan;
    


    
    void Start()
    {
        Truckinstance = FMODUnity.RuntimeManager.CreateInstance(TruckEvent);
        
        Truckinstance.start();
        
    }

    
    void Update()
    {
        
        pan = Mathf.Clamp((gameObject.transform.position.x + 10)/ 20 , -1, 1);

       
        // Debug.Log(pan);
        Truckinstance.setParameterByName("Pan", pan);

        if (pan < 0)
        {
            Truckinstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        }
    }

    
    void OnDestroy()
    {
        Truckinstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        
    }
}
