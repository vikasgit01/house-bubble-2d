using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level_01 : MonoBehaviour
{
    public static Level_01 instance;

    public  int startArrows = 8;

    private void Awake()
    {

        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }

    }

}
