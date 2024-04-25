using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Life : MonoBehaviour
{
    public int maxHealth;
    [HideInInspector] public int Health;
    
    void Start()
    {
     Health = maxHealth;
    }
}