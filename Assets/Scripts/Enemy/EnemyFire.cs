using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFire : MonoBehaviour
{
    void Start()
    {
        
    }

    public void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("EnemyDestroy"))
        {
            
            Destroy(gameObject);

        }
    }
}
