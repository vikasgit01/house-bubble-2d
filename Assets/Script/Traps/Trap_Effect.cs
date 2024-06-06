using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script is attached to a Trap effect prefab

public class Trap_Effect : MonoBehaviour
{
    void Start()
    {
        Destroy(gameObject, 1f);
    }

   
}
