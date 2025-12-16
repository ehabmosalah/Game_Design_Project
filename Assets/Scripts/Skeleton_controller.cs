using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Skeleton_Controller : Enemy_Base
{
    // public Transform Player;
    NavMeshAgent Agent;
    Animator animator;
    CharacterStats characterStats;


    public float AttackRange = 5f;
    bool CanAttack = true;
    float AttackCooldown = 2f;
    bool deathAnimationStarted = false;

    // Start is called before the first frame update
    void Start()
    {
        Agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        characterStats = GetComponent<CharacterStats>();

    }

    // Update is called once per frame
    void Update()
    {
        if (characterStats.IsDead && !deathAnimationStarted)
        {
            StartCoroutine(DieWithAnimation());
            deathAnimationStarted = true;
            return;
        }

        if (characterStats.IsDead) return;

        // Normalize the speed value between 0 and 1
        float normalizedSpeed = Mathf.Clamp01(Agent.velocity.magnitude / Agent.speed);

        animator.SetFloat("Speed", normalizedSpeed);
        float distance = Vector3.Distance(System_Manager.system.Player.position, transform.position);
        if (distance <= AttackRange && !characterStats.IsDead)
        {
            if (distance > Agent.stoppingDistance)
            {
                Agent.isStopped = false;
                Agent.SetDestination(System_Manager.system.Player.position);
            }
            else
            {
                Agent.isStopped = true;

                if (CanAttack)
                {
                    StartCoroutine(AttackCoolDown());
                    animator.SetTrigger("Attack");
                }
            }
        }
    }
    IEnumerator DieWithAnimation()
    {
        animator.SetTrigger("IsDead");
        Agent.isStopped = true;
        enabled = false;

        // Wait for death animation to finish (adjust time based on your animation)
        yield return new WaitForSeconds(30f); // Adjust this time

        Destroy(gameObject);
    }

    IEnumerator AttackCoolDown()
    {
        CanAttack = false;
        yield return new WaitForSeconds(AttackCooldown);
        CanAttack = true;
    }
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            Debug.Log("Enemy hit the player!!!!!!!!!!!!");
            characterStats.changeHealth(-collider.GetComponentInParent<CharacterStats>().power);
            //Destroy(gameObject);
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
