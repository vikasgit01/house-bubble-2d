using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
//This Script is attached to house_01 GameObject

public class House_01 : House_Base
{
    public static House_01 instance;

    //House Health Bar
    [SerializeField] private Slider househealthBar;
    [SerializeField] private Gradient househealthBarGradient;
    [SerializeField] private Image houseFillImage;

    public List<GameObject> targetPoins = new List<GameObject>();

    [HideInInspector] public bool housedead;

    public event Action gameOver;


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
        househealthBar.value = health;
        houseFillImage.color = househealthBarGradient.Evaluate(househealthBar.normalizedValue);
    }

    private void Update()
    {
        if (health <= 0)
        {
            gameOver?.Invoke();
            housedead = true;
            Destroy(gameObject);
        }

    }

    public float HouseHealth()
    {
        return health;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        househealthBar.value = health;
        houseFillImage.color = househealthBarGradient.Evaluate(househealthBar.normalizedValue);
    }



}
