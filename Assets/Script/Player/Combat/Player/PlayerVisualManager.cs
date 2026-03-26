using System;
using UnityEngine;

public class PlayerVisualManager : MonoBehaviour
{
    [SerializeField] SpriteRenderer playerSkin;
    private int anchorFrame;
    
    public void HurtVisual()
    {
        playerSkin.color = Color.red;
        anchorFrame = Time.frameCount;
    }

    private void Update()
    {
        if (Time.frameCount >= anchorFrame + 2)
        {
            playerSkin.color = Color.white;
        }
    }
}
