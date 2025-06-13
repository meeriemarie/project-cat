using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HumanChaser : ChaserBase
{
    private NavMeshAgent _agent;

    protected override void Awake()
    {
        base.Awake();
        _agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (CanSeePlayer())
        {
            Chase();
        }
    }

    protected override void Chase()
    {
        if (player != null)
        {
            _agent.SetDestination(player.position);
        }
    }
}

