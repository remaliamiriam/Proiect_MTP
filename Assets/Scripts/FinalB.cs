using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class FinalB : MonoBehaviour
{
    public Transform player;
    public float detectionRadius = 10f;
    public float attackRange = 2f;
    public float attackCooldown = 1.5f;
    public float pushForce = 8f;
    public float loseTargetDistance = 15f;

    public ParticleSystem aggroParticles;
    public ParticleSystem deathEffect;
    public AudioClip finalBSound;
    public AudioClip attackSound;
    public AudioClip deathSound;

    public int maxHealth = 10;
    private int currentHealth;

    private bool isChasing = false;
    private float lastAttackTime = -999f;

    private NavMeshAgent agent;
    private Renderer rend;
    private AudioSource audioSource;

    void Start()
    {
        currentHealth = maxHealth;

        rend = GetComponentInChildren<Renderer>();
        agent = GetComponent<NavMeshAgent>();

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        SetPassiveVisual();
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (!isChasing && distanceToPlayer < detectionRadius)
        {
            isChasing = true;
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
                SetPassiveVisual();
                agent.ResetPath();
            }
        }
    }

    void Attack()
    {
        if (attackSound != null)
            audioSource.PlayOneShot(attackSound);

        // Damage player
        PlayerController pc = player.GetComponent<PlayerController>();
        if (pc != null)
            pc.TakeDamage(2); // Boss lovește mai tare

        Rigidbody playerRb = player.GetComponent<Rigidbody>();
        if (playerRb != null)
        {
            Vector3 pushDir = (player.position - transform.position).normalized;
            playerRb.AddForce(pushDir * pushForce, ForceMode.Impulse);
        }
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        Debug.Log($"Final Boss took {amount} damage. Health: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("🔥 FINAL BOSS DEFEATED!");

        if (deathEffect != null)
            Instantiate(deathEffect, transform.position, Quaternion.identity);

        if (deathSound != null)
            audioSource.PlayOneShot(deathSound);

        Destroy(gameObject, 0.5f);
    }

    void SetPassiveVisual()
    {
        if (rend != null)
            rend.material.color = Color.magenta;

        if (aggroParticles != null)
            aggroParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
    }

    void SetAggressiveVisual()
    {
        if (rend != null)
            rend.material.color = Color.yellow;

        if (aggroParticles != null && !aggroParticles.isPlaying)
        {
            aggroParticles.Play();
        }

        if (finalBSound != null)
            audioSource.PlayOneShot(finalBSound);
    }
}
