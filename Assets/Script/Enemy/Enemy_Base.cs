using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class is inherited in Enemy_Zombie Class's
[System.Serializable]
public class Enemy_Base : MonoBehaviour
{
    protected enum EnemyState
    {
        Ideal,
        ChaseHouse,
        ChasePlayer,
        Attack,
        Dead
    }

    [SerializeField] protected EnemyState enemyState;

    [Range(50.0f , 100.0f)]
    public float health = 100;

    [Range(10.0f, 100.0f)]
    [SerializeField] protected float damage = 10.0f;
    [Range(1.0f, 100.0f)]
    [SerializeField] protected float damageHouse = 1.0f;

    [SerializeField] protected int speed = 1;

    [Range(1.0f, 20.0f)]
    [SerializeField] protected float attack_Radius = 1.0f;

    [SerializeField] protected List<GameObject> target = new List<GameObject>();
    protected GameObject player;
    protected bool _isAttaking;

    public void TakeDamage(int damageamount)
    { 
        if(health > 0)
        {
            health -= damageamount;
        }
        
    }

    public void DamageEffect(SpriteRenderer sr)
    {
        sr.color = Color.red;
        StartCoroutine(DamageEffectReset(sr));
    }

    IEnumerator DamageEffectReset(SpriteRenderer sr)
    {
        yield return new WaitForSeconds(0.3f);
        sr.color = Color.white;
    }

}
