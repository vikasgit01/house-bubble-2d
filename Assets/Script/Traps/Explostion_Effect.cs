using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script is attache to Explostion effect prefab

public class Explostion_Effect : MonoBehaviour
{
    void Start()
    {
        Destroy(gameObject, 0.5f);
    }
}
