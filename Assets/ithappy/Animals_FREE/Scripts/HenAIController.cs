using System.Collections;
using UnityEngine;

namespace Controller
{
    [RequireComponent(typeof(CreatureMover))]
    public class HenAIController : MonoBehaviour
    {
        public enum HenState { Idle, Wandering, ApproachingPlayer }

        [Header("Targeting")]
        public Transform player;
        public string playerTag = "Player";

        [Header("Detection Settings")]
        public float detectionRadius = 6f;
        public float stopDistance = 2f;

        [Header("Wandering Settings")]
        public float wanderRadius = 6f;
        public float minIdleTime = 2f;
        public float maxIdleTime = 5f;

        [Header("Current State")]
        public HenState currentState = HenState.Idle;

        private CreatureMover m_Mover;
        private Vector3 m_WanderTarget;
        private bool m_IsCoroutineRunning;

        private void Awake()
        {
            m_Mover = GetComponent<CreatureMover>();
        }

        private void Start()
        {
            if (player == null)
            {
                GameObject playerObj = GameObject.FindGameObjectWithTag(playerTag);
                if (playerObj != null) player = playerObj.transform;
            }

            ChangeState(HenState.Idle);
        }

        private void Update()
        {
            if (player == null)
            {
                m_Mover.SetInput(Vector2.zero, transform.position + transform.forward, false, false);
                return;
            }

            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            // Player Detection
            if (distanceToPlayer <= detectionRadius)
            {
                if (currentState != HenState.ApproachingPlayer)
                {
                    StopAllCoroutines();
                    m_IsCoroutineRunning = false;
                    ChangeState(HenState.ApproachingPlayer);
                }
            }
            else if (currentState == HenState.ApproachingPlayer)
            {
                ChangeState(HenState.Idle);
            }

            // State Logic Execution
            switch (currentState)
            {
                case HenState.Idle:
                    // Stop moving, keep facing current forward
                    m_Mover.SetInput(Vector2.zero, transform.position + transform.forward, false, false);
                    break;

                case HenState.Wandering:
                    ExecuteMoveTowards(m_WanderTarget, false);

                    // Check if reached wander destination
                    Vector3 flatWanderPos = new Vector3(m_WanderTarget.x, transform.position.y, m_WanderTarget.z);
                    if (Vector3.Distance(transform.position, flatWanderPos) <= 0.5f)
                    {
                        ChangeState(HenState.Idle);
                    }
                    break;

                case HenState.ApproachingPlayer:
                    if (distanceToPlayer > stopDistance)
                    {
                        // Walk toward player
                        ExecuteMoveTowards(player.position, false);
                    }
                    else
                    {
                        // Close enough to player: stop walking, look at player for IK
                        m_Mover.SetInput(Vector2.zero, player.position + Vector3.up * 1.2f, false, false);
                    }
                    break;
            }
        }

        private void ExecuteMoveTowards(Vector3 targetPos, bool isRun)
        {
            Vector3 direction = (targetPos - transform.position);
            direction.y = 0; // Keep movement on 2D horizontal plane

            if (direction.sqrMagnitude > 0.01f)
            {
                // Translate direction into local movement input for CreatureMover
                Vector2 moveInput = new Vector2(0f, 1f); // Drive forward along target path
                m_Mover.SetInput(moveInput, transform.position + direction.normalized * 5f, isRun, false);
            }
            else
            {
                m_Mover.SetInput(Vector2.zero, targetPos, false, false);
            }
        }

        private void ChangeState(HenState newState)
        {
            currentState = newState;

            if (currentState == HenState.Idle && !m_IsCoroutineRunning)
            {
                StartCoroutine(RoutineIdle());
            }
            else if (currentState == HenState.Wandering)
            {
                SetRandomWanderTarget();
            }
        }

        private IEnumerator RoutineIdle()
        {
            m_IsCoroutineRunning = true;

            float idleDuration = Random.Range(minIdleTime, maxIdleTime);
            yield return new WaitForSeconds(idleDuration);

            m_IsCoroutineRunning = false;

            if (currentState == HenState.Idle)
            {
                ChangeState(HenState.Wandering);
            }
        }

        private void SetRandomWanderTarget()
        {
            Vector2 randomCircle = Random.insideUnitCircle * wanderRadius;
            m_WanderTarget = transform.position + new Vector3(randomCircle.x, 0f, randomCircle.y);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, detectionRadius);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, stopDistance);
            
            if (currentState == HenState.Wandering)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawSphere(m_WanderTarget, 0.3f);
            }
        }
    }
}