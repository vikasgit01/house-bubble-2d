using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//This script is on Alien_01

public class Enemy_Zombie01 : Enemy_Base
{

    [SerializeField] private LayerMask player_layerMask;
    [SerializeField] private Transform enemy_Gfx;
    [SerializeField] private Animator ememy_animator;


    //enemy Health Bar
    [SerializeField] private Slider enemyhealthBar;
    [SerializeField] private Gradient enemyhealthBarGradient;
    [SerializeField] private Image enemyfillImage;

    private EnemyState _currentState;
    private bool _attackHouse;
    private bool _attackPlayer;
    private float _disPlayer;
    private float _disHouse;

    [HideInInspector] public SpriteRenderer sr;
    [HideInInspector] public bool gotHit;


    private void Start()
    {
        if (House_01.instance.gameObject != null)
        {
            GameObject tar = House_01.instance.gameObject;
            target.Add(tar);
        }
        if (Player.Instance.gameObject != null)
        {
            player = Player.Instance.gameObject;
        }

        _currentState = EnemyState.ChaseHouse;
        sr = GetComponentInChildren<SpriteRenderer>();
        EnemySetMaxHealth(health);
    }

    private void Update()
    {

        for (int i = 0; i < target.Count; i++)
        {
            if (target[i] != null && player != null)
            {
                if (_disHouse < _disPlayer)
                {
                    if (target[i].transform.position.x < transform.position.x)
                    {
                        enemy_Gfx.transform.localScale = new Vector3(-1f, 1f, 1f);
                    }
                    else
                    {
                        enemy_Gfx.transform.localScale = new Vector3(1f, 1f, 1f);
                    }
                }
                else
                {
                    if (player.transform.position.x < transform.position.x)
                    {
                        enemy_Gfx.transform.localScale = new Vector3(-1f, 1f, 1f);
                    }
                    else
                    {
                        enemy_Gfx.transform.localScale = new Vector3(1f, 1f, 1f);
                    }
                }

            }

        }


        #region CollisionCheck
        CollisionCheck();
        #endregion

        #region EnemyMovement
        EnemyMovement();
        #endregion

        #region Dead
        if (health <= 0)
        {
            FindObjectOfType<Audio_Manager>().Play("AlienDead");
            Spawn_Manager.instance.EnemiesKilled();
            Game_Manager.instance.CoinSpawner(this.transform);
            Destroy(this.gameObject);
        }
        #endregion

        #region DamageEffect
        if (gotHit)
        {
            DamageEffect(sr);
            gotHit = false;
        }
        #endregion

    }

    private void EnemyMovement()
    {
        for (int i = 0; i < target.Count; i++)
        {
            if (target[i] == null)
            {
                return;
            }
        }

        if (player == null)
        {
            return;
        }

        _disPlayer = Vector2.Distance(transform.position, player.transform.position);
        _disHouse = Vector2.Distance(transform.position, target[0].transform.position);

        if (_disPlayer <= attack_Radius || _attackHouse)
        {
            _currentState = EnemyState.Attack;
        }
        else if (_disHouse < _disPlayer)
        {
            _currentState = EnemyState.ChaseHouse;
        }
        else
        {
            _currentState = EnemyState.ChasePlayer;
        }

        switch (_currentState)
        {

            case EnemyState.Attack:
                _isAttaking = true;
                transform.position = transform.position;
                ememy_animator.SetBool("Attack", true);

                if (_disHouse > _disPlayer)
                {
                    _attackHouse = false;
                }

                return;

            case EnemyState.ChaseHouse:
                _isAttaking = false;
                ememy_animator.SetBool("Attack", false);
                transform.position = Vector3.MoveTowards(transform.position, target[0].transform.position, speed * Time.deltaTime);
                return;

            case EnemyState.ChasePlayer:
                _isAttaking = false;
                ememy_animator.SetBool("Attack", false);
                transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
                return;
        }

    }

    private void CollisionCheck()
    {
        Collider2D bubble_coll = Physics2D.OverlapCircle(transform.position, attack_Radius, player_layerMask);

        if (bubble_coll != null)
        {
            if (bubble_coll.gameObject.tag == "Player")
            {
                _attackPlayer = true;
                _attackHouse = false;
            }

            if (bubble_coll.gameObject.tag == "House" && _disHouse < _disPlayer)
            {
                _attackHouse = true;
                _attackPlayer = false;
            }
        }
    }

    public GameObject Shoot()
    {
        if (Player.Instance.playerDead == true || House_01.instance.housedead == true)
        {
            return null;
        }

        if (_attackHouse)
        {
            return target[0];
        }
        else if (_attackPlayer)
        {
            return player;
        }


        return null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, attack_Radius);
    }

    public void EnemySetMaxHealth(float health)
    {

        if (enemyhealthBar != null && enemyfillImage != null)
        {
            enemyhealthBar.maxValue = health;
            enemyhealthBar.value = health;

            enemyfillImage.color = enemyhealthBarGradient.Evaluate(1f);
        }
        else
        {
            return;
        }

    }

    public void EnemySetHealthBar(float health)
    {
        if (enemyhealthBar != null && enemyfillImage != null)
        {
            enemyhealthBar.value = health;
            enemyfillImage.color = enemyhealthBarGradient.Evaluate(enemyhealthBar.normalizedValue);
        }
        else
        {
            return;
        }
    }



}
