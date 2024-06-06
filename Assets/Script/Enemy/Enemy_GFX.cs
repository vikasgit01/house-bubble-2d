using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

//This Script is on Emeny_GFX
public class Enemy_GFX : MonoBehaviour
{
    [SerializeField] private AIPath aiPath;

    void Update()
    {
        if (aiPath.desiredVelocity.x >= 0.01f)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else if (aiPath.desiredVelocity.x <= 0.01f)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);

        }
    }
}
