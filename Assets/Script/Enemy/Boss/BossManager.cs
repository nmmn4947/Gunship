using System.Collections;
using UnityEngine;
using Napadol.Tools;

public class BossManager : MonoBehaviour
{
    [SerializeField] private GameObject mainPart;
    [SerializeField] private GameObject backPart;
    [SerializeField] private GameObject leftPart;
    [SerializeField] private GameObject rightPart;
    [SerializeField] private GameObject player;
    [SerializeField] private float deadBulletTime;
    
    BossMovement bossMovement = new BossMovement();

    private float timer = 0f;
    
    public void DisconnectParts(GameObject part)
    {
        StartCoroutine(RunDisconnectParts(part));
    }

    private IEnumerator RunDisconnectParts(GameObject part)
    {
        if (part.transform.parent == null)
        {
            yield break;
        }
        Vector3 originalPosition = part.GetComponent<BossHurtVisual>().offsetKnock;
        part.transform.parent = null;
        Rigidbody2D rb;
        if(part.GetComponent<Rigidbody2D>() == null){rb = part.AddComponent<Rigidbody2D>();}
        else
        {
            rb = part.GetComponent<Rigidbody2D>();
        }

        rb.gravityScale = 0;
        rb.linearDamping = 2f;
        yield return new WaitForFixedUpdate();
        rb.AddForce(originalPosition * 5f, ForceMode2D.Impulse);
    }

    public void BossDiedTimeSlowDown()
    {
        StartCoroutine(SlowTimeOnDeath());
    }

    private IEnumerator SlowTimeOnDeath()
    {
        while (timer <= deadBulletTime)
        {
            timer += Time.unscaledDeltaTime;
            Time.timeScale = Mathf.Lerp(0.05f, 1f, Easing.EaseInExpo(timer / deadBulletTime));
            yield return null;
        }
        
        Time.timeScale = 1f;
    }
}
