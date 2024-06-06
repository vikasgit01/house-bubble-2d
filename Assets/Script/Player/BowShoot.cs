using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Cinemachine;
//This Script is attached to the Bow_Gfx

public class BowShoot : MonoBehaviour
{
    public static BowShoot instance;

    public static event Action<int> arrowCount;


    [SerializeField] private Transform arrow;
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private float shootingResetTime = 0.3f;
   

    private int _totalArrow;

    private Animator _bowAnimator;
    private float _coolDownTime;
    

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

    void Start()
    {
       
        _bowAnimator = GetComponent<Animator>();
        arrowCount?.Invoke(_totalArrow);

    }

    void Update()
    {

        #region Arrow shoot Cooldown
        if (_coolDownTime > 0)
        {
            _coolDownTime -= Time.deltaTime;
        }

        #endregion

        #region Bow Shoot Animation
        if (Input.GetMouseButtonDown(0) && Player.Instance.canShoot && _coolDownTime <= 0)
        {
            if (_totalArrow > 0)
            {
                _bowAnimator.SetBool("Attack", true);
            }
        }
        #endregion

    }

    private void ShootArrow()
    {
        FindObjectOfType<Audio_Manager>().Play("ArrowShoot");
        CameraShake.instance.ShakeChamera(0.8f, 0.1f);
        _totalArrow--;
        arrowCount?.Invoke(_totalArrow);
        GameObject go = Instantiate(arrowPrefab, arrow.position, arrow.rotation);
        _coolDownTime = shootingResetTime;
        _bowAnimator.SetBool("Attack", false);
    }

    public void AddArrows(int addedArrows)
    {
        _totalArrow += addedArrows;
        arrowCount?.Invoke(_totalArrow);
    }
    public void RemoveArrows(int removedArrows)
    {
        _totalArrow -= removedArrows;
        arrowCount?.Invoke(_totalArrow);
    }

    public int Arrows()
    {
        return _totalArrow;
    }
}
