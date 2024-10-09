using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectScript : MonoBehaviour
{
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
            
            Destroy(gameObject);
            
        }
        if (collider.gameObject.CompareTag("EnemyFire"))
        {
            
            Destroy(collider.gameObject);
            Destroy(gameObject);

        }
    }
}
