using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Mech : MonoBehaviour
{
    [SerializeField] NavMeshAgent agent;

    private void MoveTo(Vector3 position)
    {
        agent.SetDestination(position);
    }
}
