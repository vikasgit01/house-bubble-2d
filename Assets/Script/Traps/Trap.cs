using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
//This script is Attached to All traps Prefab

public class Trap : Trap_Base
{
    private bool _isInTrap;
    private float _sliderTimer;
    private bool _startTimer;
    private bool _audioPlayed;

    private List<Enemy_Zombie01> enemyZombie = new List<Enemy_Zombie01>();
    private List<Enemy_Zombie02> enemyZombie2 = new List<Enemy_Zombie02>();

    private void Start()
    {
        _sliderTimer = explodTime;
        _audioPlayed = true;
        TrapSetMaxHealth(_sliderTimer);
    }

    private void Update()
    {
        if (_sliderTimer > 0 && _startTimer)
        {
            _sliderTimer -= Time.deltaTime;
            TrapSetHealthBar(_sliderTimer);
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Alien_01"))
        {
            if (other.GetComponent<Enemy_Zombie01>())
            {
                enemyZombie.Add(other.GetComponent<Enemy_Zombie01>());
            }

            if (other.GetComponent<Enemy_Zombie02>())
            {
                enemyZombie2.Add(other.GetComponent<Enemy_Zombie02>());
            }
            _isInTrap = true;
            StartCoroutine(ExplotionTimer());
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Alien_01"))
        {
            if (other.GetComponent<Enemy_Zombie01>())
            {
                enemyZombie.Remove(other.GetComponent<Enemy_Zombie01>());
            }
            if (other.GetComponent<Enemy_Zombie02>())
            {
                enemyZombie2.Remove(other.GetComponent<Enemy_Zombie02>());
            }

            if (enemyZombie.Count <= 0)
            {
                _isInTrap = false;
            }
        }
    }

    IEnumerator ExplotionTimer()
    {
        _startTimer = true;

        if (_audioPlayed)
        {
            if (traptype == TrapType.bomb)
            {
                FindObjectOfType<Audio_Manager>().Play("FuseEffectBomb");
            }
            else if (traptype == TrapType.tnt)
            {
                FindObjectOfType<Audio_Manager>().Play("FuseEffectTnt1");
            }
            else if (traptype == TrapType.tnt2)
            {
                FindObjectOfType<Audio_Manager>().Play("FuseEffectTnt2");
            }
            _audioPlayed = false;
        }

        yield return new WaitForSeconds(explodTime);

        FindObjectOfType<Audio_Manager>().Play("Explosion");
        Game_Manager.instance.TrapEffect(gameObject.transform);
        Game_Manager.instance.EffectDamage(gameObject.transform);

        if (_isInTrap)
        {
            foreach (var item in enemyZombie)
            {
                item.health -= damage;
                item.EnemySetHealthBar(item.health);
            }

            foreach(var item in enemyZombie2)
            {
                item.health -= damage;
                item.EnemySetHealthBar(item.health);
            }

            CameraShake.instance.ShakeChamera(5f, .2f);
            Destroy(transform.parent.gameObject);
        }
        else
        {
            CameraShake.instance.ShakeChamera(5f, .2f);
            Destroy(transform.parent.gameObject);
        }
    }

}
