using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float lifeSeconds = 2f;
    [SerializeField] private float damage = 1f;

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, lifeSeconds);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.velocity = (Vector2) transform.right * speed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        //bullet hit enemy, enemy take damage
        if (other.gameObject.TryGetComponent<Enemy>(out Enemy enemyComponent))
        {
            enemyComponent.TakeDamage(damage);
        }
        Destroy(gameObject);
        
    }
}
