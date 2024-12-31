using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HealthText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private int maxHealth;
    [SerializeField] private int currentHealth;

    public int CurrentHealth { get => currentHealth; set => currentHealth = value; }

    private void Update()
    {
        if (currentHealth <= 0)
        {
            healthText.enabled = false;
        }
        healthText.SetText($"{currentHealth}");
    }
    public void OnInit(int maxHealth)
    {
        this.maxHealth = maxHealth;
        currentHealth = maxHealth;
        healthText.SetText($"{currentHealth}");
    }
}
