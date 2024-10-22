using System;
using UnityEngine;
using UnityEngine.AI;


public class MoveDestination : MonoBehaviour
{

    [SerializeField] private Transform goal;
    private NavMeshAgent _agent;
    private void Start()
    {
        _agent  = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        _agent.destination = goal.position;
    }

    public void StopMoving()
    {
        _agent.isStopped = true;
    }
}

