using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItensForward : MonoBehaviour
{
    public int speed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("EnemyDestroy"))
        {
            Destroy(this.gameObject);
        }
    }
}
