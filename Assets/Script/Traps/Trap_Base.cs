using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Trap_Base : MonoBehaviour
{
    [SerializeField] protected float damage;

    [SerializeField] protected float explodTime;

    [SerializeField] protected Slider traptimerbar;
    [SerializeField] protected Image trapfillImage;


    protected enum TrapType
    {
        bomb,
        tnt,
        tnt2
    }

    [SerializeField] protected TrapType traptype;

    protected void TrapSetMaxHealth(float time)
    {

        if (traptimerbar != null && trapfillImage != null)
        {
            traptimerbar.maxValue = time;
            traptimerbar.value = time;
        }
        else
        {
            return;
        }

    }

    protected void TrapSetHealthBar(float time)
    {
        if (traptimerbar != null && trapfillImage != null)
        {
            traptimerbar.value = time;
        }
        else
        {
            return;
        }
    }
}
