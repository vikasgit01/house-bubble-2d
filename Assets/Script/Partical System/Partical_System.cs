using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using My.Utils;

//This script is on GameManager GameObject
public class Partical_System : MonoBehaviour
{
    [SerializeField] private GameObject mouseClickEffect;


    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            MouseClickEffect();
        }
    }

    public void MouseClickEffect()
    {
        Vector3 _mousePoint = Utils.GetMouseWorldPosition();
        Instantiate(mouseClickEffect, _mousePoint, transform.rotation);
    }


}
