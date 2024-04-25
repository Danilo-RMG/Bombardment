using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouyantScript : MonoBehaviour
{
  public float underwaterDrag = 3f;
  public float underwaterAngularDrag = 1f;
  public float airDrag = 0f;
  public float airAngularDrag = 0.05f;
  public float bouyancyForce = 10;
  private Rigidbody myRigid;
  private bool hasTouchedWater;
   void Awake()
    {
     myRigid = GetComponent<Rigidbody>();
    }
   void FixedUpdate()
    {
     // Check if underwater
      float diffy = transform.position.y;
      bool isUnderWater = diffy < 0;
      if(isUnderWater)
       {
        hasTouchedWater = true;
       }
     // Ignore if never touched water
      if(!hasTouchedWater)
       {
        return;
       }
     // Buoyancy logic
      if(isUnderWater)
       {
        Vector3 vector = Vector3.up * bouyancyForce * -diffy;
         myRigid.AddForce(vector, ForceMode.Acceleration);
       }
        myRigid.drag = isUnderWater ? underwaterDrag : airDrag;
        myRigid.angularDrag = isUnderWater ? underwaterAngularDrag : airAngularDrag;
    }

}
