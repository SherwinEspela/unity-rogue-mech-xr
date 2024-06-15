using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public enum MechActionState
{
    Idle, 
    Evading,
    Attacking
}

public class Mech : MonoBehaviour
{
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Animator animator;
    [SerializeField] MechVision mechVision;

    [SerializeField] float walkSpeed = 1.0f;

    private Vector3 currentDestination;
    private MechActionState mechActionState = MechActionState.Idle;
    private PlayerCharacter player;

    private const string TRIGGER_WALK = "TriggerWalk";
    private const string TRIGGER_IDLE = "TriggerIdle";

    public PlayerCharacter Player {
        get { return player; }
        set {
            mechVision.Player = value;
            player = value;
        }
    }

    public UnityAction OnDestinationReached;
    public UnityAction<Mech> OnMechDestinationReached;
    public UnityAction<Mech> OnMechEvaded;

    private void Awake()
    {
        agent.speed = walkSpeed;
        agent.stoppingDistance = 0.0f;

        mechVision.OnPlayerSeen += HandlePlayerSeen;
    }

    public void MoveTo(Vector3 position)
    {
        if (position == currentDestination)
        {
            DestinationReached();
            return;
        }

        currentDestination = position;
        mechActionState = MechActionState.Evading;
        agent.speed = walkSpeed;
        animator.SetTrigger(TRIGGER_WALK);
        agent.SetDestination(position);
    }

    public void Attack()
    {
        if (!Player) return;
        if (Vector3.Distance(this.transform.position, Player.transform.position) <= 2.0f)
        {
            MakeDecision();
            return;
        }

        Debug.Log("Attack....");
        mechActionState = MechActionState.Attacking;
        agent.speed = walkSpeed;
        animator.SetTrigger(TRIGGER_WALK);
    }

    public void Evade()
    {
        Debug.Log("Evade...");
        mechActionState = MechActionState.Evading;
        OnMechEvaded?.Invoke(this);
    }

    void Update()
    {
        UpdateMechMovement();
    }

    private void UpdateMechMovement()
    {
        switch (mechActionState)
        {
            case MechActionState.Idle:
                break;
            case MechActionState.Evading:
                if (Vector3.Distance(this.transform.position, currentDestination) <= 0.1f)
                {
                    DestinationReached();
                }
                break;
            case MechActionState.Attacking:
                if (Vector3.Distance(this.transform.position, Player.transform.position) <= 2.0f)
                {
                    DestinationReached();
                }
                else
                {
                    agent.SetDestination(Player.transform.position);
                }
                break;
            default:
                break;
        }
    }

    private void DestinationReached()
    {
        agent.speed = 0;
        mechActionState = MechActionState.Idle;
        animator.SetTrigger(TRIGGER_IDLE);

        MakeDecision();
    }

    private void MakeDecision()
    {
        int decision = Random.Range(0, 2);

        switch (decision)
        {
            case 0:
                Attack();
                break;

            case 1:
                Evade();
                break;

            default:
                break;
        }
    }

    private void HandlePlayerSeen()
    {
        Debug.Log("Player seen!!!!");
    }
}
