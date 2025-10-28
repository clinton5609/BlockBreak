using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BlockDispenser : MonoBehaviour
{
    public GameObject block; // block prefab to place
    public float placeDistance = 1f; // how far in front of player to place block 
    public LayerMask blockingMask;
    AudioManager audioManager;

    [SerializeField] private int maxBlocks = 3; // max blocks that can be placed at once 
    private int activeBlocks = 0; // track blocks already placed

    void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    void Update()
    {
        // when user presses space, try to place a block
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TryPlace();
        }
    }

    private void TryPlace()
    {
        // check if there is more/equal amount blocks on field than max allowed 
        if (activeBlocks >= maxBlocks)
        {
            audioManager.PlaySFX(audioManager.blockDeny);
            return;
        }

        // compute snapped position
        // adjusted to direction where player is facing
        Vector2 placePos = (Vector2)transform.position + (Vector2)transform.right * placeDistance;
        // stay on grid
        placePos = new Vector2(Mathf.Round(placePos.x), Mathf.Round(placePos.y));

        // check for collision or block prefab not assigned
        Collider2D hit = Physics2D.OverlapPoint(placePos, blockingMask);
        if (hit != null && block == null)
        {
            // can't place block 
            audioManager.PlaySFX(audioManager.blockDeny);
            return;
        }

        // spawn the block
        var newBlock = Instantiate(block, placePos, Quaternion.identity);
        // handle life span of block
        var timedBlock = newBlock.GetComponent<TimedBlock>();
        if (timedBlock == null)
        {
            timedBlock = newBlock.AddComponent<TimedBlock>();
        }

        // set this block dispenser as the owner of the block so it can be notified back when the block is destroyed
        timedBlock.SetOwner(this);
        activeBlocks++;
        audioManager.PlaySFX(audioManager.blockPlace);
    }

    public void NotifyBlockGone()
    {
        activeBlocks = Mathf.Max(0, activeBlocks - 1);
    }
}
