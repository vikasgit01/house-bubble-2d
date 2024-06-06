using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using My.Utils;
using System;

//This script is on bubble GameObject
public class Player : MonoBehaviour
{
    public static Player Instance;

    [SerializeField] private Transform bow;
    [SerializeField] private GameObject bowGfx;

    [HideInInspector] public bool canShoot;
    [HideInInspector] public bool playerDead;
    [HideInInspector] public bool canMove;

    public event Action gameOver;


    public float playerHealth = 100;

    private Vector3 _mousePoint;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        UI.instance.SetMaxHealth(playerHealth);
    }

    private void Update()
    {

        #region BowRoatation
        BowRotation();
        #endregion

        #region Dead
        PlayerDead();
        #endregion
    }

    private void BowRotation()
    {
        _mousePoint = Utils.GetMouseWorldPosition();
        Vector2 aimDir = _mousePoint - transform.position;
        float ang = Mathf.Atan2(aimDir.y, aimDir.x) * Mathf.Rad2Deg;
        bow.rotation = Quaternion.Euler(0f, 0f, ang);
        if (ang > 90 || ang < -90)
        {
            Vector3 scale = new Vector3(1f, -1f, 1f);
            bowGfx.transform.localScale = scale;
        }
        else
        {
            Vector3 scale = new Vector3(1f, 1f, 1f);
            bowGfx.transform.localScale = scale;
        }
    }

    private void PlayerDead()
    {
        if (playerHealth <= 0)
        {
            gameOver?.Invoke();
            playerDead = true;
            Destroy(gameObject);
        }
    }

    public void TakeDamage(float damageAmount)
    {
        CameraShake.instance.ShakeChamera(1.5f, 0.1f);
        playerHealth -= damageAmount;
        UI.instance.SetHealthBar(playerHealth);
    }


}
