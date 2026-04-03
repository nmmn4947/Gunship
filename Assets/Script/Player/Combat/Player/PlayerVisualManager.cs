using System;
using UnityEngine;

public class PlayerVisualManager : MonoBehaviour
{
    [SerializeField] SpriteRenderer playerSkin;
    private float anchorTime;
    
    public void HurtVisual()
    {
        playerSkin.color = Color.red;
        anchorTime = Time.time;
    }

    private void Update()
    {
        if (Time.time >= anchorTime + 0.1f)
        {
            playerSkin.color = Color.white;
        }
    }
}
