using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieAudioManager : MonoBehaviour
{
    private FMOD.Studio.EventInstance jumpinstance;
    private FMOD.Studio.EventInstance hurtinstance;
    private FMOD.Studio.EventInstance stepinstance;

    public string JumpEvent;
    public string HurtEvent;
    public string StepEvent;

    public PlayerCharacterController characterController;

    void Start()
    {
        jumpinstance = FMODUnity.RuntimeManager.CreateInstance(JumpEvent);
        hurtinstance = FMODUnity.RuntimeManager.CreateInstance(HurtEvent);
        
    }

   
    void Update()
    {
         if (InputManager.JumpPressed() && characterController.CurrentCollisions.Below)
        {
            jumpinstance.start();
           
        }
        
    }

    public void Hurt()
    {
        hurtinstance.start();
    }
}
