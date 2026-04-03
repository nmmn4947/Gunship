using System;
using UnityEngine;

public class BossHurtVisual : MonoBehaviour
{
    [SerializeField] private SpriteRenderer hurtSprite;
    [SerializeField] private GameObject explosion;
    public Vector3 offsetKnock;
    private Color originalColor;
    private float anchorTime;
    
    public void ApplyHurtVisual()
    {
        hurtSprite.color = Color.red;
        anchorTime = Time.time;
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
        if (Time.time >= anchorTime + 0.1f)
        {
            hurtSprite.color = originalColor;
        }
    }
}
