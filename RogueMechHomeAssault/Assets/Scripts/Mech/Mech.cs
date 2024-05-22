using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class Mech : MonoBehaviour
{
    [SerializeField] NavMeshAgent agent;
    [SerializeField] float walkSpeed = 1.0f;

    private bool isWalking = false;
    private Vector3 currentDestination;

    public UnityAction OnDestinationReached;

    private void Awake()
    {
        agent.speed = walkSpeed;
    }

    public void MoveTo(Vector3 position)
    {
        if (position == currentDestination)
        {
            OnDestinationReached?.Invoke();
            return;
        }

        currentDestination = position;
        isWalking = true;
        agent.SetDestination(position);
    }

    void Update()
    {
        if (isWalking)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    Debug.Log("Agent has reached its destination!");
                    isWalking = false;
                    DestinationReached();
                }
            }
        }
    }

    private void DestinationReached()
    {
        OnDestinationReached?.Invoke();
    }
}
