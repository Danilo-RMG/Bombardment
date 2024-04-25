using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipRotate : MonoBehaviour
{
  public float degreesPerSecond = 90f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
      if(GameManager.isPaused)
      {
        return;
      }
       else
        {
         float stepY = degreesPerSecond * Time.deltaTime;
         transform.Rotate(0, stepY, 0);
        }
    }
}
