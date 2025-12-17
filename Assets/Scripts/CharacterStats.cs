using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterStats : MonoBehaviour
{
    [SerializeField] 
     float maxHealth = 100f;
    public  float power = 10f;
     int killScore = 200;
    [SerializeField] Health_Bar health_Bar;

    public float CurrentHealth { get; private set; }
    public bool IsDead => CurrentHealth <= 0f;

    void Awake()
    {
        CurrentHealth = maxHealth;

        // Find health bar in Awake (executes before Start)
        if (health_Bar == null)
        {
            health_Bar = GetComponentInChildren<Health_Bar>(true);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // Now health_Bar should be found
        if (health_Bar != null)
        {
            health_Bar.UpdateHealthBar(maxHealth, CurrentHealth);
        }
    }
   
    // Update is called once per frame
    void Update()
    {
        
    }
    public void changeHealth(float value)
    {
        CurrentHealth = Mathf.Clamp(CurrentHealth + value, 0, maxHealth);
        health_Bar.UpdateHealthBar(maxHealth, CurrentHealth);
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
            
            Destroy(gameObject);
            SceneManager.LoadScene("MainMenu");
        }
    }

}
