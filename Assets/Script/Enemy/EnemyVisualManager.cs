using UnityEngine;

public class EnemyVisualManager : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private GameObject _enemyExplosion;
    private float timePivot;

    public void HurtVisual()
    {
        _spriteRenderer.color = Color.red;
        timePivot = Time.time;
    }

    public void DeadExplosionSpawn()
    {
        Instantiate(_enemyExplosion, transform.position, Quaternion.identity);
    }
    
    void Update()
    {
        if (Time.time >= timePivot + 0.1f)
        {
            _spriteRenderer.color = Color.white;
        }
    }
}
