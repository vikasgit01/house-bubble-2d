using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using My.Utils;

public class Testing : MonoBehaviour
{
    public delegate void DealDamage(float damage);
    public static DealDamage dealDamageEvent;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (dealDamageEvent != null)
            {
                //dealDamageEvent(5);
            }
        }
    }

}
