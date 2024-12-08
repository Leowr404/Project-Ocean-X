using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MisseisBoss : MonoBehaviour
{
    public Transform target; // Alvo a ser perseguido
    public GameObject playerObj;
    public float speed = 5f; // Velocidade do projétil
    public float rotateSpeed = 200f; // Velocidade de rotação para mirar no alvo

    private Rigidbody rb;

    private void Start()
    {
        playerObj = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody>();
        target = playerObj.transform;
    }

    private void FixedUpdate()
    {
        if (target != null)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            Vector3 rotateAmount = Vector3.Cross(transform.forward, direction);
            rb.angularVelocity = rotateAmount * rotateSpeed * Time.fixedDeltaTime;
            rb.velocity = transform.forward * speed;
        }
    }
}
