/*using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Fireball : MonoBehaviour
{
    public float speed = 20f;
    public float lifetime = 3f;
    public int damage = 1;
    public ParticleSystem impactEffect;
    public float turnSpeed = 5f;

    private bool hasHit = false;
    private Rigidbody rb;
    private Transform target;

    public void SetTarget(Transform enemyTransform)
    {
        target = enemyTransform;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Destroy(gameObject, lifetime);

        if (target == null)
        {
            Debug.LogWarning("Fireball has no target! Flying straight.");
            rb.linearVelocity = transform.forward * speed;
        }
        else
        {
            Debug.Log("Fireball launched with speed: " + speed + " toward " + target.name);
        }
    }

    void FixedUpdate()
    {
        if (hasHit || rb == null)
            return;

        if (target != null)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            rb.linearVelocity = direction * speed;

            Quaternion toRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, turnSpeed * Time.fixedDeltaTime);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (hasHit) return;

        hasHit = true;
        Debug.Log($"Fireball triggered with {other.name}");

        EnemyAI enemy = other.GetComponent<EnemyAI>();
        if (enemy == null)
            enemy = other.GetComponentInParent<EnemyAI>();
        if (enemy == null && other.transform.root != null)
            enemy = other.transform.root.GetComponent<EnemyAI>();

        if (enemy != null)
        {
            Debug.Log($"Fireball hit enemy: {enemy.name}, applying {damage} damage.");
            enemy.TakeDamage(damage);
        }
        else
        {
            Debug.Log($"{other.name} was hit, but has no EnemyAI script in parent/root.");
        }

        if (impactEffect != null)
        {
            ParticleSystem impact = Instantiate(impactEffect, transform.position, Quaternion.identity);
            Destroy(impact.gameObject, impact.main.duration + impact.main.startLifetime.constantMax);
        }

        if (TryGetComponent<MeshRenderer>(out var renderer))
            renderer.enabled = false;

        if (TryGetComponent<Collider>(out var col))
            col.enabled = false;

        rb.linearVelocity = Vector3.zero;
        rb.isKinematic = true;

        Destroy(gameObject, 0.1f);
    }

    public static Transform FindClosestEnemy(Vector3 origin, float maxDistance = 100f)
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        Transform closest = null;
        float minDist = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            float dist = Vector3.Distance(enemy.transform.position, origin);
            if (dist < minDist && dist <= maxDistance)
            {
                minDist = dist;
                closest = enemy.transform;
            }
        }

        return closest;
    }
}*/
