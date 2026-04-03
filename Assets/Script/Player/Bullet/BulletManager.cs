using System;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float lifeTime;
    
    private Rigidbody2D rb2D;
    private float timer = 0;
    
    private void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        rb2D.linearVelocity = this.transform.up * speed;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= lifeTime)
        {
            //this.gameObject.SetActive(false);
            KillBullet();
        }
    }

    public void KillBullet()
    {
        Destroy(gameObject);
    }

    public void StopBullet()
    {
        GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
    }
    
    public void FireBullet()
    {
        GetComponent<Rigidbody2D>().linearVelocity = this.transform.up * speed;
    }

    /*private void OnEnable()
    {
        timer = 0;
        rb2D = GetComponent<Rigidbody2D>();
        rb2D.linearVelocity = this.transform.up * speed;
    }*/
}
