using System;
using UnityEngine;

public class BackGroundResize : MonoBehaviour
{
    [SerializeField] private ParticleSystem rain;
    [SerializeField] private GameObject oceanBG;
    private Camera cam;
    private float original;
    private Vector3 originalScale;
    
    private void Start()
    {
        cam = Camera.main;
        original = cam.orthographicSize;
        originalScale = oceanBG.transform.localScale;
    }

    private void Update()
    {
        if (cam.orthographicSize > original)
        {
            oceanBG.transform.localScale *= (cam.orthographicSize - original);
        }
        else
        {
            oceanBG.transform.localScale = originalScale;
        }
    }
}
