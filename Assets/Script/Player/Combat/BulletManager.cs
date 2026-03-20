using System;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    [SerializeField] private float speed;
    Rigidbody2D rb2D;
    
    private void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        rb2D.linearVelocity = this.transform.up * speed;
    }
}
