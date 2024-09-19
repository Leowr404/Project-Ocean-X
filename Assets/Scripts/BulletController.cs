using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public bool PowerUp = true;
    public int damageMulti = 2;
    public int damage = 5;
    public static BulletController instancia;
    private void Awake()
    {
        instancia = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        PowerUp = false;
    }

    public IEnumerator PowerUps(Collider collider)
    {
        if (collider.gameObject.CompareTag("PowerUp"))
        {
            //Destroy(gameObject);
            Debug.Log("Power Up On");
            PowerUp = true;
            yield return new WaitForSeconds(5f);
            Debug.Log("Power Up Off");
            PowerUp = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
