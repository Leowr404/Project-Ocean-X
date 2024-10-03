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
    public float cooldownTime = 2f; 
    private float nextAttackTime;
    void Start()
    {
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
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        {
            if (Time.time >= nextAttackTime)
            {
                Shoot();
                nextAttackTime = Time.time + cooldownTime;
            }
        }
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
                DOTween.Kill(this.gameObject);
                Destroy(gameObject);
                enemySpawn.AddPoints(10);
            }
        }
        if(BulletDmg.PowerUp == false)
        {
            currentHealth -= BulletDmg.damage;
            if (currentHealth <= 0)
            {
                DOTween.Kill(this.gameObject);
                Destroy(gameObject);
                enemySpawn.AddPoints(10);
            }

        }
    }

    private IEnumerator ResetMaterial()
    {
        // Vai executar depois que o tempo de duração do dano passar
        yield return new WaitForSeconds(tempoTexturaDano);
        // Depois desse tempo aí de cima passar o material do inimigo vai voltar pro base   
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
            Debug.Log("teste Enemydestroy");
            Destroy(this.gameObject);
        }
    }
    private void Shoot()
    {
        GameObject projectile = Instantiate(projectilePrefab, shootPoint.position, shootPoint.rotation);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        rb.AddForce(shootPoint.forward * _shootForce, ForceMode.Impulse);
        nextAttackTime = Time.time + cooldownTime;

    }

    
}
