using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieAudioManager : MonoBehaviour
{
    private FMOD.Studio.EventInstance jumpinstance;
    private FMOD.Studio.EventInstance hurtinstance;
    private FMOD.Studio.EventInstance stepinstance;
    private FMOD.Studio.EventInstance walljumpinstance;
    private FMOD.Studio.EventInstance DoubleJumpinstane;
    private FMOD.Studio.EventInstance Dieinstane;
    private FMOD.Studio.EventInstance UIselectinstance;
    

    public string JumpEvent;
    public string HurtEvent;
    public string StepEvent;
    public string WallJumpEvent;
    public string DoubleJumpEvent;
    public string DieEvent;

    public string UISelectEvent;
    

    public PlayerCharacterController characterController;
    public PlayerAbilities playerAbilities;

    public PlayerScript playerScript;

    public MusicIntensity musicIntensity;


    public float footsteptime;
    public float counter = 0f;
    private bool CanWallsoud = true;

    public Animator _anim;

   
 

    

    void Start()
    {
        jumpinstance = FMODUnity.RuntimeManager.CreateInstance(JumpEvent);
        hurtinstance = FMODUnity.RuntimeManager.CreateInstance(HurtEvent);
        stepinstance = FMODUnity.RuntimeManager.CreateInstance(StepEvent);
        walljumpinstance = FMODUnity.RuntimeManager.CreateInstance(WallJumpEvent);
        DoubleJumpinstane = FMODUnity.RuntimeManager.CreateInstance(DoubleJumpEvent);
        Dieinstane = FMODUnity.RuntimeManager.CreateInstance(DieEvent);
        UIselectinstance = FMODUnity.RuntimeManager.CreateInstance(UISelectEvent);
        
        
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
         if (Input.GetKeyDown("space"))
        {
            if (characterController.CurrentCollisions.Below)
            {
              jumpinstance.start();
            }
           
            if (playerAbilities.ability == AbilityEnum.DoubleJump
                && playerScript._jumpCount == 1)
            {
              DoubleJumpinstane.start();
            }

            if (playerAbilities.ability == AbilityEnum.WallJump
                &&characterController.CurrentCollisions.WallLeft
                )

            {
               jumpinstance.start();
            }

            if (playerAbilities.ability == AbilityEnum.WallJump
                &&characterController.CurrentCollisions.WallRight
                )

            {
               jumpinstance.start();
            }


        }


        if (playerAbilities.ability == AbilityEnum.WallJump)
        {

          if (characterController.CurrentCollisions.WallRight ||
              characterController.CurrentCollisions.WallLeft )
          {

            if (CanWallsoud && characterController.IsMovingOnGround == false)
            {
              walljumpinstance.start();
             // _anim.SetBool("WallSlide", true);
              CanWallsoud = false;
              
            }
              
            
          }

          if (characterController.CurrentCollisions.WallRight == false &&
              characterController.CurrentCollisions.WallLeft == false 
              || characterController.IsMovingOnGround)
          {
              walljumpinstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
             // _anim.SetBool("WallSlide", false);
              CanWallsoud = true;
              //Debug.Log("WallSlide");
             
          }
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

    public void Jump()
    {
        jumpinstance.start();
    }

    public void DieWarp()
    {
        musicIntensity.DIE = true;
        Dieinstane.start();
    }
    public void UiSelect()
    {
        UIselectinstance.start();
    }
}
