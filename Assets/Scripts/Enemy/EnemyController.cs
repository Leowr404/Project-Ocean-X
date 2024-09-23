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
    void Start()
    {
        meshRenderer = GetComponentInChildren<MeshRenderer>();
        materialOriginal = meshRenderer.material;
        impulseSource = GetComponent<CinemachineImpulseSource>();
        BulletDmg = BulletController.instancia;
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
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
            }
        }
        if(BulletDmg.PowerUp == false)
        {
            currentHealth -= BulletDmg.damage;
            if (currentHealth <= 0)
            {
            Destroy(gameObject);
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
    }
}
