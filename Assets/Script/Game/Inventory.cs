using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
//This Script is attached to Shop Panel

public class Inventory : MonoBehaviour
{
    public static Inventory instance;

    public static event Action<Animator> inventoryOpen;
    public static event Action<Animator> inventoryClose;

    [SerializeField] public Animator inventoryAnimator;

    [HideInInspector] public bool canOpen;
    [HideInInspector] public bool canPlaceTrap;

    [HideInInspector] public bool isOpen;

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

    private void Start()
    {
        isOpen = false;

    }

    private void Update()
    {
        #region Inventory Open And Close

        if (Input.GetKeyDown(KeyCode.Tab) && canOpen)
        {
            if (isOpen)
            {
                inventoryClose?.Invoke(inventoryAnimator);
                isOpen = false;
                Player.Instance.canMove = true;
            }
            else
            {
                inventoryOpen?.Invoke(inventoryAnimator);
                isOpen = true;
                Player.Instance.canMove = false;
            }
        }

        #endregion

    }

    public bool IsOpen()
    {
        return isOpen;
    }
}
