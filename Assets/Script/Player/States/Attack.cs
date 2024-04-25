using System;
using System.Collections;
using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.UIElements;
public class Attack : State
{
 public Attack(PlayerControler controller) : base("Attack")
  {
   this.controller = controller;
  }
   
   private PlayerControler controller;
   private float delay = 0.3f;
   private float delaySlash = 0.5f;

  public override void Enter()
   {
    base.Enter();
    // Handle animator
     controller.thisAnimator.SetTrigger("tAttack");
   }
  public override void Exit()
   {
    base.Exit();
   }
  public override void Update()
   {
    base.Update();
    // Return to idle
     if(!controller.attacking)
      {
       controller.stateMachine.ChangeState(controller.idleState);
        return;
      }
    // Start coroutine
     controller.StartCoroutine(DelayAttack());
     controller.effectActive = false;
      return;
   }

  private IEnumerator DelayAttack()
   {
    yield return new WaitForSeconds(delay);
    // Start whoosh sound
     controller.whoosh.Play();
    // Desable VFX slash
     controller.slash.SetActive(true);
    // Create attack
     controller.PerformAttack();
    // Reset vars
     controller.hasAttacked = false;
     controller.attacking = false;
      controller.StartCoroutine(DisableSlashe());
   }
  private IEnumerator DisableSlashe()
   {
    yield return new WaitForSeconds(delaySlash);
    controller.slash.SetActive(false);
    // Desable whoosh sound
     controller.whoosh.Stop();
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