using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Source")]
    [SerializeField] AudioSource SFXSource;
    [Header("Audio Clip")]
    public AudioClip gunShot;
    public AudioClip playerHit;
    public AudioClip collectCoin;
    public AudioClip enemyHit;
    public AudioClip enemyDeath;
    public AudioClip enterShop;
    public AudioClip upgrade;
    public AudioClip blockPlace;
    public AudioClip blockDeny;

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }

}
