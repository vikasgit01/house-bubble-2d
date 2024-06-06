using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing_Events : MonoBehaviour
{
  
    private void Start()
    {
        //Testing.dealDamageEvent += TrapDamage;
    }

    public void TrapDamage(float damage)
    {
        Debug.Log(damage);
    }

}
