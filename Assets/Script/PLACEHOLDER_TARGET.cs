using System;
using UnityEngine;

public class PLACEHOLDER_TARGET : MonoBehaviour
{
    public static PLACEHOLDER_TARGET instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
        }
        instance = this;
    }
}
