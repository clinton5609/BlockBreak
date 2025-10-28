using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public GameObject player;
    public float moveSpeed;
    [SerializeField] float health;
    [SerializeField] float maxHealth = 5f;
    [Header("Loot")] // a list of possible loot from this enemy
    public List<LootItem> lootTable = new List<LootItem>();

    Transform playerT; // for pos tracking
    AudioManager audioManager;
    NavMeshAgent agent;
    void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>(); // find Audio manager using the tag
    }

    // Start is called before the first frame update
    void Start()
    {
        // for nav mesh
        agent = GetComponent<NavMeshAgent>(); 
        agent.updateRotation = false; 
        agent.updateUpAxis = false;

        health = maxHealth;
        // find target/player if not already defined
        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null)
            {
                player = p;
                playerT = p.transform;
            }
        }else
        {
            playerT = player.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // check for player after they respawn (cliked on retry in game over screen)
        if (playerT == null)
        {
            // try to find a player with tag "Player" in hierarchy
            var p = GameObject.FindGameObjectWithTag("Player");
            if (p != null)
            {
                player = p;
                playerT = p.transform;
            }
            else
            {
                return;
            }

        }
        // rotate to follow player
        Vector2 dir = ((Vector2)playerT.position - (Vector2)transform.position).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
        // move with navmesh
        agent.SetDestination(playerT.position); 
        
    }

    public void TakeDamage(float amount)
    {
        // take damage
        health -= amount;
        audioManager.PlaySFX(audioManager.enemyHit);
        // when health reaches 0, player dies
        if (health <= 0)
        {
            audioManager.PlaySFX(audioManager.enemyDeath);
            Die();
        }
    }
    void Die()
    {
        // check if there is loot in loot table 
        if (lootTable.Count > 0)
        {
            // track total chance of all the loot in the table
            float total = 0f;
            foreach (var item in lootTable)
            {
                total += Mathf.Max(0f, item.dropChance);
            }

            // roll for a single item
            if (total > 0f)
            {
                float r = Random.Range(0f, total);
                foreach (var item in lootTable)
                {
                    r -= Mathf.Max(0f, item.dropChance);
                    if (r <= 0f)
                    {
                        // spawn the loot at enemy's pos
                        if (item.itemPrefab)
                        {
                            InstantiateLoot(item.itemPrefab);
                        }
                        break;
                    }
                }
            }
        }
        //destroy this object (enemy)
    Destroy(gameObject);
    }
    void InstantiateLoot(GameObject loot)
    {
        if(loot)
        {
            // instantiate loot 
            GameObject droppedLoot = Instantiate(loot, transform.position, Quaternion.identity);
        }
    }
}
