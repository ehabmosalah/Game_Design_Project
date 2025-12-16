using BigRookGames.Weapons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Boss_Controller : Enemy_Base
{
    NavMeshAgent Agent;
    Animator animator;
    CharacterStats characterStats;

    [Header("Gun Settings")]
    public GunfireController gun; // Reference to the gun component

    [Header("Combat")]
    public float AttackRange = 5f;
    public float AttackCooldown = 2f;

    bool CanAttack = true;
    bool isDead = false;

    // Start is called before the first frame update
    void Start()
    {
        Agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        characterStats = GetComponent<CharacterStats>();

        // Initialize gun if assigned
        if (gun != null)
        {
            gun.player = System_Manager.system.Player;
            gun.enemyIsAttacking = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (characterStats.IsDead)
        {
            if (!isDead) // Check if we already triggered death
            {
                Die();
            }
            return; // Exit Update early
        }

        // Normalize the speed value between 0 and 1
        float normalizedSpeed = Mathf.Clamp01(Agent.velocity.magnitude / Agent.speed);
        animator.SetFloat("Speed", normalizedSpeed);

        float Distance = Vector3.Distance(System_Manager.system.Player.position, transform.position);

        if (Distance <= AttackRange)
        {
            Agent.SetDestination(System_Manager.system.Player.position);

            if (Distance < Agent.stoppingDistance)
            {
                // Stop the agent when within stopping distance
                Agent.isStopped = true;

                if (CanAttack)
                {
                    StartCoroutine(AttackCoolDown());
                    animator.SetTrigger("Attack");
                }
            }
            else
            {
                // Resume movement if player moves away
                Agent.isStopped = false;
            }
        }
        else
        {
            // Ensure agent is stopped when out of range
            Agent.isStopped = false;
        }
    }

    void Die()
    {
        isDead = true;
        animator.SetBool("IsDead", true);
        Agent.isStopped = true;

        // Stop gun from firing when dead
        if (gun != null)
        {
            gun.enemyIsAttacking = false;
        }

        enabled = false;
    }

    IEnumerator AttackCoolDown()
    {
        CanAttack = false;

        // Start gun firing
        if (gun != null)
        {
            gun.enemyIsAttacking = true;
        }

        yield return new WaitForSeconds(AttackCooldown);

        // Stop gun firing after cooldown
        if (gun != null)
        {
            gun.enemyIsAttacking = false;
        }

        CanAttack = true;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            Debug.Log("Enemy hit the player!!!!!!!!!!!!");
            Instantiate(System_Manager.system.Particles[0], collider.transform.position, collider.transform.rotation);
            characterStats.changeHealth(-collider.GetComponentInParent<CharacterStats>().power);
            if (characterStats.CurrentHealth <= 0)
            {
                Instantiate(System_Manager.system.Particles[3], collider.transform.position, collider.transform.rotation);
                animator.SetBool("IsDead", true);
                enabled = false; // stops Update()
            }
        }
    }

    public override void DamagePlayer()
    {
        System_Manager.system.Player.GetComponent<CharacterStats>().changeHealth(-characterStats.power);
    }
    public override CharacterStats GetCharacterStats()
    {
        return characterStats;
    }
}