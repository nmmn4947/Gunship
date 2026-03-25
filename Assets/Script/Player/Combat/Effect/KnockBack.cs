using System;
using Codice.Client.BaseCommands.BranchExplorer;
using UnityEngine;

public class KnockBack : MonoBehaviour
{
    public float force;
    public bool justX;
    public bool justY;
    
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerManager pm = other.GetComponent<PlayerManager>();
        if (pm != null)
        {
            Vector2 thisTransform = transform.position;
            if (justX)
            {
                thisTransform = new Vector2(thisTransform.x, other.transform.position.y);
            }
            if (justY)
            {
                thisTransform = new Vector2(other.transform.position.x, thisTransform.y);
            }
            Vector2 dir = (Vector2)other.transform.position - thisTransform;
            pm.playerMovement.ApplyKnockback(dir * force);
        }
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        PlayerManager pm = other.gameObject.GetComponent<PlayerManager>();
        if (pm != null)
        {
            Vector2 thisTransform = transform.position;
            if (justX)
            {
                thisTransform = new Vector2(thisTransform.x, other.transform.position.y);
            }
            if (justY)
            {
                thisTransform = new Vector2(other.transform.position.x, thisTransform.y);
            }
            Vector2 dir = (Vector2)other.transform.position - thisTransform;
            pm.playerMovement.ApplyKnockback(dir * force);
        }
    }
    
    private void OnCollisionStay2D(Collision2D other)
    {
        PlayerManager pm = other.gameObject.GetComponent<PlayerManager>();
        if (pm != null)
        {
            Vector2 thisTransform = transform.position;
            if (justX)
            {
                thisTransform = new Vector2(thisTransform.x, other.transform.position.y);
            }
            if (justY)
            {
                thisTransform = new Vector2(other.transform.position.x, thisTransform.y);
            }
            Vector2 dir = (Vector2)other.transform.position - thisTransform;
            pm.playerMovement.ApplyKnockback(dir * force);
        }
    }
}
