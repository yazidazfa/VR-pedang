using System.Collections;
using UnityEngine;

public class DummyDamageHandler : MonoBehaviour
{
    public int maxHealth = 100; // Dummy's maximum health
    private int currentHealth;
    private Animator animator;

    public GameObject dummyPrefab; // Reference to the dummy prefab
    private bool isRespawning = false;
    private bool isPushed = false; // Track if the dummy is being pushed
    public AudioSource hitAudioSource;
    void Start()
    {
        // Initialize health and cache the Animator
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();

        if (animator == null)
        {
            Debug.LogError("Animator component missing on dummy.");
        }
        hitAudioSource = GetComponent<AudioSource>();
        if (hitAudioSource == null)
        {
            Debug.LogError("AudioSource component missing on dummy.");
        }
        // Make sure the dummy starts in a neutral state (like Idle)
        // Do not trigger "Pushed" animation at the start
        animator.Play("Default");  // Make sure the dummy starts idle
    }

    public void TakeDamage(int damage)
    {
        if (currentHealth <= 0 || isPushed) return; // Skip if already dead or in pushed state

        currentHealth = Mathf.Max(0, currentHealth - damage);
        Debug.Log($"Dummy took {damage} damage. Current Health: {currentHealth}");

        // Play hit sound
        if (hitAudioSource && hitAudioSource.clip != null)
        {
            hitAudioSource.PlayOneShot(hitAudioSource.clip);
        }
        
        if (currentHealth > 0)
        {
            // Trigger the "Pushed" animation when damage is taken
            animator.SetTrigger("Pushed");
            StartCoroutine(PreventDamageWhilePushed()); // Start the coroutine to block damage while pushed
        }
        else
        {
            // Trigger the "Die" animation and handle respawn
            StartCoroutine(RespawnDummy());
        }
    }

    private IEnumerator PreventDamageWhilePushed()
    {
        isPushed = true;

        // Wait for the pushed animation to finish (adjust duration as needed)
        yield return new WaitForSeconds(1f); // This should match the duration of your "Pushed" animation

        // Allow damage to be taken again
        isPushed = false;
    }

    private IEnumerator RespawnDummy()
    {   
        if (isRespawning) yield break; // Avoid multiple respawn calls

        isRespawning = true;

        // Save the position and rotation of the current dummy
        Vector3 currentPosition = transform.position;
        Quaternion currentRotation = transform.rotation;
        Vector3 currentScale = transform.localScale;

        // Trigger die animation
        animator.SetTrigger("Pushed");
        animator.SetTrigger("Die");
        Debug.Log("Dummy is dying...");

        // Wait for 3 seconds (or adjust this based on the duration of your death animation)
        yield return new WaitForSeconds(3f); // Wait before destroying the dummy to allow animation to play

        // Instantiate a new dummy at the respawn point BEFORE destroying the current one
        if (dummyPrefab != null)    
        {
            // Instantiate the new dummy at the respawn location
            GameObject newDummy = Instantiate(dummyPrefab, currentPosition, currentRotation);
            newDummy.transform.localScale = currentScale;
            Debug.Log("Dummy respawned!");
            // Reset animator to a default state (e.g., "Idle")
            Animator newAnimator = newDummy.GetComponent<Animator>();
            if (newAnimator != null)
            {
                newAnimator.Play("Default"); // Replace with your default state, if needed
            }
            // Optionally, you can start the same process for the new dummy here if needed
            // For example, resetting health, triggering animations, etc.

            // Destroy the current dummy object AFTER instantiating the new one
            Destroy(gameObject);
        }
        else
        {
            Debug.LogError("Dummy prefab or respawn point not set.");
        }

        isRespawning = false;
    }
}
