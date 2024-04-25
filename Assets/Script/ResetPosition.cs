using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPosition : MonoBehaviour
{
    Vector3 origin = new Vector3(0, 1.3f,0);
    Rigidbody myRigid;
    private float delay = 5f;
    private bool hasReset = false;
    void Start ()
     {
      myRigid = GetComponent<Rigidbody>();
     }
    void Update()
    {
     if(!hasReset)
     {
      StartCoroutine(Restdelay());
      hasReset = true;
      myRigid.velocity = Vector3.zero;
     }
    }
    private IEnumerator Restdelay()
     {
      yield return new WaitForSeconds(delay);
        transform.position = origin;
        hasReset = false;
     }
}
