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
        Enemy_Controller[] enemies = transform.GetComponentsInParent<Enemy_Controller>();

        foreach (Enemy_Controller enemy in enemies)
        {
            enemy.DamagePlayer();
        }

    }
}
