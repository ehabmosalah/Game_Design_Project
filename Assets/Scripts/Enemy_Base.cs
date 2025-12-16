using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy_Base : MonoBehaviour
{
    public abstract void DamagePlayer();
    public abstract CharacterStats GetCharacterStats();
}
