using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_Controller : MonoBehaviour
{
   // public Transform Player;
    NavMeshAgent Agent;
    Animator animator;
    CharacterStats characterStats;


    public float AttackRange = 5f;
    bool CanAttack = true;
    float AttackCooldown = 2f;
    private bool isDead = false;


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
        float Distance=Vector3.Distance(System_Manager.system.Player.position,transform.position);
        if (Distance <= AttackRange)
        {
           // Agent.destination = System_Manager.system.Player.position;
            Agent.SetDestination(System_Manager.system.Player.position);
            if(Distance<Agent.stoppingDistance)
            {
                // FaceTarget();
                if (CanAttack)
                {
                    StartCoroutine(AttackCoolDown());
                    animator.SetTrigger("Attack");
                    // StartCoroutine(Attack());
                }
            }
        }

    }
    void Die()
    {
        isDead = true;
        animator.SetBool("IsDead", true);
        Agent.isStopped = true;
        enabled = false;

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
            if(characterStats.CurrentHealth<=0)
            {
                animator.SetBool("IsDead", true);
                //Agent.isStopped = true;
                enabled = false; // stops Update()
            }
        }
    }

    public void DamagePlayer()
    {

        System_Manager.system.Player.GetComponent<CharacterStats>().changeHealth(-characterStats.power);
    }
}
