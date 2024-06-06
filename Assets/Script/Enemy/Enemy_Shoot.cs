using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Shoot : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed = 20f;

    private Enemy_Zombie01 _enemyZombie01;
    private Enemy_Zombie02 _enemyZombie02;
    private GameObject _target;

    private void Start()
    {
        _enemyZombie01 = GetComponentInParent<Enemy_Zombie01>();
        _enemyZombie02 = GetComponentInParent<Enemy_Zombie02>();
    }

    private void Update()
    {
        if (_enemyZombie01 != null)
        {
            _target = _enemyZombie01.Shoot();
        }
        else
        {
            _target = _enemyZombie02.Shoot();
        }
    }

    public void Shoot()
    {
        if (_target != null)
        {
            GameObject bullet = Instantiate(bulletPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation);
            Rigidbody2D _rb = bullet.GetComponent<Rigidbody2D>();
            Vector2 moveDir = (_target.transform.position - bullet.transform.position).normalized * bulletSpeed;
            _rb.velocity = new Vector2(moveDir.x, moveDir.y);
        }
    }
}
