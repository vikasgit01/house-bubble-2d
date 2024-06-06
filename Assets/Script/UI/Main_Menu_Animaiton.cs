using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
using UnityEngine.EventSystems;

//This script is attached to Buttons in main menu

public class Main_Menu_Animaiton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{


    public void OnPointerEnter(PointerEventData eventData)
    {
            transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
            FindObjectOfType<Audio_Manager>().Play("ButtonHover");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        
            transform.localScale = new Vector3(1f, 1f, 1f);
    }
}
