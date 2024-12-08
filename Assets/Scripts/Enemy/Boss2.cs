using Cinemachine;
using DG.Tweening;
using System.Collections;
using UnityEngine;

public class Boss2 : MonoBehaviour
{
    [Header("Configurações Gerais")]
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int currentHealth;

    [Header("Movimentação")]
    [SerializeField] private float targetX = 0f;
    [SerializeField] private float targetZ = -20f;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float sideMovementDistance = 7f;
    [SerializeField] private float sideSpeed = 1f;
    [SerializeField] private float teleportInterval = 10f;

    [Header("Ataques")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject homingBulletPrefab;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private Transform shootPointMissel;
    [SerializeField] private float shootInterval = 1f;
    [SerializeField] private float shootIntervalphase1;
    [SerializeField] private float shootIntervalphase2;
    [SerializeField] private float shootIntervalphase3;
    [SerializeField] private int shootForce = 10;
    [SerializeField] private int spreadProjectiles = 8;
    [SerializeField] private int spiralProjectiles = 12;

    [Header("Outros")]
    [SerializeField] private Material materialOriginal;
    [SerializeField] private Material materialDano;
    [SerializeField] private float tempoTexturaDano = 0.2f;

    private bool hasStartedSpreadShot = false;
    private bool hasStartedAdvancedPhase = false;
    private CinemachineImpulseSource impulseSource;
    private MeshRenderer meshRenderer;
    private BulletController bulletDmg;

    private void Start()
    {
        shootIntervalphase1 = Random.Range(0.2f, 0.8f);
        shootIntervalphase2 = Random.Range(0.5f, 1f);
        shootIntervalphase3 = Random.Range(0.2f, 0.8f);
        bulletDmg = BulletController.instancia;
        impulseSource = GetComponent<CinemachineImpulseSource>();
        meshRenderer = GetComponentInChildren<MeshRenderer>();
        currentHealth = maxHealth;

        MoveToInitialPosition();
    }

    private void MoveToInitialPosition()
    {
        transform.DOMove(new Vector3(targetX, transform.position.y, targetZ), moveSpeed)
            .SetEase(Ease.Linear)
            .OnComplete(() => StartCoroutine(StartBossBehavior()));
    }

    private IEnumerator StartBossBehavior()
    {
        yield return new WaitForSeconds(3f);
        StartShooting();
        StartSideMovement();
    }

    private void StartShooting()
    {
        InvokeRepeating(nameof(SingleShot), 0, shootIntervalphase1);
    }

    private void StartSideMovement()
    {
        Sequence movementSequence = DOTween.Sequence();
        movementSequence.Append(transform.DOMoveX(targetX + sideMovementDistance, sideSpeed).SetEase(Ease.Linear))
                        .Append(transform.DOMoveX(targetX - sideMovementDistance, sideSpeed).SetEase(Ease.Linear))
                        .SetLoops(-1, LoopType.Yoyo);
    }

    private void UpdatePhase()
    {
        if (currentHealth <= maxHealth * 0.5f && !hasStartedSpreadShot)
        {
            hasStartedSpreadShot = true;
            CancelInvoke(nameof(SingleShot));
            InvokeRepeating(nameof(SpreadShot), 0, shootIntervalphase2);
            InvokeRepeating(nameof(HomingShot), 5f, 3f);
        }

        if (currentHealth <= maxHealth * 0.25f && !hasStartedAdvancedPhase)
        {
            hasStartedAdvancedPhase = true;
            CancelInvoke(nameof(SpreadShot));
            InvokeRepeating(nameof(SpiralShot), 0, shootIntervalphase3);
            InvokeRepeating(nameof(HomingShot), 0f, 1f);
            StartCoroutine(TeleportBehavior());
        }
    }

    private void SingleShot()
    {
        InstantiateAndShoot(shootPoint.forward);
    }

    private void SpreadShot()
    {
        float angleStep = 30f / (spreadProjectiles - 1);
        float startAngle = -15f;
        for (int i = 0; i < spreadProjectiles; i++)
        {
            float angle = startAngle + (i * angleStep);
            Vector3 direction = Quaternion.Euler(0, angle, 0) * shootPoint.forward;
            InstantiateAndShoot(direction);
        }
    }

    private void SpiralShot()
    {
        float angleStep = 360f / spiralProjectiles;
        for (int i = 0; i < spiralProjectiles; i++)
        {
            float angle = i * angleStep + Time.time * 100f;
            Vector3 direction = Quaternion.Euler(0, angle, 0) * shootPoint.forward;
            InstantiateAndShoot(direction);
        }
    }

    private void InstantiateAndShoot(Vector3 direction)
    {
        GameObject projectile = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        rb.AddForce(direction * shootForce, ForceMode.Impulse);
    }
    private void HomingShot()
    {
        Instantiate(homingBulletPrefab, shootPoint.position, Quaternion.Euler(0, 180, 0));
    }

    private IEnumerator TeleportBehavior()
    {
        while (currentHealth > 0)
        {
            yield return new WaitForSeconds(teleportInterval);
            Vector3 randomPosition = new Vector3(
                Random.Range(-sideMovementDistance, sideMovementDistance),
                transform.position.y,
                Random.Range(targetZ - 5f, targetZ + 5f)
            );
            transform.position = randomPosition;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerFire"))
        {
            CinemachineManager.instancia.CameraShake(impulseSource);
            TakeDamage();
        }
    }

    private void TakeDamage()
    {
        meshRenderer.material = materialDano;
        StartCoroutine(ResetMaterial());
        currentHealth -= bulletDmg.PowerUp ? bulletDmg.damage * bulletDmg.damageMulti : bulletDmg.damage;
        UpdatePhase();
        if (currentHealth <= 0) Die();
    }

    private IEnumerator ResetMaterial()
    {
        yield return new WaitForSeconds(tempoTexturaDano);
        meshRenderer.material = materialOriginal;
    }

    private void Die()
    {
        DOTween.Kill(this.gameObject);
        DOTween.Kill(transform);
        Destroy(gameObject);
        GameManager.Instance.WinLevel();
    }
}
