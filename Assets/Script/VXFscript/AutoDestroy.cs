using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    private float cooldown = 0;
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
