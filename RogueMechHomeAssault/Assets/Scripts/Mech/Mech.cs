using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class Mech : MonoBehaviour
{
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Animator animator;

    [SerializeField] float walkSpeed = 1.0f;

    private bool isWalking = false;
    private Vector3 currentDestination;

    private const string TRIGGER_WALK = "TriggerWalk";
    private const string TRIGGER_IDLE = "TriggerIdle";

    public UnityAction OnDestinationReached;

    private void Awake()
    {
        agent.speed = walkSpeed;
        agent.stoppingDistance = 0.0f;
    }

    public void MoveTo(Vector3 position)
    {
        if (position == currentDestination)
        {
            DestinationReached();
            return;
        }

        currentDestination = position;
        isWalking = true;
        agent.speed = walkSpeed;
        animator.SetTrigger(TRIGGER_WALK);
        agent.SetDestination(position);
    }

    void Update()
    {
        if (isWalking)
        {
            if (Vector3.Distance(this.transform.position, currentDestination) <= 0.1f)
            {
                DestinationReached();
            }
        }
    }

    private void DestinationReached()
    {
        agent.speed = 0;
        isWalking = false;
        animator.SetTrigger(TRIGGER_IDLE);

        OnDestinationReached?.Invoke();
    }
}
