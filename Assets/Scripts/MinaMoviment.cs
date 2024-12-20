using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinaMoviment : MonoBehaviour
{
    public int speed;
    public GameObject eXplosion;
    AudioManager audioManage;
    // Start is called before the first frame update
    void Start()
    {
        transform.DOMoveY(-0.8f, 2f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
        audioManage = AudioManager.instancia;
        speed = Random.Range(25,40);
        transform.rotation = new Quaternion(0, 90, 90, 0);    
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime);
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("EnemyDestroy"))
        {
            Destroy(this.gameObject);
            DOTween.Kill(transform);
        }
        if (other.gameObject.CompareTag("Player"))
        {
            Destroy(this.gameObject);
            audioManage.PlaySFX(audioManage.Explosion, false);
            Instantiate(eXplosion, transform.position, transform.rotation);
            DOTween.Kill(transform);
        }
        if (other.gameObject.CompareTag("PlayerFire"))
        {
            Destroy(this.gameObject);
            audioManage.PlaySFX(audioManage.Explosion, false);
            Instantiate(eXplosion, transform.position, transform.rotation);
            DOTween.Kill(transform);
        }
    }
}
