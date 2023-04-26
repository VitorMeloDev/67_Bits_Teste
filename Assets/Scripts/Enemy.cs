using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private BoxCollider trigger;
    private Rigidbody[] ragdollRb;

    void Awake()
    { 
        ragdollRb = GetComponentsInChildren<Rigidbody>();
        DisableRagdoll();
    }
    
    public void DisableRagdoll()
    {
        foreach (var rb in ragdollRb)
        {
            rb.isKinematic = true;
        }
    }

    public void EnableRagdoll()
    {
        foreach (var rb in ragdollRb)
        {
            rb.isKinematic = false;
        }

        Destroy(this.gameObject,2);
    }

    public void Golpe()
    {
        trigger.enabled = false;
        EnableRagdoll();
    }
}
