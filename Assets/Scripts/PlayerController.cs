using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(AudioSource))]
public class PlayerController : MonoBehaviour
{
    [Header("Mișcare")]
    public float walkSpeed = 5f;
    public float runSpeed = 8f;
    public float jumpForce = 7f;
    public float rotationSpeed = 5f;
    public float turnAngle = 90f;

    [Header("Camera")]
    public Transform cameraTransform;

    [Header("Audio")]
    public AudioClip footstepSound;
    public AudioClip fireballSound;

    [Header("Health")]
    public int maxHealth = 4;
    private int currentHealth;

    [Header("Atac")]
    public Transform firePoint;
    public int fireballAmmo = 0;
    public float attackRange = 15f;
    public int attackDamage = 1;

    [Header("UI")]
    public Image healthFill;        
    public TMP_Text ammoText;         

    [Header("Game UI Manager")]
    public GYManager gyManager;       

    private Rigidbody rb;
    private AudioSource audioSource;
    private bool isGrounded = true;
    private int jumpCount = 0;
    private int maxJumps = 2;
    private bool isMoving = false;
    private Quaternion targetRotation;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = true;
        audioSource.playOnAwake = false;

        if (cameraTransform == null && Camera.main != null)
            cameraTransform = Camera.main.transform;

        targetRotation = transform.rotation;
        currentHealth = maxHealth;

        UpdateHealthUI();
        UpdateAmmoUI();
    }

    void Update()
    {
        if (Keyboard.current == null) return;

        float vertical = 0f;

        if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed)
            vertical = 1f;

        Vector3 moveDirection = transform.forward * vertical;
        isMoving = moveDirection.magnitude > 0f;

        float speed = Keyboard.current.leftShiftKey.isPressed ? runSpeed : walkSpeed;
        Vector3 velocity = moveDirection * speed;
        rb.linearVelocity = new Vector3(velocity.x, rb.linearVelocity.y, velocity.z);

        if (Keyboard.current.aKey.wasPressedThisFrame || Keyboard.current.leftArrowKey.wasPressedThisFrame)
            targetRotation *= Quaternion.Euler(0f, -turnAngle, 0f);

        if (Keyboard.current.dKey.wasPressedThisFrame || Keyboard.current.rightArrowKey.wasPressedThisFrame)
            targetRotation *= Quaternion.Euler(0f, turnAngle, 0f);

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        if (Keyboard.current.spaceKey.wasPressedThisFrame && jumpCount < maxJumps)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
            jumpCount++;
        }

        if (isGrounded && isMoving)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.clip = footstepSound;
                audioSource.Play();
            }
        }
        else if (audioSource.isPlaying && audioSource.clip == footstepSound)
        {
            audioSource.Stop();
        }

        if (Keyboard.current.enterKey.wasPressedThisFrame || Keyboard.current.numpadEnterKey.wasPressedThisFrame)
        {
            if (fireballAmmo > 0)
            {
                InstantAttack();
                fireballAmmo--;
                UpdateAmmoUI();

               
                CheckIfWon();
            }
            else
            {
                Debug.LogWarning("No ammo available to attack.");
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            isGrounded = true;
            jumpCount = 0;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            isGrounded = false;
        }
    }

    void InstantAttack()
    {
        EnemyAI closestEnemy = null;
        float minDist = attackRange + 1f;

        foreach (var enemy in Object.FindObjectsOfType<EnemyAI>())
        {
            float dist = Vector3.Distance(firePoint.position, enemy.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                closestEnemy = enemy;
            }
        }

        if (closestEnemy != null)
        {
            Vector3 direction = (closestEnemy.transform.position - firePoint.position).normalized;
            firePoint.rotation = Quaternion.LookRotation(direction);

            Debug.Log($"[InstantAttack] FirePoint se uită spre {closestEnemy.name} la distanța {minDist}");

            Debug.DrawRay(firePoint.position, firePoint.forward * attackRange, Color.green, 2f);

            closestEnemy.TakeDamage(attackDamage);
            Debug.Log($"[InstantAttack] Enemy {closestEnemy.name} lovit și damage aplicat.");

            if (fireballSound != null)
                audioSource.PlayOneShot(fireballSound);
        }
        else
        {
            Debug.LogWarning("[InstantAttack] Nu a fost găsit niciun inamic în raza de atac.");
        }
    }

    EnemyAI FindEnemyAI(Collider collider)
    {
        EnemyAI enemy = collider.GetComponent<EnemyAI>();
        if (enemy != null)
            return enemy;

        enemy = collider.GetComponentInParent<EnemyAI>();
        if (enemy != null)
            return enemy;

        enemy = collider.GetComponentInChildren<EnemyAI>();
        if (enemy != null)
            return enemy;

        return null;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        Debug.Log("Player Health: " + currentHealth);

        UpdateHealthUI();

        if (currentHealth <= 0)
        {
            if (gyManager != null)
            {
                gyManager.ShowGameOver();
            }
            Destroy(gameObject);
        }
    }

    public void CollectToken()
    {
        fireballAmmo += 3;
        Debug.Log("Fireball Ammo: " + fireballAmmo);
        UpdateAmmoUI();
    }

    private void UpdateHealthUI()
    {
        if (healthFill != null)
            healthFill.fillAmount = (float)currentHealth / maxHealth;
    }

    private void UpdateAmmoUI()
    {
        if (ammoText != null)
            ammoText.text = "Ammo: " + fireballAmmo;
    }

    private void CheckIfWon()
    {
        EnemyAI[] enemies = Object.FindObjectsOfType<EnemyAI>();
        if (enemies.Length == 0)
        {
            if (gyManager != null)
            {
                gyManager.ShowYouWon();
                enabled = false;
            }
        }
    }
}
