using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    Cinemachine.CinemachineImpulseSource source;
    public AudioSource audioClip;
    public GameObject VFX_Impact;
    private float cooldown = 0;

     // Knockback props
    private float knockbackForce = 25f;
    private float rangeInDegrees = 45f;

    private void OnTriggerEnter(Collider other)
     {
      GameObject hitObject = other.gameObject;
       if(hitObject.CompareTag("Bomb")) 
        {
        Debug.Log(hitObject);
         Rigidbody bombRigid = hitObject.GetComponent<Rigidbody>();
         source = GetComponent<Cinemachine.CinemachineImpulseSource>();
         audioClip = GetComponent<AudioSource>();
         // Calculate impulseVector
          Vector3 impulseDirection = (hitObject.transform.position - transform.position).normalized;
          impulseDirection.y += rangeInDegrees / 35f;
          impulseDirection = impulseDirection.normalized * knockbackForce;
         // Apply force
          bombRigid.AddForce(impulseDirection, ForceMode.Impulse);
           source.GenerateImpulse(Camera.main.transform.forward);
            Instantiate(VFX_Impact, hitObject.transform.position, transform.rotation); 
             audioClip.Play();
        }
     }
    void Update()
    {
     cooldown += Time.deltaTime;
    // Destroy hitbox obj
     if(cooldown > 0.5f) 
      {
       Destroy(this.gameObject);
      }
    }
}
