using UnityEngine;

public class EnemyVisualManager : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    private int framePivot;

    public void HurtVisual()
    {
        _spriteRenderer.color = Color.red;
        framePivot = Time.frameCount;
    }
    
    void Update()
    {
        if (Time.frameCount >= framePivot + 3)
        {
            _spriteRenderer.color = Color.white;
        }
    }
}
