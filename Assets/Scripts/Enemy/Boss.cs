using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Boss : MonoBehaviour
{
    [Header("Vida Boss")]
    [SerializeField] public int maxHealth = 10;
    [SerializeField] public int currentHealth;
    /// <summary>
    ///
    /// </summary>
    public float targetX = 0f;                
    public float targetZ = -20f;              
    public float sideMovementDistance = 5f;   
    public float moveSpeed = 2f;              
    public float sideSpeed = 1f;          
    /// <summary>
    /// 
    /// </summary>
    public GameObject bulletPrefab;           
    public Transform shootPoint;
    [SerializeField]private int _shootForce;
    public float shootInterval = 1f;          
    private CinemachineImpulseSource impulseSource;
    GameManager gameManager;
    BulletController BulletDmg;
    MeshRenderer meshRenderer;
    [SerializeField] private Material materialOriginal;
    [SerializeField] private Material materialDano;
    [SerializeField] private float tempoTexturaDano;

    void Start()
    {
        impulseSource = GetComponent<CinemachineImpulseSource>();
        gameManager = GameManager.Instance;
        BulletDmg = BulletController.instancia;
        shootInterval = Random.Range(0.5f,1f);
        meshRenderer = GetComponentInChildren<MeshRenderer>();
        currentHealth = maxHealth;
        // Garante que o boss inicie olhando na direção do eixo -Z
        transform.rotation = Quaternion.LookRotation(Vector3.back);

        // Define a movimentação inicial até a posição (0, Y, targetZ)
        transform.DOMove(new Vector3(targetX, transform.position.y, targetZ), moveSpeed)
                 .SetEase(Ease.Linear)
                 .OnComplete(() =>
                 {
                     // Quando chega na posição alvo, começa o movimento lateral e o disparo contínuo
                     StartShooting();
                     MoveSideToSide();
                 });
    }

    void MoveSideToSide()
    {
        // Cria a sequência para o movimento lateral
        Sequence sideMovement = DOTween.Sequence();
        sideMovement.Append(transform.DOMoveX(sideMovementDistance, sideSpeed).SetEase(Ease.Linear));
        sideMovement.Append(transform.DOMoveX(-sideMovementDistance, sideSpeed).SetEase(Ease.Linear));
        sideMovement.SetLoops(-1, LoopType.Yoyo);  // Loop infinito indo de um lado para o outro
    }

    void StartShooting()
    {
        // Inicia o disparo repetido dos projéteis para frente
        InvokeRepeating(nameof(Shoot), 0, shootInterval);
    }

    void Shoot()
    {
        GameObject projectile = Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        rb.AddForce(shootPoint.forward * _shootForce, ForceMode.Impulse);
    }
    public void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("PlayerFire"))
        {
            CinemachineManager.instancia.CameraShake(impulseSource);
            TakeDamage();
        }
    }
    private IEnumerator ResetMaterial()
    {
        yield return new WaitForSeconds(tempoTexturaDano);
        meshRenderer.material = materialOriginal;
    }
    public void TakeDamage()
    {
        meshRenderer.material = materialDano;
        StartCoroutine(ResetMaterial());
        if (BulletDmg.PowerUp == true)
        {
            currentHealth -= BulletDmg.damage * +BulletDmg.damageMulti;
            if (currentHealth <= 0)
            {
                Destroy(gameObject);
                DOTween.Kill(this.gameObject);
                DOTween.Kill(transform);
                gameManager.WinLevel();
            }
        }
        if (BulletDmg.PowerUp == false)
        {
            currentHealth -= BulletDmg.damage;
            if (currentHealth <= 0)
            {
                Destroy(gameObject);
                DOTween.Kill(this.gameObject);
                DOTween.Kill(transform);
                gameManager.WinLevel();

            }

        }
    }
}
