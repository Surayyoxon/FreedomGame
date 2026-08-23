using System.Collections;
using UnityEngine;

namespace Controller
{
    [RequireComponent(typeof(CreatureMover))]
    public class KittenAIController : MonoBehaviour
    {
        public enum KittenState { PlayfulIdle, FastWander, FollowMother, ApproachPlayer, StartledFlee }

        [Header("Targeting")]
        public Transform player;
        public string playerTag = "Player";
        public KittyAIController motherKitty;
        public string motherTag = "Kitty";

        [Header("Distance Thresholds")]
        public float playerFollowRadius = 8f;   
        public float startleRadius = 1.8f;       
        public float motherFollowRadius = 12f;   
        public float motherStopDistance = 2.5f;  

        [Header("Movement Tuning")]
        public float wanderRadius = 4f;
        public float minPauseTime = 1f;
        public float maxPauseTime = 3f;

        [Header("Current State")]
        public KittenState currentState = KittenState.PlayfulIdle;

        private CreatureMover m_Mover;
        private Vector3 m_WanderTarget;
        private Vector3 m_FleeTarget;
        private bool m_IsCoroutineRunning;
        private int m_KittenIndex = 0; // Spreads kittens around targets

        private void Awake()
        {
            m_Mover = GetComponent<CreatureMover>();
        }

        public void SetKittenIndex(int index)
        {
            m_KittenIndex = index;
        }

        private void Start()
        {
            if (player == null)
            {
                GameObject playerObj = GameObject.FindGameObjectWithTag(playerTag);
                if (playerObj != null) player = playerObj.transform;
            }

            if (motherKitty == null)
            {
                GameObject motherObj = GameObject.FindGameObjectWithTag(motherTag);
                if (motherObj != null) motherKitty = motherObj.GetComponent<KittyAIController>();
            }

            ChangeState(KittenState.PlayfulIdle);
        }

        private void Update()
        {
            float distanceToPlayer = (player != null) ? Vector3.Distance(transform.position, player.position) : float.MaxValue;
            float distanceToMother = (motherKitty != null) ? Vector3.Distance(transform.position, motherKitty.transform.position) : 0f;

            if (distanceToPlayer <= startleRadius && currentState != KittenState.StartledFlee)
            {
                InterruptCoroutines();
                CalculateFleeTarget();
                ChangeState(KittenState.StartledFlee);
            }
            else if (motherKitty != null && distanceToMother > motherFollowRadius && currentState != KittenState.StartledFlee)
            {
                if (currentState != KittenState.FollowMother)
                {
                    InterruptCoroutines();
                    ChangeState(KittenState.FollowMother);
                }
            }
            else if (distanceToPlayer <= playerFollowRadius && currentState != KittenState.StartledFlee && currentState != KittenState.FollowMother)
            {
                if (currentState != KittenState.ApproachPlayer)
                {
                    InterruptCoroutines();
                    ChangeState(KittenState.ApproachPlayer);
                }
            }
            else if (distanceToPlayer > playerFollowRadius && currentState == KittenState.ApproachPlayer)
            {
                ChangeState(KittenState.PlayfulIdle);
            }

            // State Execution
            switch (currentState)
            {
                case KittenState.PlayfulIdle:
                    Vector3 lookTarget = (motherKitty != null) ? motherKitty.transform.position + Vector3.up * 0.4f : player.position;
                    m_Mover.SetInput(Vector2.zero, lookTarget, false, false);
                    break;

                case KittenState.FastWander:
                    ExecuteMoveTowards(m_WanderTarget, isRun: true);

                    Vector3 flatWanderPos = new Vector3(m_WanderTarget.x, transform.position.y, m_WanderTarget.z);
                    if (Vector3.Distance(transform.position, flatWanderPos) <= 0.5f)
                    {
                        ChangeState(KittenState.PlayfulIdle);
                    }
                    break;

                case KittenState.FollowMother:
                    if (motherKitty != null)
                    {
                        Vector3 offsetPos = GetOffsetPosition(motherKitty.transform.position, motherStopDistance);
                        if (Vector3.Distance(transform.position, offsetPos) > 0.6f)
                        {
                            ExecuteMoveTowards(offsetPos, isRun: true);
                        }
                        else
                        {
                            ChangeState(KittenState.PlayfulIdle);
                        }
                    }
                    break;

                case KittenState.ApproachPlayer:
                    Vector3 playerOffsetPos = GetOffsetPosition(player.position, startleRadius + 1.2f);
                    if (Vector3.Distance(transform.position, playerOffsetPos) > 0.6f)
                    {
                        ExecuteMoveTowards(playerOffsetPos, isRun: true);
                    }
                    else
                    {
                        m_Mover.SetInput(Vector2.zero, player.position + Vector3.up * 0.8f, false, false);
                    }
                    break;

                case KittenState.StartledFlee:
                    ExecuteMoveTowards(m_FleeTarget, isRun: true);

                    if (Vector3.Distance(transform.position, m_FleeTarget) <= 0.8f || distanceToPlayer > playerFollowRadius)
                    {
                        ChangeState(KittenState.PlayfulIdle);
                    }
                    break;
            }
        }

        private Vector3 GetOffsetPosition(Vector3 centerTarget, float distanceRadius)
        {
            float angle = m_KittenIndex * 120f;
            Vector3 dir = Quaternion.Euler(0, angle, 0) * Vector3.forward;
            return centerTarget + dir * distanceRadius;
        }

        private void ExecuteMoveTowards(Vector3 targetPos, bool isRun)
        {
            Vector3 direction = (targetPos - transform.position);
            direction.y = 0;

            if (direction.sqrMagnitude > 0.01f)
            {
                m_Mover.SetInput(new Vector2(0f, 1f), transform.position + direction.normalized * 5f, isRun, false);
            }
            else
            {
                m_Mover.SetInput(Vector2.zero, targetPos, false, false);
            }
        }

        private void ChangeState(KittenState newState)
        {
            currentState = newState;

            if (currentState == KittenState.PlayfulIdle && !m_IsCoroutineRunning)
            {
                StartCoroutine(RoutinePlayfulPause());
            }
            else if (currentState == KittenState.FastWander)
            {
                SetRandomWanderTarget();
            }
        }

        private IEnumerator RoutinePlayfulPause()
        {
            m_IsCoroutineRunning = true;
            float pauseDuration = Random.Range(minPauseTime, maxPauseTime);
            yield return new WaitForSeconds(pauseDuration);
            m_IsCoroutineRunning = false;

            if (currentState == KittenState.PlayfulIdle)
            {
                ChangeState(KittenState.FastWander);
            }
        }

        private void SetRandomWanderTarget()
        {
            Vector3 origin = (motherKitty != null) ? motherKitty.transform.position : transform.position;
            Vector2 randomCircle = Random.insideUnitCircle * wanderRadius;
            m_WanderTarget = origin + new Vector3(randomCircle.x, 0f, randomCircle.y);
        }

        private void CalculateFleeTarget()
        {
            Vector3 fleeDirection = (transform.position - player.position).normalized;
            m_FleeTarget = transform.position + fleeDirection * 6f;
        }

        private void InterruptCoroutines()
        {
            StopAllCoroutines();
            m_IsCoroutineRunning = false;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, playerFollowRadius);
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(transform.position, startleRadius);

            if (motherKitty != null)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawWireSphere(motherKitty.transform.position, motherFollowRadius);
            }
        }
    }
}