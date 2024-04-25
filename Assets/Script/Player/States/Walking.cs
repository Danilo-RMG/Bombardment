using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.Controls;
public class Walking : State
{
 public Walking(PlayerControler controler) : base("Walking")
  {
    this.controler = controler;
    MyRigid = controler.GetComponent<Rigidbody>();
  }
   private PlayerControler controler;
   private Rigidbody MyRigid;

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
    //Switch to Idle
     if(controler.movementVector.IsZero())
      {
        controler.stateMachine.ChangeState(controler.idleState);
      }
    // Control camera
     controler.rotateOnMove = false;
   }
  public override void LateUpdate()
   {
    base.LateUpdate();
   }
  public override void FixedUpdate()
   {
    base.FixedUpdate();

    //Create vector
     Vector3 walkVector = new Vector3(controler.movementVector.x, 0f, controler.movementVector.y);
     walkVector = walkVector.normalized;
      walkVector = controler.GetForward() * walkVector;
       walkVector *= controler.Speed;

    //Apply input to character
     controler.MyRigid.AddForce(walkVector, ForceMode.Force);
   }
}