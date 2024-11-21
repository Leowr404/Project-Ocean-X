using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class parallax : MonoBehaviour
{
    public float x,y;
    [SerializeField]private RawImage rawImage;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        rawImage.uvRect = new Rect(rawImage.uvRect.position + new Vector2(x,y) * Time.deltaTime, rawImage.uvRect.size);
    }

}
