using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] public int maxHealth = 10;
    [SerializeField] public int currentHealth;
    BulletController BulletDmg;
    void Start()
    {
        BulletDmg = BulletController.instancia;
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void TakeDamage()
    {
        if (BulletDmg.PowerUp == true)
        {
            currentHealth -= BulletDmg.damage *= BulletDmg.damageMulti;
            if (currentHealth <= 0)
            {
                Destroy(gameObject);
            }
        }
        if(BulletDmg.PowerUp == false)
        {
            currentHealth -= BulletDmg.damage;
            if (currentHealth <= 0)
            {
            Destroy(gameObject);
            }

        }
    }

    public void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("PlayerFire"))
        {
            TakeDamage();
        }
    }
}
