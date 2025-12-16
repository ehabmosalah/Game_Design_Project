using BigRookGames.Weapons;
using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;


public class Boss_Cpmtroller : MonoBehaviour
{
    NavMeshAgent agent;
    Animator animator;
    CharacterStats characterStats;

    [Header("Combat")]
    public float attackRange = 15f;
    public float attackCooldown = 1.2f;

    [Header("Gun")]
    public GunfireController gun;

    bool canAttack = true;
    bool isDead = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        characterStats = GetComponent<CharacterStats>();

        // Assign player to gun automatically
        gun.player = System_Manager.system.Player;
    }

    void Update()
    {
        if (characterStats.IsDead)
        {
            if (!isDead)
                Die();
            return;
        }

        float distance = Vector3.Distance(System_Manager.system.Player.position, transform.position);

        // Move toward player
        if (distance <= attackRange)
        {
            agent.SetDestination(System_Manager.system.Player.position);

            float speed = agent.velocity.magnitude / agent.speed;
            animator.SetFloat("Speed", speed);

            // Stop and shoot
            if (distance <= agent.stoppingDistance)
            {
                agent.isStopped = true;

                if (canAttack)
                {
                    StartCoroutine(AttackCooldown());
                    animator.SetTrigger("Attack");
                }
            }
            else
            {
                agent.isStopped = false;
            }
        }
    }

    void Die()
    {
        isDead = true;
        animator.SetBool("IsDead", true);
        agent.isStopped = true;
        gun.enemyIsAttacking = false;
        enabled = false;
    }

    IEnumerator AttackCooldown()
    {
        canAttack = false;

        gun.enemyIsAttacking = true;

        yield return new WaitForSeconds(attackCooldown);

        gun.enemyIsAttacking = false;
        canAttack = true;
    }
    public void DamagePlayer()
    {

        System_Manager.system.Player.GetComponent<CharacterStats>().changeHealth(-characterStats.power);
    }
}
