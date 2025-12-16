using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class Health_Bar : MonoBehaviour
{
    [SerializeField] private Image healthFill;
    private Transform mainCamera;
    void Start()
    {
        mainCamera = Camera.main.transform;
    }

    public void UpdateHealthBar(float maxHealth,float currentHealth)
    {
        healthFill.fillAmount = currentHealth / maxHealth;
    }

    void Update()
    {
        transform.rotation = Quaternion.LookRotation(transform.position - mainCamera.position);
    }   
}
