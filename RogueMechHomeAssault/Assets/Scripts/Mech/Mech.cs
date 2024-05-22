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
        animator.SetTrigger(TRIGGER_WALK);
        agent.SetDestination(position);
    }

    void Update()
    {
        if (isWalking)
        {
            //if (agent.remainingDistance <= agent.stoppingDistance)
            //{
            //    if (!agent.hasPath || agent.velocity.sqrMagnitude == 0.0f)
            //    {
            //        DestinationReached();
            //    }
            //}

            //var distance = Vector3.Distance(this.transform.position, currentDestination);
            //Debug.Log($"distance value = {distance}");

            if (Vector3.Distance(this.transform.position, currentDestination) <= 1.0f)
            {
                DestinationReached();
            }
        }
    }

    private void DestinationReached()
    {
        isWalking = false;
        animator.SetTrigger(TRIGGER_IDLE);

        OnDestinationReached?.Invoke();
    }
}
