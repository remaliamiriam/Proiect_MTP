using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(AudioSource))]
public class TokenCollectible : MonoBehaviour
{
    public AudioClip collectSound;
    public float destroyDelay = 0.5f;
    public float rotateSpeed = 60f;        
    public float floatAmplitude = 0.25f;   
    public float floatFrequency = 1f;      

    private bool collected = false;
    private AudioSource audioSource;
    private Vector3 startPos;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        startPos = transform.position;
    }

    void Update()
    {
        if (collected) return;

       
        transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime, Space.World);

       
        float newY = startPos.y + Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }

    void OnTriggerEnter(Collider other)
    {
        if (collected || !other.CompareTag("Player")) return;

        collected = true;

       
        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null)
        {
            Debug.Log("Player found! Calling CollectToken().");
            player.CollectToken();
        }
        else
        {
            Debug.LogWarning("PlayerController script not found on Player!");
        }

       
        if (collectSound != null)
            AudioSource.PlayClipAtPoint(collectSound, transform.position);

       
        StartCoroutine(CollectAndDestroy());
    }


    System.Collections.IEnumerator CollectAndDestroy()
    {
        float t = 0f;
        Vector3 startScale = transform.localScale;

        while (t < destroyDelay)
        {
            t += Time.deltaTime;
            transform.localScale = Vector3.Lerp(startScale, Vector3.zero, t / destroyDelay);
            yield return null;
        }

        Destroy(gameObject);
    }
}
