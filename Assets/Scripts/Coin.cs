using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    AudioManager audioManager;
    [SerializeField] int value = 1;
    void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // when player collides with this object, add to score
        if(collision.CompareTag("Player"))
        {
            audioManager.PlaySFX(audioManager.collectCoin);
            ScoreManager.Instance.AddCoins(value); // update coin ui 
            Destroy(gameObject);
        }
    }
}
