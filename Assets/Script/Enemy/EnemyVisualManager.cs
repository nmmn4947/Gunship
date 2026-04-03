using UnityEngine;

public class EnemyVisualManager : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private GameObject _enemyExplosion;
    private int framePivot;

    public void HurtVisual()
    {
        _spriteRenderer.color = Color.red;
        framePivot = Time.frameCount;
    }

    public void DeadExplosionSpawn()
    {
        Instantiate(_enemyExplosion, transform.position, Quaternion.identity);
    }
    
    void Update()
    {
        if (Time.frameCount >= framePivot + 3)
        {
            _spriteRenderer.color = Color.white;
        }
    }
}
