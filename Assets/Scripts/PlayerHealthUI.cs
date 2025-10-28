using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerHealthUI : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private TMP_Text healthText;

    void Awake()
    {
        // assign appropriate fields
        if (!playerHealth)
            playerHealth = FindObjectOfType<PlayerHealth>();
        if (!playerStats)
            playerStats = FindObjectOfType<PlayerStats>();
        if (!healthText)
            healthText = GetComponent<TMP_Text>();
    }

    void Update()
    {
        // stats are not assigned yet
        if (playerHealth == null)
        {
            // assign appropriate stats
            playerHealth = FindObjectOfType<PlayerHealth>();
            playerStats = FindObjectOfType<PlayerStats>();
        }
        if (healthText && playerHealth && playerStats)
        {
            healthText.text = $"Health: {playerHealth.CurrentHealth}/{playerStats.MaxHealth}";
        }
    }
    
    public void HideHealthText()
    {
        if(healthText != null)
        {
            healthText.text = "";
        }
    }
}
