using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Controller
{
    [RequireComponent(typeof(CreatureMover))]
    public class KittyAIController : MonoBehaviour
    {
        public enum KittyState { Sitting, Wandering, CuriousApproach, CheckOnKitten, Fleeing }

        [Header("Targeting")]
        public Transform player;
        public string playerTag = "Player";
        
        [Tooltip("Assign 3 kittens here, or leave empty to auto-find by tag.")]
        public List<KittenAIController> kittens = new List<KittenAIController>();
        public string kittenTag = "Kitten";

        [Header("Player Distance Thresholds")]
        public float curiousRadius = 8f;   
        public float scareRadius = 2.5f;   
        public float safeFleeDistance = 10f;

        [Header("Motherly Instincts")]
        public float maxKittenDistance = 10f; 
        public float checkKittenStopDist = 2f; 

        [Header("Wandering & Rest")]
        public float wanderRadius = 7f;
        public float minRestTime = 3f;
        public float maxRestTime = 7f;

        [Header("Current State")]
        public KittyState currentState = KittyState.Sitting;

        private CreatureMover m_Mover;
        private Vector3 m_WanderTarget;
        private Vector3 m_FleeTarget;
        private KittenAIController m_TargetKitten;
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

            if (kittens.Count == 0)
            {
                GameObject[] kittenObjs = GameObject.FindGameObjectsWithTag(kittenTag);
                for (int i = 0; i < kittenObjs.Length; i++)
                {
                    KittenAIController k = kittenObjs[i].GetComponent<KittenAIController>();
                    if (k != null)
                    {
                        kittens.Add(k);
                        k.SetKittenIndex(i);
                    }
                }
            }
            else
            {
                for (int i = 0; i < kittens.Count; i++)
                {
                    if (kittens[i] != null) kittens[i].SetKittenIndex(i);
                }
            }

            ChangeState(KittyState.Sitting);
        }

        private void Update()
        {
            float distanceToPlayer = (player != null) ? Vector3.Distance(transform.position, player.position) : float.MaxValue;
            
            m_TargetKitten = GetFarthestStrayKitten();
            float distanceToFarthestKitten = (m_TargetKitten != null) ? Vector3.Distance(transform.position, m_TargetKitten.transform.position) : 0f;

            if (distanceToPlayer <= scareRadius)
            {
                if (currentState != KittyState.Fleeing)
                {
                    InterruptCoroutines();
                    CalculateFleeTarget();
                    ChangeState(KittyState.Fleeing);
                }
            }
            else if (m_TargetKitten != null && distanceToFarthestKitten > maxKittenDistance && currentState != KittyState.Fleeing)
            {
                if (currentState != KittyState.CheckOnKitten)
                {
                    InterruptCoroutines();
                    ChangeState(KittyState.CheckOnKitten);
                }
            }
            else if (distanceToPlayer <= curiousRadius && currentState != KittyState.Fleeing && currentState != KittyState.CheckOnKitten)
            {
                if (currentState != KittyState.CuriousApproach)
                {
                    InterruptCoroutines();
                    ChangeState(KittyState.CuriousApproach);
                }
            }
            else if (distanceToPlayer > curiousRadius && currentState == KittyState.CuriousApproach)
            {
                ChangeState(KittyState.Sitting);
            }

            switch (currentState)
            {
                case KittyState.Sitting:
                    KittenAIController closestKitten = GetClosestKitten();
                    Vector3 lookTarget = (closestKitten != null) ? closestKitten.transform.position + Vector3.up * 0.3f : transform.position + transform.forward;
                    m_Mover.SetInput(Vector2.zero, lookTarget, false, false);
                    break;

                case KittyState.Wandering:
                    ExecuteMoveTowards(m_WanderTarget, isRun: false);

                    Vector3 flatWanderPos = new Vector3(m_WanderTarget.x, transform.position.y, m_WanderTarget.z);
                    if (Vector3.Distance(transform.position, flatWanderPos) <= 0.6f)
                    {
                        ChangeState(KittyState.Sitting);
                    }
                    break;

                case KittyState.CuriousApproach:
                    if (distanceToPlayer > scareRadius + 1f)
                    {
                        ExecuteMoveTowards(player.position, isRun: false);
                    }
                    else
                    {
                        m_Mover.SetInput(Vector2.zero, player.position + Vector3.up * 0.5f, false, false);
                    }
                    break;

                case KittyState.CheckOnKitten:
                    if (m_TargetKitten != null)
                    {
                        if (distanceToFarthestKitten > checkKittenStopDist)
                        {
                            ExecuteMoveTowards(m_TargetKitten.transform.position, isRun: false);
                        }
                        else
                        {
                            ChangeState(KittyState.Sitting);
                        }
                    }
                    break;

                case KittyState.Fleeing:
                    ExecuteMoveTowards(m_FleeTarget, isRun: true);

                    if (distanceToPlayer >= safeFleeDistance)
                    {
                        ChangeState(KittyState.Sitting);
                    }
                    break;
            }
        }

        private KittenAIController GetFarthestStrayKitten()
        {
            KittenAIController farthest = null;
            float maxDist = 0f;

            foreach (var k in kittens)
            {
                if (k == null) continue;
                float d = Vector3.Distance(transform.position, k.transform.position);
                if (d > maxDist)
                {
                    maxDist = d;
                    farthest = k;
                }
            }
            return farthest;
        }

        private KittenAIController GetClosestKitten()
        {
            KittenAIController closest = null;
            float minDist = float.MaxValue;

            foreach (var k in kittens)
            {
                if (k == null) continue;
                float d = Vector3.Distance(transform.position, k.transform.position);
                if (d < minDist)
                {
                    minDist = d;
                    closest = k;
                }
            }
            return closest;
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

        private void ChangeState(KittyState newState)
        {
            currentState = newState;

            if (currentState == KittyState.Sitting && !m_IsCoroutineRunning)
            {
                StartCoroutine(RoutineSitting());
            }
            else if (currentState == KittyState.Wandering)
            {
                SetRandomWanderTarget();
            }
        }

        private IEnumerator RoutineSitting()
        {
            m_IsCoroutineRunning = true;
            float sitDuration = Random.Range(minRestTime, maxRestTime);
            yield return new WaitForSeconds(sitDuration);
            m_IsCoroutineRunning = false;

            if (currentState == KittyState.Sitting)
            {
                ChangeState(KittyState.Wandering);
            }
        }

        private void SetRandomWanderTarget()
        {
            Vector2 randomCircle = Random.insideUnitCircle * wanderRadius;
            m_WanderTarget = transform.position + new Vector3(randomCircle.x, 0f, randomCircle.y);
        }

        private void CalculateFleeTarget()
        {
            Vector3 fleeDirection = (transform.position - player.position).normalized;
            m_FleeTarget = transform.position + fleeDirection * safeFleeDistance;
        }

        private void InterruptCoroutines()
        {
            StopAllCoroutines();
            m_IsCoroutineRunning = false;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, curiousRadius);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, scareRadius);
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, maxKittenDistance);
        }
    }
}