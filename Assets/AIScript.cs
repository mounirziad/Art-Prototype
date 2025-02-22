using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIScript : MonoBehaviour
{
    public NavMeshAgent navMeshAgent;
    public float startWaitTime = 4;
    public float speedWalk = 6;
    public float speedRun = 9;

    public float viewRadius = 15;
    public float viewAngle = 90;
    public LayerMask playerMask;
    public LayerMask obstacleMask;

    public Transform[] waypoints;

    private int m_CurrentWaypointIndex;
    private Vector3 playerLastPosition = Vector3.zero;
    private Vector3 m_PlayerPosition;

    private float m_WaitTime;
    private bool m_PlayerInRange;
    private bool m_PlayerNear;
    private bool m_IsPatrol;
    private bool m_CaughtPlayer;
    private bool isColliding = false;

    void Start()
    {
        m_PlayerPosition = Vector3.zero;
        m_IsPatrol = true;
        m_CaughtPlayer = false;
        m_PlayerInRange = false;
        m_WaitTime = startWaitTime;

        m_CurrentWaypointIndex = 0;
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.isStopped = false;
        navMeshAgent.speed = speedWalk;
        navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);

        navMeshAgent.updateRotation = false; // Disable automatic rotation

        // Disable physics-based movement
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
        }

        // Adjust NavMeshAgent settings
        navMeshAgent.stoppingDistance = 0.5f;
        navMeshAgent.obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance;
        navMeshAgent.avoidancePriority = 50;
        navMeshAgent.angularSpeed = 120f;
        navMeshAgent.acceleration = 8f;
    }

    void Update()
    {
        EnvironmentView();

        if (!m_IsPatrol)
        {
            Chasing();
        }
        else
        {
            Patroling();
        }

        RotateTowardsMovementDirection(); // Ensure proper rotation
    }

    private void Chasing()
    {
        m_PlayerNear = false;
        playerLastPosition = Vector3.zero;

        if (!m_CaughtPlayer)
        {
            Move(speedRun);

            GameObject player = GameObject.FindGameObjectWithTag("Player");
            GameObject canPickUp = GameObject.FindGameObjectWithTag("canPickUp");

            if (player != null && canPickUp != null)
            {
                float playerDistance = Vector3.Distance(transform.position, player.transform.position);
                float canPickUpDistance = Vector3.Distance(transform.position, canPickUp.transform.position);

                if (playerDistance < canPickUpDistance)
                {
                    navMeshAgent.SetDestination(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));
                }
                else
                {
                    navMeshAgent.SetDestination(new Vector3(canPickUp.transform.position.x, transform.position.y, canPickUp.transform.position.z));
                }
            }
            else if (player != null)
            {
                navMeshAgent.SetDestination(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));
            }
            else if (canPickUp != null)
            {
                navMeshAgent.SetDestination(new Vector3(canPickUp.transform.position.x, transform.position.y, canPickUp.transform.position.z));
            }
        }

        if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            if (m_WaitTime <= 0 && !m_CaughtPlayer)
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                GameObject canPickUp = GameObject.FindGameObjectWithTag("canPickUp");

                if (player != null && Vector3.Distance(transform.position, player.transform.position) >= 6f &&
                    canPickUp != null && Vector3.Distance(transform.position, canPickUp.transform.position) >= 6f)
                {
                    m_IsPatrol = true;
                    m_PlayerNear = false;
                    Move(speedWalk);
                    m_WaitTime = startWaitTime;
                    navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
                }
            }
            else
            {
                Stop();
                m_WaitTime -= Time.deltaTime;
            }
        }
    }

    private void Patroling()
    {
        if (m_PlayerNear)
        {
            Move(speedWalk);
            LookingPlayer(playerLastPosition);
        }
        else
        {
            m_PlayerNear = false;
            playerLastPosition = Vector3.zero;
            navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);

            if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
            {
                if (m_WaitTime <= 0)
                {
                    NextPoint();
                    Move(speedWalk);
                    m_WaitTime = startWaitTime;
                }
                else
                {
                    Stop();
                    m_WaitTime -= Time.deltaTime;
                }
            }
        }
    }

    void Move(float speed)
    {
        navMeshAgent.isStopped = false;
        navMeshAgent.speed = speed;
    }

    void Stop()
    {
        navMeshAgent.isStopped = true;
        navMeshAgent.speed = 0;
    }

    public void NextPoint()
    {
        m_CurrentWaypointIndex = (m_CurrentWaypointIndex + 1) % waypoints.Length;
        navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
    }

    void CaughtPlayer()
    {
        m_CaughtPlayer = true;
    }

    void LookingPlayer(Vector3 player)
    {
        navMeshAgent.SetDestination(new Vector3(player.x, transform.position.y, player.z));

        if (Vector3.Distance(transform.position, player) <= 0.3)
        {
            if (m_WaitTime <= 0)
            {
                m_PlayerNear = false;
                Move(speedWalk);
                navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
                m_WaitTime = startWaitTime;
            }
            else
            {
                Stop();
                m_WaitTime -= Time.deltaTime;
            }
        }
    }

    void EnvironmentView()
    {
        Collider[] targetsInRange = Physics.OverlapSphere(transform.position, viewRadius, playerMask | LayerMask.GetMask("canPickUp"));

        Transform closestTarget = null;
        float closestDistance = Mathf.Infinity;

        foreach (Collider target in targetsInRange)
        {
            Transform targetTransform = target.transform;
            Vector3 dirToTarget = (targetTransform.position - transform.position).normalized;
            float distanceToTarget = Vector3.Distance(transform.position, targetTransform.position);

            // Check if the target is within the view radius and not blocked by obstacles
            if (!Physics.Raycast(transform.position, dirToTarget, distanceToTarget, obstacleMask))
            {
                if (distanceToTarget < closestDistance)
                {
                    closestDistance = distanceToTarget;
                    closestTarget = targetTransform;
                }
            }
        }

        if (closestTarget != null)
        {
            m_IsPatrol = false;
            m_PlayerInRange = true;
            m_PlayerPosition = closestTarget.position;
        }
        else
        {
            m_PlayerInRange = false;
        }
    }

    void RotateTowardsMovementDirection()
    {
        if (!m_IsPatrol && m_PlayerInRange)
        {
            // Calculate the distance to the player
            float distanceToPlayer = Vector3.Distance(transform.position, m_PlayerPosition);

            // Only rotate if the rat is not too close to the player
            if (distanceToPlayer > 0.5f) // Adjust this threshold as needed
            {
                // During chase, rotate towards the player with 360-degree freedom
                Vector3 directionToPlayer = (m_PlayerPosition - transform.position).normalized;
                directionToPlayer.y = 0; // Ensure the character doesn't tilt up or down
                if (directionToPlayer != Vector3.zero) // Avoid errors when direction is zero
                {
                    // Calculate the target rotation
                    Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);

                    // Apply an offset to compensate for the model's default rotation
                    float rotationOffset = -270f; // Adjust this value based on your model's default rotation
                    targetRotation *= Quaternion.Euler(0, rotationOffset, 0);

                    // Smoothly interpolate towards the target rotation
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f); // Adjust rotation speed as needed
                }
            }
        }
        else if (m_IsPatrol && navMeshAgent.velocity.sqrMagnitude > 0.01f)
        {
            // During patrol, rotate based on movement direction (left or right)
            float direction = navMeshAgent.velocity.x;
            if (direction > 0)
            {
                transform.rotation = Quaternion.Euler(0, 180, 0); // Face right
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 0, 0); // Face left
            }
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        if ((collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("canPickUp")) && !isColliding)
        {
            // Stop the NavMeshAgent during the collision
            navMeshAgent.isStopped = true;
            isColliding = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if ((collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("canPickUp")) && isColliding)
        {
            // Resume the NavMeshAgent after a short delay
            StartCoroutine(ResumeAfterCollision());
        }
    }

    IEnumerator ResumeAfterCollision()
    {
        yield return new WaitForSeconds(0.5f); // Adjust delay as needed
        navMeshAgent.isStopped = false;
        isColliding = false;
    }
}