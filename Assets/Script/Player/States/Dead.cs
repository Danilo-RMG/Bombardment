using UnityEngine;
using Cinemachine;
public class Dead : State
{
  public Dead(PlayerControler controller) : base("Dead")
  {
   this.controller = controller;
  }
   private PlayerControler controller;
  public override void Enter()
   {
    base.Enter();
    // Handle animator
     controller.thisAnimator.SetTrigger("tGameOver");
   }
  public override void Exit()
   {
    base.Exit();
   }
  public override void Update()
   {
    base.Update();
     if(GameManager.Instance.isGameOver)
      {
       controller.DeadCam.gameObject.SetActive(true);
       controller.normalCam.gameObject.SetActive(false);
      }
       else
        {
         controller.DeadCam.gameObject.SetActive(false);
         controller.normalCam.gameObject.SetActive(true);
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
