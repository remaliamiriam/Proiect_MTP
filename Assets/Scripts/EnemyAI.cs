using UnityEngine;
using UnityEngine.AI;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAI : MonoBehaviour
{
    public Transform player;
    public float detectionRadius = 7f;
    public float attackRange = 1.5f;
    public float attackCooldown = 1f;
    public float pushForce = 6f;
    public float loseTargetDistance = 10f;

    public Transform[] patrolPoints;
    public ParticleSystem aggroParticles;
    public AudioClip aggroSound;
    public AudioClip attackSound;
    public AudioClip deathSound;

    [Header("Speed")]
    public float patrolSpeed = 3.5f;
    public float chaseSpeed = 5.5f;

    private int currentPatrolIndex = 0;
    private bool patrolForward = true;
    private float lastAttackTime = -999f;
    private bool isChasing = false;
    private bool returningToPatrol = false;
    private bool isPatrolling = true;

    private Renderer rend;
    private AudioSource audioSource;
    private NavMeshAgent agent;
    public int health = 2;

    public void TakeDamage(int amount)
    {
        health -= amount;
        Debug.Log($"{gameObject.name} took {amount} damage. Health: {health}");

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log($"{gameObject.name} died!");

        if (agent != null)
            agent.isStopped = true;

        SetPassiveVisual();

        if (deathSound != null)
            audioSource.PlayOneShot(deathSound);

        if (aggroParticles != null)
            aggroParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

        StartCoroutine(DisappearAnimation());
    }

    IEnumerator DisappearAnimation()
    {
        float duration = 0.4f;
        float time = 0f;
        Vector3 originalScale = transform.localScale;

        while (time < duration)
        {
            transform.localScale = Vector3.Lerp(originalScale, Vector3.zero, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        transform.localScale = Vector3.zero;
        Destroy(gameObject);
    }

    void Start()
    {
        rend = GetComponentInChildren<Renderer>();
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        agent = GetComponent<NavMeshAgent>();
        agent.speed = patrolSpeed;
        SetPassiveVisual();

        isPatrolling = true;
        GoToNextPatrolPoint();
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (!isChasing && distanceToPlayer < detectionRadius)
        {
            isChasing = true;
            isPatrolling = false;
            returningToPatrol = false;
            agent.speed = chaseSpeed;
            SetAggressiveVisual();
        }

        if (isChasing)
        {
            agent.SetDestination(player.position);

            if (distanceToPlayer <= attackRange && Time.time - lastAttackTime > attackCooldown)
            {
                Attack();
                lastAttackTime = Time.time;
            }

            if (distanceToPlayer > loseTargetDistance)
            {
                isChasing = false;
                returningToPatrol = true;
                agent.speed = patrolSpeed;
                SetPassiveVisual();
            }
        }
        else if (returningToPatrol)
        {
            if (!agent.pathPending && agent.remainingDistance < 0.5f)
            {
                returningToPatrol = false;
                isPatrolling = true;
                GoToNextPatrolPoint();
            }
        }
        else if (isPatrolling)
        {
            if (!agent.pathPending && agent.remainingDistance < 0.5f)
            {
                GoToNextPatrolPoint();
            }
        }
    }

    void GoToNextPatrolPoint()
    {
        if (patrolPoints.Length == 0) return;

        Transform targetPoint = patrolPoints[currentPatrolIndex];
        if (targetPoint == null) return;

        agent.destination = targetPoint.position;

        if (patrolForward)
        {
            currentPatrolIndex++;
            if (currentPatrolIndex >= patrolPoints.Length)
            {
                currentPatrolIndex = patrolPoints.Length - 2;
                patrolForward = false;
            }
        }
        else
        {
            currentPatrolIndex--;
            if (currentPatrolIndex < 0)
            {
                currentPatrolIndex = 1;
                patrolForward = true;
            }
        }
    }

    void Attack()
    {
        Debug.Log($"{gameObject.name} attacks the player!");

        if (attackSound != null)
            audioSource.PlayOneShot(attackSound);

        PlayerController pc = player.GetComponent<PlayerController>();
        if (pc != null)
        {
            pc.TakeDamage(1);
        }

        Rigidbody playerRb = player.GetComponent<Rigidbody>();
        if (playerRb != null)
        {
            Vector3 pushDir = (player.position - transform.position).normalized;
            playerRb.AddForce(pushDir * pushForce, ForceMode.Impulse);
        }
    }

    void SetPassiveVisual()
    {
        if (rend != null)
            rend.material.color = Color.black;

        if (aggroParticles != null)
            aggroParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
    }

    void SetAggressiveVisual()
    {
        if (rend != null)
            rend.material.color = Color.red;

        if (aggroParticles != null && !aggroParticles.isPlaying)
        {
            aggroParticles.Clear();
            aggroParticles.Play();
        }

        if (aggroSound != null)
            audioSource.PlayOneShot(aggroSound);
    }
}
