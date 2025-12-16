using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animation_Events : MonoBehaviour
{
    public Player_Movement playerMovement;
    public void PlayerAttack()
    {
        Debug.Log("Player Attack Hit Event Triggered");
        playerMovement.PlayerAttack();
    }
    public void PlayerDamage()
    {
        Enemy_Base enemy = GetComponent<Enemy_Base>();
        if (enemy == null)
            enemy = GetComponentInParent<Enemy_Base>();

        if (enemy != null)
        {
            enemy.DamagePlayer();
        }
        else
        {
            Debug.LogError("No Enemy_Base found on " + gameObject.name);
        }

    }


    ///------------------------------ Sound Events ------------------------------///
    public void MoveSound()
    {
        System_Manager.system.PlaySound(System_Manager.system.Sounds[0], System_Manager.system.Player.position);
    }
    public void SwordSound_1()
    {
        System_Manager.system.PlaySound(System_Manager.system.Sounds[2], System_Manager.system.Player.position);
    }
    public void SwordSound_2()
    {
        System_Manager.system.PlaySound(System_Manager.system.Sounds[3], System_Manager.system.Player.position);
    }
    public void SwordSound_3()
    {
        System_Manager.system.PlaySound(System_Manager.system.Sounds[4], System_Manager.system.Player.position);
    }
    public void JumpSound()
    {
        System_Manager.system.PlaySound(System_Manager.system.Sounds[5], System_Manager.system.Player.position);
    }
    public void Enemy_MoveSound()
    {

        Vector3 enemyPosition = transform.position;
        System_Manager.system.PlaySound(System_Manager.system.Sounds[6], enemyPosition);
    }
}
