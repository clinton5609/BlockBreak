using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    public PlayerHealth playerHealth;
    public int damage = 1;
    public float damageInterval = 1f;

    // to keep track of time since player last took dmg 
    private readonly Dictionary<PlayerHealth, float> playerDmgCD = new();

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // if enemy collides with an object tagged "Player"
        if (collision.gameObject.CompareTag("Player"))
        {
            // get the player's health
            var player = collision.gameObject.GetComponent<PlayerHealth>();
            // check if it exists
            if (player != null)
            {
                // player take damage on first contact
                player.TakeDamage(damage);
                // dmg interval to handle when enemy stays in contact with player
                playerDmgCD[player] = damageInterval;
            }
        }
    }
    // make sure the enemy continuously damages player while colliders are colliding
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            var player = collision.gameObject.GetComponent<PlayerHealth>();

            if (player == null) return;

            if (!playerDmgCD.ContainsKey(player))
            {
                playerDmgCD[player] = 0f;
            }

            // count down dmg cooldown until player can be damaged again
            playerDmgCD[player] -= Time.deltaTime;

            if(playerDmgCD[player] <= 0f)
            {
                player.TakeDamage(damage);
                // reset cooldown
                playerDmgCD[player] = damageInterval;
            }
        }
    }
}
