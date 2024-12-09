using Cinemachine;
using DG.Tweening;
using DG.Tweening.Core.Easing;
using System.Collections;
using UnityEngine;

public class Boss2 : MonoBehaviour
{
    [Header("Configurações Gerais")]
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int currentHealth;
    AudioManager audioManager;

    [Header("Componentes Giratorios")]
    //[SerializeField] private Transform rotatingObject;
    public int rotationSpeed1;
    public int rotationSpeed2;
    public int rotationSpeed3;
    public int rotationSpeed4;
    public int rotationSpeedCruz;
    [SerializeField] private Transform _cruz;
    [SerializeField] private Transform _dente1;
    [SerializeField] private Transform _dente2;
    [SerializeField] private Transform _dente3;
    [SerializeField] private Transform _dente4;


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
    //[SerializeField] private float shootInterval = 1f;
    [SerializeField] private float shootIntervalphase1 = 0.8f;
    [SerializeField] private float shootIntervalphase2 = 1f;
    [SerializeField] private float shootIntervalphase3 = 0.3f;
    [SerializeField] private int shootForce = 10;
    [SerializeField] private int spreadProjectiles = 8;
    [SerializeField] private int spiralProjectiles = 12;

    [Header("Outros")]
    [SerializeField] private Material materialOriginal;
    [SerializeField] private Material materialDano;
    [SerializeField] private float tempoTexturaDano = 0.2f;
    [SerializeField] private GameObject destructionPrefab;

    private bool hasStartedSpreadShot = false;
    private bool hasStartedAdvancedPhase = false;
    private CinemachineImpulseSource impulseSource;
    private MeshRenderer meshRenderer;
    public MeshRenderer meshRendererr;
    private BulletController bulletDmg;
    PlayerController playerController;


    private void Start()
    {
        playerController = FindAnyObjectByType<PlayerController>();
        audioManager = AudioManager.instancia;
        //shootIntervalphase1 = Random.Range(0.2f, 0.8f);
        // shootIntervalphase2 = Random.Range(0.5f, 1f);
        // shootIntervalphase3 = Random.Range(0.2f, 0.8f);
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
        yield return new WaitForSeconds(2f);
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
            InvokeRepeating(nameof(HomingShot), 0f, 2f);
            rotationSpeedCruz = 500;
        }

        if (currentHealth <= maxHealth * 0.25f && !hasStartedAdvancedPhase)
        {
            hasStartedAdvancedPhase = true;
            CancelInvoke(nameof(SpreadShot));
            InvokeRepeating(nameof(SpiralShot), 0, shootIntervalphase3);
            InvokeRepeating(nameof(HomingShot), 0f, 0.2f);
            StartCoroutine(TeleportBehavior());
            rotationSpeedCruz = 1000;
        }
        if (currentHealth <= 0)
        {
            playerController.currentHealth = 10;
            hasStartedAdvancedPhase = true;
            CancelInvoke(nameof(SpreadShot));
            CancelInvoke(nameof(SpiralShot));
            CancelInvoke(nameof(HomingShot));

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
    public void Rotation()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerFire"))
        {
            CinemachineManager.instancia.CameraShake(impulseSource);
            audioManager.PlaySFX(audioManager.Hit, false);
            TakeDamage();
        }
    }

    private void TakeDamage()
    {
        meshRenderer.material = materialDano;
        meshRendererr.material = materialDano;
        StartCoroutine(ResetMaterial());
        currentHealth -= bulletDmg.PowerUp ? bulletDmg.damage * bulletDmg.damageMulti : bulletDmg.damage;
        UpdatePhase();
        if (currentHealth <= 0) DefeatBoss();
    }

    private IEnumerator ResetMaterial()
    {
        yield return new WaitForSeconds(tempoTexturaDano);
        meshRenderer.material = materialOriginal;
        meshRendererr.material = materialOriginal;
    }
    private IEnumerator ExplodeAndPlaySound()
    {
        for (int i = 0; i < 4; i++)
        {
            Instantiate(destructionPrefab, transform.position, Quaternion.identity);
            audioManager.PlaySFX(audioManager.BossDeath, false);
            yield return new WaitForSeconds(0.5f);
        }
    }


    public void DefeatBoss()
    {

        StartCoroutine(ExplodeAndPlaySound());
        transform.DOMoveY(transform.position.y - 10f, 3f)
            .SetEase(Ease.InCubic)
            .OnComplete(() =>
            {
                DOTween.Kill(transform);
                DOTween.Kill(this);
                Die();
            });
    }

    private void Die()
    {
        DOTween.Kill(this.gameObject);
        DOTween.Kill(transform);
        Destroy(gameObject);
        GameManager.Instance.WinLevel();
    }
    public void LateUpdate()
    {
        _dente1.Rotate(Vector3.up * rotationSpeed1 * Time.deltaTime);
        _dente2.Rotate(Vector3.up * rotationSpeed2 * Time.deltaTime);
        _dente3.Rotate(Vector3.up * rotationSpeed3 * Time.deltaTime);
        _dente4.Rotate(Vector3.up * rotationSpeed4 * Time.deltaTime);
        _cruz.Rotate(Vector3.forward * rotationSpeedCruz * Time.deltaTime);
    }
}
