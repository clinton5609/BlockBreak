using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedBlock : MonoBehaviour
{
    public float lifeTime = 3f;
    public float fadeDelay = 2f;

    private float remainingLife;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private BlockDispenser owner;

    // this dispenser owns this block, allows notification of when block dies
    public void SetOwner(BlockDispenser dispenser)
    {
        owner = dispenser;
    }

    void Start()
    {
        // fade the block as it gets closer to life span
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
            originalColor = spriteRenderer.color;

        remainingLife = lifeTime;
    }
    void Update()
    {
        // continuously update life of block
        remainingLife -= Time.deltaTime;

        // fade
        if (spriteRenderer && remainingLife <= fadeDelay)
        {
            float alpha = Mathf.Clamp01(remainingLife / fadeDelay);
            spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
        }
        // destroy block 
        if (remainingLife <= 0f)
            Destroy(gameObject);
    }

    void OnDestroy()
    {
        if(owner != null)
        {
            owner.NotifyBlockGone();
        }
    }
}
