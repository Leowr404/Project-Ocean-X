using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarCanvas : MonoBehaviour
{
    private Transform cameraTransform;

    void Start()
    {
        // Pega a c�mera principal, que est� sendo controlada pelo Cinemachine
        cameraTransform = Camera.main.transform;
    }

    void LateUpdate()
    {
        // Faz com que a barra de vida olhe sempre para a c�mera do Cinemachine
        transform.LookAt(transform.position + cameraTransform.rotation * Vector3.forward,
                         cameraTransform.rotation * Vector3.up);
    }
}
