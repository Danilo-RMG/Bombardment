using UnityEngine;
public class Jump : State
{
   private PlayerControler controler;
   private bool hasJumping;
   private float cooldown;
 public Jump(PlayerControler controler) : base("Jump")
  {
   this.controler = controler;
  }

  public override void Enter()
   {
    base.Enter();
    // Reset vars
     hasJumping = false;
     cooldown = 0.5f;
     // Handle animator
      controler.thisAnimator.SetBool("bJumping", true);
   }
  public override void Exit()
   {
    base.Exit();
    // Handle animator
     controler.thisAnimator.SetBool("bJumping", false);
   }
  public override void Update()
   {
    base.Update();
    //Update cooldown
     cooldown -= Time.deltaTime;

    //Switch to idle
     if(hasJumping && cooldown <= 0)
      {
       controler.stateMachine.ChangeState(controler.idleState);
       controler.hasJumped = false;
        return;
      }
   }
  public override void LateUpdate()
   {
    base.LateUpdate();
   }
  public override void FixedUpdate()
   {
    base.FixedUpdate();
     //Jump
     if(!hasJumping)
      {
       hasJumping = true;
        ApplyImpulse();
      }

    //Create vector
    Vector3 walkVector = new Vector3(controler.movementVector.x, 0f, controler.movementVector.y);
     walkVector = controler.GetForward() * walkVector;
      walkVector *= controler.Speed * controler.JumpMovementFactor;

    //Apply input to character
     controler.MyRigid.AddForce(walkVector, ForceMode.Force);
      controler.rotateOnMove = true;
   } 
   private void ApplyImpulse()
    {
     // Applay impulse
      Vector3 forceVector = Vector3.up * controler.Jumpforce;
      controler.MyRigid.AddForce(forceVector, ForceMode.Impulse);
    }

}
