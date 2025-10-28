using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{

    public GameManager gameManager;
    [SerializeField] PlayerStats stats;
    public int CurrentHealth { get; private set; }
    private bool isDead;
    AudioManager audioManager;

    void Awake()
    {
        // access audio maanger and its components 
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        // check if a player has been instantiated
        if (!stats)
        {
            // get the stats of the player
            stats = GetComponent<PlayerStats>();
        }
        // define player's current health
        CurrentHealth = stats.MaxHealth;
    }

    public void TakeDamage(int amount)
    {
        // subtract enemy dmg from player's health
        CurrentHealth = Mathf.Max(0, CurrentHealth - amount);
        // play sound effect
        audioManager.PlaySFX(audioManager.playerHit);
        // check if player is about to die 
        if (CurrentHealth <= 0 && !isDead)
        {
            // set bool isDead to true 
            isDead = true;
            // pause everything 
            Time.timeScale = 0f;
            // call game over screen
            gameManager.GameOver();
            Die();
        } 
    }

    public void Heal(int amount)
    {
        // check if player is at max health, if not, add healed amount
        CurrentHealth = Mathf.Min(stats.MaxHealth, CurrentHealth + amount);
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
