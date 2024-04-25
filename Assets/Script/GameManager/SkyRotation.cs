using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SkyRotation : MonoBehaviour
{
  void Update()
   {
    var Manager = GameManager.Instance;
     if(GameManager.Instance.isGameOver || GameManager.isPaused)
      {
       return;
      }
       float skySpeed = Manager.skySpeed * Time.deltaTime;
        transform.Rotate(0f, Manager.skySpeed, 0f);
   }
}
