using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineManager : MonoBehaviour
{
    public static CinemachineManager instancia;
    [SerializeField] private float globalshakeForce = 1f;
    private void Awake()
    {
        if (instancia == null)
        {
            instancia = this;
        }
    }

    public void CameraShake(CinemachineImpulseSource impulseSource)
    {
        impulseSource.GenerateImpulseWithForce(globalshakeForce);
    }
}
