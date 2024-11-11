using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;


public class PlayerController : MonoBehaviour
{
    [Header("Config Player")]
    [SerializeField] public CanvasGroup DangerUi;
    [SerializeField] public Slider sliderBar;
    [SerializeField] public int maxHealth;
    [SerializeField] public int currentHealth;
    [SerializeField] public int Velocidade;
    [SerializeField] public CanvasGroup UiBar;
    private bool isLoopActive = false;
    Vector2 moveInput;
    [SerializeField] private GameObject player;
    private CharacterController characterController;
    MeshRenderer meshRenderer;
    [SerializeField] private Material materialOriginal;
    [SerializeField] private Material materialDano;
    [SerializeField] private float tempoTexturaDano;
    [SerializeField] private TextMeshProUGUI ColletableTxt;
    //==========Config Manager/Controller==========
    AudioManager audioManager;
    GameManager gameManager;
    //===========Config Tiro==========
    [Header("Config Tiro")]
    public GameObject projectilePrefab;
    public Transform shootPoint;
    [SerializeField] private float _shootForce = 20f;
    [SerializeField] private float _fireRate;
    [SerializeField] private float _nextFireTime = 0f;
    [SerializeField] private bool _isShooting = false;
    //================================
    [Header("Config TiroShootgun")]
    public GameObject projectilePrefabShotgun;
    [SerializeField] private bool _isShootingG = false;
    [SerializeField]private float cooldownTime = 0.5f; 
    [SerializeField]private float lastShootTime = 0f;
    [SerializeField] private float spreadAngle;
    [SerializeField] private float numberOfProjectiles;
    //===============================
    public GameObject hitPrefab;

    //======Limite De Cenario=========
    [Header("Config limite cenario")]
    public float xMin = -10f;
    public float xMax = 10f;
    public float zMin = -5f;
    public float zMax = 5f;
    //================================
    //Material material;
    [SerializeField] private float _tiltAmount = 15f;
    BulletController BulletDmg;
    
    void Start()
    {
        
        ColletableTxt = GetComponentInChildren<TextMeshProUGUI>();
        DangerUi.GetComponent<CanvasGroup>();
        DangerUi.DOFade(0, 0.1f);
        _fireRate = 0.3f;
        meshRenderer = GetComponentInChildren<MeshRenderer>();
        materialOriginal = meshRenderer.material;
        UiBar.GetComponent<CanvasGroup>();
        UiBar.DOFade(0,2f);
        currentHealth = maxHealth;
        sliderBar.maxValue = maxHealth;
        sliderBar.value = currentHealth;
        characterController = GetComponent<CharacterController>();
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        gameManager = GameManager.Instance;
        BulletDmg = BulletController.instancia;

    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }
    public void OnMenu(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            gameManager.PauseGame();
        }
    }
    public void OnShoot(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _isShooting = true;
        }
        else if (context.canceled)
        {
            _isShooting = false;
        }
    }
    public void OnShootGun(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _isShootingG = true;
            ShootGun();
        }
        else if (context.canceled)
        {
            _isShootingG = false;
        }
    }


    // Update is called once per frame
    void Update()
    {
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y);
        characterController.Move(move * Velocidade * Time.deltaTime);
        Vector3 clampedPosition = transform.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, xMin, xMax);
        clampedPosition.z = Mathf.Clamp(clampedPosition.z, zMin, zMax);
        transform.position = clampedPosition;
        float tilt = moveInput.x * -_tiltAmount;
        transform.rotation = Quaternion.Euler(0, 0, tilt);
        

    }
    private void FixedUpdate()
    {
        if (_isShooting && Time.time >= _nextFireTime)
        {
            Shoot();
            _nextFireTime = Time.time + _fireRate;
        }
        

    }
    private void Shoot()
    {
        audioManager.PlaySFX(audioManager.Tiro_sound);
        GameObject projectile = Instantiate(projectilePrefab, shootPoint.position, shootPoint.rotation);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        rb.AddForce(shootPoint.forward * _shootForce, ForceMode.Impulse);

    }
    private void ShootGun()
    {
        if (Time.time - lastShootTime < cooldownTime)
            return; 
        lastShootTime = Time.time;
        audioManager.PlaySFX(audioManager.Tiro_sound);
        float initialAngle = -spreadAngle * (numberOfProjectiles - 1) / 2; // Ângulo inicial para centralizar o spread
        for (int i = 0; i < numberOfProjectiles; i++)
        {
            // Calcula o ângulo para cada projétil
            Quaternion rotation = Quaternion.Euler(0, initialAngle + spreadAngle * i, 0) * shootPoint.rotation;
            GameObject projectile = Instantiate(projectilePrefabShotgun, shootPoint.position, rotation);
            Rigidbody rb = projectile.GetComponent<Rigidbody>();
            rb.AddForce(rotation * Vector3.forward * _shootForce, ForceMode.Impulse);
        }
    }

    public void TakeDamage(int amount)
    {
        meshRenderer.material = materialDano;
        StartCoroutine(ResetMaterial());
        currentHealth -= amount;
        sliderBar.value = currentHealth;
        UpdateHealthStatus();
        if(currentHealth >= 3)
        {
            ShowAndFadeLifeBar();
        }
        if (currentHealth <= 0)
        {
            player.SetActive(false);
            gameManager.GameOver();
            audioManager.StopLoopingSFX();

        }
    }
    public void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Enemy"))
        {
            TakeDamage(1);
        }
        if (collider.gameObject.CompareTag("EnemyFire"))
        {
            TakeDamage(1);
            Destroy(collider.gameObject);
            Instantiate(hitPrefab, transform.position, transform.rotation);
        }
        if (collider.gameObject.CompareTag("PowerUp"))
        {
            Destroy(collider.gameObject);
            StartCoroutine(PowerUps());  
        }
        if (collider.gameObject.CompareTag("Cure"))
        {
            Destroy(collider.gameObject);
            ShowAndFadeLifeBar();
            StartCoroutine(Cure());
            currentHealth = maxHealth;
            sliderBar.value = currentHealth;
            UpdateHealthStatus();
        }
    }
    public IEnumerator PowerUps()
    {
            ColletableTxt.text = "POWER+";
            ColletableTxt.alpha = 1.0f;
            DesaparecerTextoAposTempo(1f, 0.5f);
            BulletDmg.PowerUp = true;
            audioManager.PlaySFX(audioManager.Coletavel, false);
            _fireRate = 0.2f;
            yield return new WaitForSeconds(5f);
            BulletDmg.PowerUp = false;
            _fireRate = 0.3f;
    }
    public IEnumerator Cure()
    {
        ColletableTxt.text = "HEALTH+";
        ColletableTxt.alpha = 1.0f;
        DesaparecerTextoAposTempo(1f, 0.5f);
        yield break;
    }
    private IEnumerator ResetMaterial()
    {
        yield return new WaitForSeconds(tempoTexturaDano); 
        meshRenderer.material = materialOriginal;
    }

    private void UpdateHealthStatus()
    {
        if (currentHealth <= 2 && !isLoopActive)
        {
            DangerUi.DOFade(1, 0.2f).SetLoops(-1, LoopType.Yoyo);
            UiBar.DOFade(1, 0.3f).SetLoops(-1, LoopType.Yoyo);
            isLoopActive = true;
            audioManager.PlaySFX(audioManager.Danger, true);
        }
        else if (currentHealth > 2 && isLoopActive)
        {
            DOTween.Kill(DangerUi);
            DOTween.Kill(UiBar);
            DangerUi.DOFade(0, 0.1f); 
            UiBar.DOFade(0, 1f);
            isLoopActive = false;
            audioManager.StopLoopingSFX();
        }
    }
        public void ShowAndFadeLifeBar()
        {
        // Faz a barra de vida aparecer (Fade para 1)
        UiBar.DOFade(1, 2f)
            .OnComplete(() =>
            {
                // Após 1 segundo, faz a barra de vida desaparecer (Fade para 0)
                UiBar.DOFade(0, 2f);
            });
        }
    void DesaparecerTextoAposTempo(float delay, float duracaoFade)
    {
        ColletableTxt.DOFade(0, duracaoFade).SetDelay(delay);
    }

}
