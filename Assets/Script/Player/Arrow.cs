using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

//This script is on the Arrow_GFX prefab
public class Arrow : MonoBehaviour
{
    private float _arrowSpeed = 20.0f;
    private int arrowDamage = 50;
    private bool _hitCollider;

    private void Start()
    {
        Destroy(gameObject, 1.0f);
        _hitCollider = false;
    }

    private void Update()
    {
        if (!_hitCollider)
        {
            transform.Translate(Vector2.up * _arrowSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Alien_01")
        {
            DamageEnemy(collision);
            Destroy(gameObject);
        }

        if (collision.gameObject.tag == "House")
        {
            transform.position = transform.position;
            _hitCollider = true;
        }

        if (collision.gameObject.tag == "Alien_02")
        {
            DamageEnemy(collision);
            Destroy(gameObject);
        }


    }

    private void DamageEnemy(Collider2D coll)
    {
        Enemy_Zombie01 ez1 = coll.gameObject.GetComponent<Enemy_Zombie01>();
        Enemy_Zombie02 ez2 = coll.gameObject.GetComponent<Enemy_Zombie02>();
        if (ez1 != null)
        {
            ez1.TakeDamage(arrowDamage);
            ez1.EnemySetHealthBar(ez1.health);
            ez1.gotHit = true;
        }else
        {
            ez2.TakeDamage(arrowDamage);
            ez2.EnemySetHealthBar(ez2.health);
            ez2.gotHit = true;
        }
    }


}
