using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopUI : MonoBehaviour
{
    public static ShopUI Instance { get; private set; }

    [Header("UI")]
    [SerializeField] private GameObject panel;
    [SerializeField] private TMP_Text coinsText;

    [Header("Costs & Effects")]
    [SerializeField] private int fireRateCost = 5;
    [SerializeField] private int moveSpeedCost = 5;
    [SerializeField] private int healCost     = 3;
    [SerializeField] private float fireRateMultiplier = 1.15f;
    [SerializeField] private float moveSpeedMultiplier = 1.15f; 

    [Header("Targets")]
    [SerializeField] private PlayerStats  playerStats;
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private WaveSpawner  spawner;

    [SerializeField] private Button fireRateButton;
    [SerializeField] private Button moveSpeedButton;
    [SerializeField] private Button healButton;
    // a check to see if shop panel is open
    public bool IsOpen => panel != null && panel.activeSelf;

    AudioManager audioManager;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        if (panel)
        {
            // disable shop panel at the start
            panel.SetActive(false);
        }
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    // open shop and pause the game
    public void Open()
    {
        EnsureRefs();
        audioManager.PlaySFX(audioManager.enterShop);
        if (panel == null) return;
        panel.SetActive(true);
        Time.timeScale = 0f;
        Refresh();
    }

    // close the shop, unpause game, notifies spawner it can continue
    public void ContinueGame()
    {
        if (panel == null) return;
        panel.SetActive(false);
        Time.timeScale = 1f;
        if (spawner != null) spawner.CloseShopAndContinue();
    }

    // try to buy fire rate upgrade
    public void BuyFireRate()
    {
        EnsureRefs();
        audioManager.PlaySFX(audioManager.upgrade);
        // spend coin
        if (ScoreManager.Instance.TrySpend(fireRateCost))
        {
            // apply upgrade multiplier
            playerStats.UpgradeFireRate(fireRateMultiplier);
            Refresh();
        }
    }

    //try to buy move speed upgrade
    public void BuyMoveSpeed()
    {
        EnsureRefs();
        audioManager.PlaySFX(audioManager.upgrade);
        // spend coin
        if (ScoreManager.Instance.TrySpend(moveSpeedCost))
        {
            // apply upgrade multiplier
            playerStats.UpgradeMoveSpeed(moveSpeedMultiplier);
            Refresh();
        }
    }

    // buy hp
    public void BuyHeal()
    {
        EnsureRefs();
        audioManager.PlaySFX(audioManager.upgrade);
        // spend coins
        if (ScoreManager.Instance.TrySpend(healCost))
        {
            // apply the heal
            playerHealth.Heal(1);
            Refresh();
        }
    }
    // enable/disable buttons depending on coin count
    public void Refresh()
    {
        EnsureRefs();
        int coins = ScoreManager.Instance.Coins;
        if (coinsText) coinsText.text = $"Coins: {coins}";

        if (fireRateButton)
            fireRateButton.interactable = coins >= fireRateCost;
        if (moveSpeedButton)
            moveSpeedButton.interactable = coins >= moveSpeedCost;
        if (healButton)
            healButton.interactable = coins >= healCost && playerHealth.CurrentHealth < playerStats.MaxHealth;
    }
    // a helper to check if player stats were properly assigned
    void EnsureRefs()
    {
        if (!playerHealth)
        {
            var ph = FindObjectOfType<PlayerHealth>();
            if (ph) { playerHealth = ph; playerStats = ph.GetComponent<PlayerStats>(); }
        }
        if (!playerStats)
        {
            var ps = FindObjectOfType<PlayerStats>();
            if (ps) playerStats = ps;
        }
        if (!spawner)
        {
            spawner = FindObjectOfType<WaveSpawner>();
        }
    }
}
