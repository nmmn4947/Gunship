using System;
using UnityEngine;

public class BossHurtVisual : MonoBehaviour
{
    [SerializeField] private SpriteRenderer hurtSprite;
    [SerializeField] private GameObject explosion;
    public Vector3 offsetKnock;
    private Color originalColor;
    private int anchorFrame;
    
    public void ApplyHurtVisual()
    {
        hurtSprite.color = Color.red;
        anchorFrame = Time.frameCount;
    }

    public void SpawnExplosion()
    {
        Instantiate(explosion, transform.position, Quaternion.identity);
    }

    private void Start()
    {
        originalColor = hurtSprite.color;
    }

    private void Update()
    {
        if (Time.frameCount >= anchorFrame + 5)
        {
            hurtSprite.color = originalColor;
        }
    }
}
