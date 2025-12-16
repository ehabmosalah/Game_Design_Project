using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [SerializeField] 
     float maxHealth = 100f;
    public  float power = 10f;
     int killScore = 200;

    public float CurrentHealth { get; private set; }
    public bool IsDead => CurrentHealth <= 0f;


    // Start is called before the first frame update
    void Start()
    {
        CurrentHealth = maxHealth;

    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void changeHealth(float value)
    {
        CurrentHealth += value;
        Debug.Log("Health changed by " + value + ". Current health: " + CurrentHealth);
        if(CurrentHealth <= 0)
        {
            Die();
        }
    }
    void Die()
    {
       
        if (transform.CompareTag("Enemy"))
        {
            System_Manager.system.score += killScore;
            Destroy(gameObject);
            Debug.Log("Score increased by " + killScore + ". Current score: " + System_Manager.system.score);
        }
        else if (transform.CompareTag("Player"))
        {
            Debug.Log("Player has died!");
            // Implement player death logic here (e.g., respawn, game over screen, etc.)
        }
    }

}
