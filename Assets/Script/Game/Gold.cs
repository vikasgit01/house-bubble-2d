using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//This script is attached to a stackofgold prefab

public class Gold : MonoBehaviour
{
    [SerializeField] int goldAmount;
    [SerializeField] float radius;
    [SerializeField] private LayerMask layerMask;
    private Animator _goldAnimator;
    private int _goldPickupAnimtorId;
    private bool _canMove;
    private Vector3 playerpos;
    private void Start()
    {
        _goldAnimator = GetComponentInChildren<Animator>();
        _goldPickupAnimtorId = Animator.StringToHash("PickUp");
        _canMove = false;
    }

    private void Update()
    {
        if (_canMove)
        {
            transform.position = Vector2.MoveTowards(this.transform.position, Player.Instance.transform.position, 10f * Time.deltaTime);
        }

        Collider2D coll = Physics2D.OverlapCircle(transform.position, radius, layerMask);

        if (coll != null)
        {
            if (coll.gameObject.tag == "Player")

                _canMove = true;
                _goldAnimator.SetBool(_goldPickupAnimtorId, true);
        }



    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Game_Manager.instance.coinCollected++;
            Game_Manager.instance.IncrementGold(goldAmount);
            UI.instance.Message("+" + goldAmount);
            FindObjectOfType<Audio_Manager>().Play("GoldPickUp");
            Destroy(gameObject);
            StartCoroutine(WaitForSecondsFunction());
        }

        if(collision.gameObject.tag == "House")
        {
            Debug.Log("hi");
        }
    }

    IEnumerator WaitForSecondsFunction()
    {
        yield return new WaitForSeconds(0.5f);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
