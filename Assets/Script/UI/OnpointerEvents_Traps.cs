using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;


public class OnpointerEvents_Traps : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public void OnPointerEnter(PointerEventData eventData)
    {

        Player.Instance.canShoot = false;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Player.Instance.canShoot = true;
    }
}
