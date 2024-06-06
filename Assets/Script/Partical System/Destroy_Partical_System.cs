using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This Script is on MouseClickEffect Prefab
public class Destroy_Partical_System : MonoBehaviour
{
    void Start()
    {
        Destroy(this.gameObject, 1f);
    }

    
}
