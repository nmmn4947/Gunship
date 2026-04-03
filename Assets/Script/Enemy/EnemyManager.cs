using System;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private float rotateSpeed;
    private Transform target;

    private void Start()
    {
        target = FindAnyObjectByType<PlayerManager>().transform;
        if (target == null) { Debug.LogError("Cant find player"); }
    }

    private void Update()
    {
        if (target != null)
        {
            Vector2 direction = target.position - transform.position;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            float step = rotateSpeed * Time.deltaTime;

            Quaternion targetRotation = Quaternion.Euler(0, 0, angle - 90f);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, step);

        }
    }
}
