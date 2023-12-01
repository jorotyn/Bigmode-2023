using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieAudioManager : MonoBehaviour
{
    private FMOD.Studio.EventInstance jumpinstance;
    private FMOD.Studio.EventInstance hurtinstance;

    public string JumpEvent;
    public string HurtEvent;

    public PlayerCharacterController characterController;

    void Start()
    {
        jumpinstance = FMODUnity.RuntimeManager.CreateInstance(JumpEvent);
        hurtinstance = FMODUnity.RuntimeManager.CreateInstance(HurtEvent);
        
    }

   
    void Update()
    {
         if (Input.GetKeyDown("space") && characterController.CurrentCollisions.Below)
        {
            jumpinstance.start();
           
        }
        
    }

    public void Hurt()
    {
        hurtinstance.start();
    }
}
