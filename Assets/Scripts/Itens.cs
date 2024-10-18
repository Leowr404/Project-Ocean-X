using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Itens : MonoBehaviour
{
    public int speed;
    public int speedLow;
    public int radius;
    public bool Mover = true;
    public LayerMask playerLayer;
    private bool playerInRange = false;

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
        PlayerDetect();
        if (playerInRange)
        {
            Mover = false;
        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    private void PlayerDetect()
    {
        playerInRange = Physics.OverlapSphere(transform.position, radius, playerLayer).Length > 0;
    }
    public void Move()
    {
        if(Mover == true)
        {
         transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
        else if(Mover == false)
        {
            transform.Translate(Vector3.forward * speedLow * Time.deltaTime);

        }
    }
    
}
