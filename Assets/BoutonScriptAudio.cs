using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoutonScriptAudio : MonoBehaviour
{

    private FMOD.Studio.EventInstance ButtonInstance;
    public string ButtonEvent;

    public bool can = true;
    // Start is called before the first frame update
    void Start()
    {
                ButtonInstance = FMODUnity.RuntimeManager.CreateInstance(ButtonEvent);

        
    }
     void OnMouseOver()
    {
        if (can)
        {
         ButtonInstance.start();
         
         can = false;
        }
    }

     void OnMouseExit() 
     {
        can = true;
        
    }
}
