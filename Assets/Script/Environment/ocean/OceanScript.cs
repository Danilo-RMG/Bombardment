using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OceanScript : MonoBehaviour
{
    void Start()
    {  
    }
    void Update()
    { 
    }
    
    void OnTriggerEnter(Collider other)
     {
      GameObject otherObject = other.gameObject;
       if(otherObject.CompareTag("Player"))
        {
         GameManager.Instance.EndGame();
        }
     }
}
