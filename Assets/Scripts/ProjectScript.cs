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
        Destroy(gameObject, 5);
    }

    public void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Enemy"))
        {    
            Debug.Log("Inimigo Atingido");
            Destroy(gameObject);
            
        }
    }
}
