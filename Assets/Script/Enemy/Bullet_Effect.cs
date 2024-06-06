using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script is on the Bullet Effect Prefab;
public class Bullet_Effect : MonoBehaviour
{
    
    void Start()
    {
        Destroy(gameObject, 0.3f);
    }

    
}
