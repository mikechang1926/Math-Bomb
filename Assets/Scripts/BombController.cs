using UnityEngine;
using System.Collections;
public class BombController : MonoBehaviour
{
    // Growth Settings
    [SerializeField] private float growthSpeed = 1f; // Speed of the bomb's growth
    [SerializeField] private float maxSize = 3f; // Maximum size before the bomb explodes

    // Explosion Settings
    [SerializeField] private GameObject explosionEffect; // Particle effect prefab for the explosion
    [SerializeField] private float explosionForce = 500f; // Explosion force
    [SerializeField] private float explosionRadius = 5f; // Radius of the explosion
    private Renderer objectRenderer;
    private Color originalColor; // Stores the original color
    private bool isTemporarilyBlinking = false; // Flag to prevent overlapping coroutines

    [SerializeField] private Color redBlinkColor = Color.red; // Default blinking color (red)
    [SerializeField] private Color greenBlinkColor = Color.green; // Temporary blink color (green)
    [SerializeField] private float blinkDuration = 0.2f; // Time for each blink (on/off)
    [SerializeField] private int greenBlinkCount = 3; // Number of times to blink green

    private Coroutine currentBlinkCoroutine; // Store coroutine reference to control blinking

    // Internal Variables
    private AudioSource audioSource;
    private Vector3 originalScale;
    private bool hasExploded = false;
    private bool isTicking = false;
    public AudioSource backgroundMusic;
    private void Start()
    {
        // Save the original scale of the bomb
        originalScale = transform.localScale;

        // Initialize the AudioSource for the explosion sound
        audioSource = GetComponent<AudioSource>();
        objectRenderer = GetComponent<Renderer>();

        // Store the original color (to revert if needed)
        if (objectRenderer != null)
        {
            originalColor = objectRenderer.material.color;
            Debug.Log("Original color: " + originalColor);
        }
    }

    public void blinkGreen() {
        StartBlinkingGreen();
    }

    public void startTicking() {
        ResetBombSize();
        isTicking = true;
        backgroundMusic.Play();
        if (objectRenderer != null)
        {
            // Start blinking red indefinitely
            currentBlinkCoroutine = StartCoroutine(BlinkIndefinitely(redBlinkColor));
        }
    }
    
    private void Update()
    {
        if (GameManager.Instance.isPendingLevelUp)
        {
            isTicking = false;
        }

        // If the bomb has already exploded, do nothing
        if (hasExploded) return;

        if (isTicking) 
        {
            // Gradually increase the bomb's size
            transform.localScale += Vector3.one * growthSpeed * Time.deltaTime;

                    // Check if the bomb has reached its maximum size
            if (transform.localScale.x >= maxSize)
            {
                Explode();
            }
        }
        else 
        {
            // Stop the current red blinking
            if (currentBlinkCoroutine != null)
            {
                StopCoroutine(currentBlinkCoroutine);
            }
            backgroundMusic.Stop();
        }
    }

    public void ResetBombSize()
    {
        transform.localScale = originalScale; // Restore original size
    }

    public void StartBlinkingGreen()
    {
        transform.localScale -= Vector3.one * 20;
        if (!isTemporarilyBlinking)
        {
            // Stop the current red blinking
            if (currentBlinkCoroutine != null)
            {
                StopCoroutine(currentBlinkCoroutine);
            }

            // Start blinking green a few times, then return to red
            StartCoroutine(BlinkGreenThenRed());
        }
    }

    private IEnumerator BlinkIndefinitely(Color blinkColor)
    {
        while (true) // Infinite loop for continuous blinking
        {
            objectRenderer.material.color = blinkColor;
            yield return new WaitForSeconds(blinkDuration);

            objectRenderer.material.color = originalColor;
            yield return new WaitForSeconds(blinkDuration);
        }
    }

    private IEnumerator BlinkGreenThenRed()
    {
        isTemporarilyBlinking = true;

        // Blink green a set number of times
        for (int i = 0; i < greenBlinkCount; i++)
        {
            objectRenderer.material.color = greenBlinkColor;
            yield return new WaitForSeconds(blinkDuration);

            objectRenderer.material.color = originalColor;
            yield return new WaitForSeconds(blinkDuration);
        }

        isTemporarilyBlinking = false;

        if (isTicking) 
        {
            // Resume red blinking indefinitely
            currentBlinkCoroutine = StartCoroutine(BlinkIndefinitely(redBlinkColor));
        }
    }


    private void Explode()
    {
        hasExploded = true; // Prevent multiple explosions
        // Play explosion sound
        Debug.Log("Playing explosion sound");
        Debug.Log("AudioSource volume: " + audioSource.volume);
        audioSource.volume = 1f;
        audioSource.mute = false;
        audioSource.enabled = true;
        audioSource.Play();

        // Instantiate the explosion particle effect
        if (explosionEffect != null)
        {
            explosionEffect.SetActive(true);
        }

        // Apply explosion force to nearby objects
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider nearbyObject in colliders)
        {
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
            }
        }

        // Destroy the bomb after the explosion effect
        objectRenderer = GetComponent<Renderer>();
        objectRenderer.enabled = false;
        GameManager.Instance.GameOver();
    }
}
