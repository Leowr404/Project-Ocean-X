using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectScript : MonoBehaviour
{
    [SerializeField] private GameObject hitPrefab;
    void Start()
    {  
        DestroyGameObject();
    }
    void DestroyGameObject()
    {
        Destroy(gameObject, 2);
    }

    public void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Enemy"))
        {
            Instantiate(hitPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
            
            
        }
        if (collider.gameObject.CompareTag("EnemyFire"))
        {
            
            Destroy(collider.gameObject);
            
            

        }
        if (collider.gameObject.CompareTag("Mina"))
        {

            Destroy(collider.gameObject);
            Destroy(gameObject);


        }
    }
}
