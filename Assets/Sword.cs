using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    public int damageAmount = 10; // Damage dealt by the sword
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        // Check if the collided object has a DummyDamageHandler script
        DummyDamageHandler damageHandler = other.GetComponent<DummyDamageHandler>();

        if (damageHandler != null)
        {
            // Apply damage to the dummy
            damageHandler.TakeDamage(damageAmount);
            Debug.Log($"Sword hit {other.name}, dealt {damageAmount} damage.");
        }
    }
}
