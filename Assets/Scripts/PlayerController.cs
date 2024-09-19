using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("Config Player")]
    [SerializeField] public Slider sliderBar;
    [SerializeField] public int maxHealth;
    [SerializeField] public int currentHealth;
    [SerializeField] public int Velocidade;
    [SerializeField] public CanvasGroup UiBar;
    Vector2 moveInput;
    private CharacterController characterController;
    //==========Config Audio==========
    AudioManager audioManager;
    private AudioSource Tiro;

    //===========Config Tiro==========
    [Header("Config Tiro")]
    public GameObject projectilePrefab;
    public Transform shootPoint;
    private AudioClip Tiro_sound;
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
    [SerializeField] private float _tiltAmount = 15f;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        sliderBar.maxValue = maxHealth;
        sliderBar.value = currentHealth;
        characterController = GetComponent<CharacterController>();
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();

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
        currentHealth -= amount;
        sliderBar.value = currentHealth;
        if (currentHealth < 0)
        {
            Destroy(gameObject);
        }
    }

    private void FadeIn()
    {
        UiBar.alpha = 1.0f;
    }

    IEnumerator Fade()
    {
        if (UiBar.alpha >= 1)
        {
            UiBar.alpha += Time.deltaTime;
        }

        yield return new WaitForSeconds(5f);
    }
    public void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Hitou");
            TakeDamage(4);
        }
    }
}
