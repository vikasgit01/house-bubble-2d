using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script is attached to the Bullet Prefab

public class Bullet : MonoBehaviour
{
    [SerializeField] private GameObject bulletEffectPrefab;
    [SerializeField] private float playerDamage = 10f;
    [SerializeField] private float houseDamage = 5f;
    private void Start()
    {
        
        Destroy(this.gameObject, 1);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (Player.Instance.playerHealth > 0)
            {
                FindObjectOfType<Audio_Manager>().Play("PlayerDamage");
                Player.Instance.TakeDamage(playerDamage);
            }
            Destroy(gameObject);
        }
        if (collision.gameObject.tag == "House")
        {
            Instantiate(bulletEffectPrefab, transform.position, transform.rotation);
            House_01 hs = collision.gameObject.GetComponent<House_01>();
            if (hs != null)
            {
                float houseHealth = hs.HouseHealth();

                if (houseHealth > 0)
                {
                    FindObjectOfType<Audio_Manager>().Play("AlienBulletHit");
                    hs.TakeDamage(houseDamage);
                }
            }
            Destroy(gameObject);
        }
    }
}
