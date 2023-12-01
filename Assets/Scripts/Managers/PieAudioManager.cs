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


    public float footsteptime;
    public float counter = 0f;
    public float speed ;

    

    void Start()
    {
        jumpinstance = FMODUnity.RuntimeManager.CreateInstance(JumpEvent);
        hurtinstance = FMODUnity.RuntimeManager.CreateInstance(HurtEvent);
        stepinstance = FMODUnity.RuntimeManager.CreateInstance(StepEvent);
        
    }

   
    void FixedUpdate()
    {
        

        if (counter <= footsteptime )
            {
               counter+= 0.1f;
            }
        else if (characterController.IsMovingOnGround)
            {
                 Step();
                 counter = 0f;
            }
        
    }

    void Update ()
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

    private void Step()
    {
        stepinstance.start();
    }
}
