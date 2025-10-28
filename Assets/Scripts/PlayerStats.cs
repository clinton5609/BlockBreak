using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Base Stats")]
    [SerializeField] public float baseMoveSpeed = 5f;
    [SerializeField] public float baseFireRate = 8f;
    [SerializeField] public int baseMaxHealth = 5;

    [Header("Multipliers/Bonuses")]
    [SerializeField] float moveSpeedMult = 1f;
    [SerializeField] float fireRateMult  = 1f;
    [SerializeField] int   bonusMaxHP    = 0;

    public float MoveSpeed => baseMoveSpeed * moveSpeedMult;
    public float FireRate  => baseFireRate  * fireRateMult;
    public int   MaxHealth => baseMaxHealth + bonusMaxHP;

    // multipliers for stats
    public void UpgradeMoveSpeed(float mult) => moveSpeedMult *= mult;
    public void UpgradeFireRate(float mult)  => fireRateMult *= mult;
    public void AddMaxHealth(int amount) => bonusMaxHP += amount;

    // used for when player dies, set stats back to base 
    public void ResetToBase()
    {
        moveSpeedMult = 1f;
        fireRateMult = 1f;
        bonusMaxHP = 0;
    }
    
    void Awake()
    {
        ResetToBase();
    }
}
