using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgrounsScrolling : MonoBehaviour
{
    public float speedX;
    public float speedY;
    private MeshRenderer meshRenderer;
    // Start is called before the first frame update
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        meshRenderer.material.mainTextureOffset = new Vector2(Time.realtimeSinceStartup * speedX, Time.realtimeSinceStartup * speedY);
    }
}
