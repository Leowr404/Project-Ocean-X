using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFire : MonoBehaviour
{
    [SerializeField] private GameObject hitPrefab;

    public void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("EnemyDestroy"))
        {
            Instantiate(hitPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);

        }
        if (collider.gameObject.CompareTag("Player"))
        {
            Instantiate(hitPrefab, transform.position, Quaternion.identity);

        }
    }
}
