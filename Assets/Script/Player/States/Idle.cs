using UnityEngine;
public class Idle : State
{
   private PlayerControler controler;
 public Idle(PlayerControler controler) : base("Idle")
  {
   this.controler = controler;
  }
  public override void Enter()
   {
    base.Enter();
   }
  public override void Exit()
   {
    base.Exit();
   }
  public override void Update()
   {
    base.Update();
    //Switch to Attack
     if(controler.attacking)
      {
        controler.stateMachine.ChangeState(controler.attackState);
        return;
      }
    //Switch to Jump
     if(controler.hasJumpInput)
      {
        controler.stateMachine.ChangeState(controler.jumpState);
        return;
      }
    //Switch to Walking
     if(!controler.movementVector.IsZero())
      {
        controler.stateMachine.ChangeState(controler.walkingState);
        return;
      }

    // Control camera
     controler.rotateOnMove = true;

    // Gradually reduce velocity towards zero
     controler.MyRigid.velocity -= controler.MyRigid.velocity * controler.speedDamping * Time.deltaTime;

    // Ensure velocity doesn't become negative
     if (controler.MyRigid.velocity.magnitude < 0.01f)
      {
       controler.MyRigid.velocity = Vector3.zero;
      }
   }
  public override void LateUpdate()
   {
    base.LateUpdate();
   }
  public override void FixedUpdate()
   {
    base.FixedUpdate();
   } 

}
