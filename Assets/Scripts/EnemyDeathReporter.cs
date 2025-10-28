using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// used to notify wavespawner and score manager 
public class EnemyDeathReporter : MonoBehaviour
{
    // used to track if any enemies are still alive in the wave
    private WaveSpawner owner;
    [SerializeField] private int points = 100; // gain points for killing an emey
    private bool notified = false;
    private bool reportingEnabled = true;
    public void DisableReporting()
    {
        reportingEnabled = false;
    }
    // called in spawner to take ownership of this enemy after it is instantiated
    public void SetOwner(WaveSpawner spawner)
    {
        owner = spawner;
    }

    private void NotifyOnce()
    {
        if (notified || !reportingEnabled)
        {
            return;
        }
        notified = true;

        // award points to player for killing this enemy
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.AddScore(points);
        }

        // notify spawner that one enemy died 
        if (owner != null)
        {
            owner.NotifyEnemyDied();
        }
    }

    private void OnDisable()
    {
        NotifyOnce();
    }

    private void OnDestroy()
    {
        NotifyOnce();
    }

}
