using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System.Linq;
using UnityEngine.UI;

public class Enemy_Zombie02 : Enemy_Base
{

    [SerializeField] private LayerMask player_layerMask;
    [SerializeField] private Transform enemy_Gfx;
    [SerializeField] private Animator ememy_animator;


    //enemy Health Bar
    [SerializeField] private Slider enemyhealthBar;
    [SerializeField] private Gradient enemyhealthBarGradient;
    [SerializeField] private Image enemyfillImage;

    private AIPath _aiPath;
    private EnemyState _currentState;
    private bool _attackHouse;
    private bool _attackPlayer;
    private bool _isDead;
    private float _disPlayer;
    private float _disHouse;
    private float _dis;
    private float _previousDis;
    private int _houseIndex;
    private AIDestinationSetter _aiDestinationSetter;

    [HideInInspector] public SpriteRenderer sr;
    [HideInInspector] public bool gotHit;

    private int _attackAnimatorId;
    private int _deadAnimatorId;

    private void Start()
    {
        if (House_01.instance.gameObject != null)
        {
            foreach (var tar in House_01.instance.targetPoins)
            {
                target.Add(tar);
            }
        }
        if (Player.Instance.gameObject != null)
        {
            player = Player.Instance.gameObject;
        }

        _aiDestinationSetter = GetComponent<AIDestinationSetter>();
        _aiPath = GetComponent<AIPath>();
        _currentState = EnemyState.ChaseHouse;
        sr = GetComponentInChildren<SpriteRenderer>();
        EnemySetMaxHealth(health);
        _aiDestinationSetter.target = House_01.instance.transform;
        _attackAnimatorId = Animator.StringToHash("Attack");
        _deadAnimatorId = Animator.StringToHash("Dead");
        _isDead = false;
    }

    private void Update()
    {

        #region FlipGFX
        Flip();
        #endregion

        #region CollisionCheck
        CollisionCheck();
        #endregion

        #region EnemyMovement
        EnemyMovement();
        #endregion

        #region DamageEffect
        if (gotHit)
        {
            DamageEffect(sr);
            gotHit = false;
        }
        #endregion

        #region Dead
        Dead();
        #endregion

    }

    private void Dead()
    {
        if (health <= 0 && !_isDead)
        {
            _isDead = true;
            ememy_animator.SetBool(_deadAnimatorId, true);
            FindObjectOfType<Audio_Manager>().Play("AlienDead");
            GetComponent<BoxCollider2D>().enabled = false;
            GetComponent<AIPath>().enabled = false;
            GetComponent<Seeker>().enabled = false;
            GetComponent<AIDestinationSetter>().enabled = false;
            enemyhealthBar.gameObject.SetActive(false);
            Spawn_Manager.instance.EnemiesKilled();
            StartCoroutine(DeadWaitTime());
        }
    }

    IEnumerator DeadWaitTime()
    {
        yield return new WaitForSeconds(1f);
        Game_Manager.instance.CoinSpawner(this.transform);
        Destroy(this.gameObject);
    }

    private void Flip()
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
        for (int i = 0; i < target.Count; i++)
        {
            _dis = Vector2.Distance(transform.position, target[i].transform.position);
            if (i == 0)
            {
                _previousDis = _dis;
            }

            if (_previousDis < _dis)
            {
                _disHouse = _previousDis;
                _previousDis = _dis;
            }
            else if (_previousDis > _dis)
            {
                if (_disHouse > _dis)
                {
                    _disHouse = _dis;
                }
                _previousDis = _dis;
            }

            if (_disHouse == _dis)
            {
                _houseIndex = i;
            }
        }


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
               ememy_animator.SetBool(_attackAnimatorId, true);

                if (_disHouse > _disPlayer)
                {
                    _attackHouse = false;
                }
                return;

            case EnemyState.ChaseHouse:
                _isAttaking = false;
                ememy_animator.SetBool(_attackAnimatorId, false);
                _aiDestinationSetter.target = target[_houseIndex].transform;
                return;

            case EnemyState.ChasePlayer:
                _isAttaking = false;
                ememy_animator.SetBool(_attackAnimatorId, false);
                _aiDestinationSetter.target = Player.Instance.transform;
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
            return target[_houseIndex];
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
