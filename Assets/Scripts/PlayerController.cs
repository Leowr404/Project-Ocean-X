using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using DG.Tweening;


public class PlayerController : MonoBehaviour
{
    [Header("Config Player")]
    [SerializeField] public Slider sliderBar;
    [SerializeField] public int maxHealth;
    [SerializeField] public int currentHealth;
    [SerializeField] public int Velocidade;
    [SerializeField] public CanvasGroup UiBar;
    Vector2 moveInput;
    [SerializeField] private GameObject player;
    private CharacterController characterController;
    MeshRenderer meshRenderer;
    [SerializeField] private Material materialOriginal;
    [SerializeField] private Material materialDano;
    [SerializeField] private float tempoTexturaDano;
    //==========Config Audio==========
    AudioManager audioManager;
    //===========Config Tiro==========
    [Header("Config Tiro")]
    public GameObject projectilePrefab;
    public Transform shootPoint;
    [SerializeField] private float _shootForce = 20f;
    [SerializeField] private float _fireRate = 0.5f;
    [SerializeField] private float _nextFireTime = 0f;
    [SerializeField] private bool _isShooting = false;
    //================================

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
    // Start is called before the first frame update
    void Start()
    {
        meshRenderer = GetComponentInChildren<MeshRenderer>();
        materialOriginal = meshRenderer.material;
        UiBar.GetComponent<CanvasGroup>();
        UiBar.DOFade(0,2f);
        currentHealth = maxHealth;
        sliderBar.maxValue = maxHealth;
        sliderBar.value = currentHealth;
        characterController = GetComponent<CharacterController>();
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        BulletDmg = BulletController.instancia;

    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
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
        // Confi tiro abaixo
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

    public void TakeDamage(int amount)
    {
        meshRenderer.material = materialDano;
        StartCoroutine(ResetMaterial());
        StartCoroutine(Fadeout());
        currentHealth -= amount;
        sliderBar.value = currentHealth;
       // material.DOColor(Color.white, 0.5f);
        if (currentHealth < 0)
        {
            player.SetActive(false);
        }
    }
    public void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Tomou Dano");
            TakeDamage(1);
        }
        if (collider.gameObject.CompareTag("PowerUp"))
        {
            Destroy(collider.gameObject);
            StartCoroutine(PowerUps());
            
        }
    }
    public IEnumerator PowerUps()
    {
            
            Debug.Log("Power Up On");
            BulletDmg.PowerUp = true;
            yield return new WaitForSeconds(5f);
            Debug.Log("Power Up Off");
            BulletDmg.PowerUp = false; 
    }
    private IEnumerator ResetMaterial()
    {
        // Vai executar depois que o tempo de dura��o do dano passar
        yield return new WaitForSeconds(tempoTexturaDano);
        // Depois desse tempo a� de cima passar o material do inimigo vai voltar pro base   
        meshRenderer.material = materialOriginal;
    }

    public IEnumerator Fadeout()
    {

        UiBar.DOFade(1, 1f);
        yield return new WaitForSeconds(1.0f);
        UiBar.DOFade(0, 2f);
    }

}
