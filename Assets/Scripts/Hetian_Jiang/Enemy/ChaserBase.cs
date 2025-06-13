using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ChaserBase : MonoBehaviour
{
    public LayerMask groundLayer;
    protected Transform player;

    [Header("Detection")]
    public float detectionRadius = 50f;

    protected virtual void Awake()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");

        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }

    protected bool CanSeePlayer()
    {
        if (player == null) return false;
        float distance = Vector3.Distance(transform.position, player.position);
        Debug.Log($"Distance to Player: {distance}");
        return distance <= detectionRadius;
    }

    protected abstract void Chase();
}

