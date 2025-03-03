using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : AYENpc<EnemyBehavior>
{
    #region Initialization and Registration of States
    protected override void Awake()
    {
        base.Awake(); // Call the base class to avoid breaking the program

        // Registering the methods with the base class
        AddStatus(EnemyBehavior.Idle, EnterIdle, IdleState, ExitIdle);
        AddStatus(EnemyBehavior.Patrol, EnterPatrol, PatrolState, ExitPatrol);
        AddStatus(EnemyBehavior.Suspicious, EnterSuspicious, SuspiciousState, ExitSuspicious);
        AddStatus(EnemyBehavior.Chase, EnterChase, ChaseState, ExitChase);
        AddStatus(EnemyBehavior.Attack, EnterAttack, AttackState, ExitAttack);
    }
    #endregion

    #region Idle
    void EnterIdle()
    {
        // Look straight ahead
        look = headForward;
    }
    void IdleState()
    {
        // If idle for two seconds
        if (statusTime > 2f)
        {
            // Switch state to Patrol
            status = EnemyBehavior.Patrol;
            return;
        }
    }
    void ExitIdle()
    {
        
    }
    #endregion

    #region Patrol
    Vector3 patrolDestination = Vector3.zero;
    void EnterPatrol()
    {
        // 1. Decide where to go
        if (suspiciousPosition != Vector3.zero)
        {
            patrolDestination = GetNavigationPos(suspiciousPosition);
            suspiciousPosition = Vector3.zero;
        }
        else
        {
            patrolDestination = GetRandomNavigationPos(10f, 100f);
        }
        patrolDestination = GetRandomNavigationPos(10f, 100f);
        // 2. Play walking animation
        animator.SetBool("Walk", true);
    }
    void PatrolState()
    {
        // Look straight ahead
        look = headForward;
        // Continuously check the navigation path
        Vector3 nextWaypoint = GetNavigationCorners(patrolDestination);
        // Face the waypoint
        face = nextWaypoint;
        // Set turning speed to 2
        faceSpeed = 2f;
        // If close to the destination, return to Idle
        if (IsClose(patrolDestination, 1f))
        {
            status = EnemyBehavior.Idle;
            return;
        }
        // Maximum patrol time
        if (statusTime > 30f)
        {
            status = EnemyBehavior.Idle;
            return;
        }

        // If the angle to the next waypoint is large, stop and wait for the body to turn
        animator.SetBool("Walk", faceAngle < 30f);
    }
    void ExitPatrol()
    {
        // Cancel the walking animation at the end of the patrol
        animator.SetBool("Walk", false);
    }
    #endregion

    #region Suspicious
    [SerializeField] float suspicionTime = 1f;
    [SerializeField] float timeToConfirmPlayer = 1f;
    float timeSeenPlayer = 0f;
    void EnterSuspicious()
    {
        timeSeenPlayer = 0f;
    }
    void SuspiciousState()
    {
        // Stare at the player
        look = Player.head.ins.transform.position;
        lookSpeed = 5f;

        // Face the player
        face = Player.head.ins.transform.position;
        faceSpeed = 0.1f;

        if (seenPlayer)
        {
            timeSeenPlayer += Time.deltaTime;
        }

        // Timeout and give up
        if (statusTime > suspicionTime && timeSeenPlayer < timeToConfirmPlayer)
        {
            status = EnemyBehavior.Patrol;
            return;
        }
        // Seen the player long enough
        else if (timeSeenPlayer > timeToConfirmPlayer)
        {
            status = EnemyBehavior.Chase;
            return;
        }
    }
    void ExitSuspicious()
    {

    }
    #endregion

    #region Chase
    float chasePatience = 0f;
    [SerializeField] float patienceTimeAfterSeeingPlayer = 3f;
    void EnterChase()
    {
        animator.SetBool("Run", true);
        chasePatience = patienceTimeAfterSeeingPlayer;
    }
    void ChaseState()
    {
        Vector3 nextWaypoint = GetNavigationCorners(Player.head.ins.transform.position);
        face = nextWaypoint;
        faceSpeed = 20f;

        look = Player.head.ins.transform.position;
        lookSpeed = 20f;

        if (seenPlayer)
        {
            chasePatience = patienceTimeAfterSeeingPlayer;
        }
        else
        {
            return;
        }

        chasePatience -= Time.deltaTime;
        if (chasePatience <= 0f)
        {
            status = EnemyBehavior.Idle;
            return;
        }

        if (IsClose(Player.head.ins.transform.position, 1.5f))
        {
            // Debug.Log("You are dead!");
            // status = EnemyBehavior.Idle;
            status = EnemyBehavior.Attack;
            return;
        }

        if (IsClose(Player.head.ins.transform.position, 0f))
        {
            Debug.Log("Return to Patrol!");
            status = EnemyBehavior.Patrol;
            return;
        }
    }
    void ExitChase()
    {
        lookSpeed = 5f;
        animator.SetBool("Run", false);
    }
    #endregion

    #region Attack
	void EnterAttack()
	{
		animator.SetTrigger("Attack");
	}
	void AttackState()
	{
		// Continually look at the player
		face = Player.head.ins.transform.position;
		if (statusTime > 3f)
		{
			status = EnemyBehavior.Idle;
			return;
		}
	}
	void ExitAttack()
	{
		animator.ResetTrigger("Attack");
	}
	public void OnSwing()
	{
		float distanceToPlayer = Vector3.Distance(this.transform.position, Player.head.ins.transform.root.position);
		if (distanceToPlayer < 3.5f)
		{
			// Call the player knockout function
			Player.head.ins.transform.root.gameObject.SendMessage("GetKnockedOut");
			lossOfVisionTime = Time.time + 10f;
		}
	}
	#endregion

	#region Collision Detection
	private void OnCollisionEnter(Collision collision)
	{
		// If I collide with StopAi
		if (collision.collider.tag == "StopAi")
		{
			// If patrolling, cancel
			if (status == EnemyBehavior.Patrol)
			{
				status = EnemyBehavior.Idle;
				return;
			}
		}
	}
	#endregion

	#region Vision
	[SerializeField] float visionDistance = 10f;
	[SerializeField] float crouchVisionDistance = 3f;
	[SerializeField] float visionAngle = 100f;
	[SerializeField] LayerMask visionBlockingLayers = 0;
	bool seenPlayer = false;
	float lossOfVisionTime = 0f;
	protected override void FixedUpdate()
	{
		base.FixedUpdate();
		seenPlayer = false;
		float distanceToPlayer = Vector3.Distance(head.position, Player.head.ins.transform.position);

		// Mixed calculation of vision distance
		float detectionDistance = visionDistance;
		// Shorten when crouching
		if (PlayerMove.instance.runMix <= -0.5f)
		{
			detectionDistance = crouchVisionDistance;
		}
		// Halve the vision distance if the flashlight is off
		if (PlayerMove.instance.isFlashlightOn == false)
		{
			detectionDistance *= 0.5f;
		}

		if (distanceToPlayer < detectionDistance && Time.time > lossOfVisionTime && PlayerMove.instance.isDown == false)
		{
			Vector3 directionToPlayer = head.forward;
			Vector3 directionToPlayerRelative = Player.head.ins.transform.position - head.position;

			float angleToPlayer = Vector3.Angle(directionToPlayer, directionToPlayerRelative);

			// Within the vision cone in front of me
			if (angleToPlayer < visionAngle * 0.5f)
			{
				bool rayHit = Physics.Raycast(head.position, directionToPlayerRelative, directionToPlayerRelative.magnitude, visionBlockingLayers);
				// If the ray doesn't hit anything, it means the guard's head and the player's head are unobstructed
				if (!rayHit)
				{
					seenPlayer = true;
					if (status == EnemyBehavior.Patrol || status == EnemyBehavior.Idle)
					{
						status = EnemyBehavior.Suspicious;
						return;
					}
				}
			}
		}
	}
	#endregion

	#region Hearing
	Vector3 patrolSuspiciousPosition = Vector3.zero;
	public void OnSound(Vector3 position)
	{
		// If the player is already down, hear nothing
		if (PlayerMove.instance.isDown == true)
		{
			return;
		}
		if (status == EnemyBehavior.Idle || status == EnemyBehavior.Patrol)
		{
			patrolSuspiciousPosition = position;
			status = EnemyBehavior.Patrol;
		}
	}
	#endregion
}

public enum EnemyBehavior
{
    Idle, 
    Patrol,
    Suspicious,
    Chase,
    Attack,
}

