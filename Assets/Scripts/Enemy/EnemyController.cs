using Cinemachine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class EnemyController : MonoBehaviour
{
    [SerializeField] public int maxHealth = 10;
    [SerializeField] public int currentHealth;
    [Header("Config Radius Gizmos")]
    [SerializeField]private float detectionRadius;
    private bool playerInRange = false;
    public LayerMask playerLayer;
    //
    BulletController BulletDmg;
    private CinemachineImpulseSource impulseSource;
    MeshRenderer meshRenderer;
    [SerializeField] private Material materialOriginal;
    [SerializeField] private Material materialDano;
    [SerializeField] private float tempoTexturaDano;
    public int speed;
    EnemySpawn enemySpawn;
    public float enemyrange;
    public GameObject projectilePrefab;
    public Transform shootPoint;
    [SerializeField] private float _shootForce = 120f;
    private float cooldownTime; 
    private float nextAttackTime;
    [SerializeField] private GameObject[] ItemDrop;
    public int chancedrop;
    void Start()
    {
        cooldownTime = Random.Range(0.5f, 2f);
        enemyrange = Random.Range(1,6);
        transform.DOMoveX(7, enemyrange).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
        speed = Random.Range(-10, -30);
        enemySpawn = EnemySpawn.instance;
        meshRenderer = GetComponentInChildren<MeshRenderer>();
        materialOriginal = meshRenderer.material;
        impulseSource = GetComponent<CinemachineImpulseSource>();
        BulletDmg = BulletController.instancia;
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
        DetectPlayer();
        if (playerInRange && Time.time >= nextAttackTime)
        {
            Shoot();
        }
    }
    void Move()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
    public void TakeDamage()
    {
        meshRenderer.material = materialDano;
        StartCoroutine(ResetMaterial());
        if (BulletDmg.PowerUp == true)
        {
            currentHealth -= BulletDmg.damage *+ BulletDmg.damageMulti;
            if (currentHealth <= 0)
            {
                Destroy(gameObject);
                enemySpawn.AddPoints(10);
                if(Random.Range(0,100) <= chancedrop)
                {
                    int randomIndex = Random.Range(0, ItemDrop.Length);
                    Instantiate(ItemDrop[randomIndex], transform.position, transform.rotation);
                    
                }
                DOTween.Kill(this.gameObject);
                DOTween.Kill(transform);
            }
        }
        if(BulletDmg.PowerUp == false)
        {
            currentHealth -= BulletDmg.damage;
            if (currentHealth <= 0)
            {
                Destroy(gameObject);
                enemySpawn.AddPoints(10);
                if (Random.Range(0, 100) <= chancedrop)
                {
                    int randomIndex = Random.Range(0, ItemDrop.Length);
                    Instantiate(ItemDrop[randomIndex], transform.position, transform.rotation);
                    
                }
                DOTween.Kill(this.gameObject);
                DOTween.Kill(transform);

            }

        }
    }

    private IEnumerator ResetMaterial()
    {
        
        yield return new WaitForSeconds(tempoTexturaDano);  
        meshRenderer.material = materialOriginal;
    }

    public void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("PlayerFire"))
        {
            CinemachineManager.instancia.CameraShake(impulseSource);
            TakeDamage();
        }
        if (collider.gameObject.CompareTag("EnemyDestroy"))
        {
            Destroy(this.gameObject);
            DOTween.Kill(transform);
        }
    }
    private void Shoot()
    {
        GameObject projectile = Instantiate(projectilePrefab, shootPoint.position, shootPoint.rotation);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        rb.AddForce(shootPoint.forward * _shootForce, ForceMode.Impulse);
        nextAttackTime = Time.time + cooldownTime;

    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

    }
    void DetectPlayer()
    {
        playerInRange = Physics.OverlapSphere(transform.position, detectionRadius, playerLayer).Length > 0;
    }


}
