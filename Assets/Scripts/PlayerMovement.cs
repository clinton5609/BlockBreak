using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private PlayerStats stats;

    private Rigidbody2D rb;
    private Vector2 moveInput;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (!stats)
        {
            stats = GetComponent<PlayerStats>();
        }
    }

    void Update()
    {
        // read inputs 
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");
        // prevents faster diagonal movement
        moveInput = moveInput.normalized;
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveInput * stats.MoveSpeed * Time.fixedDeltaTime);
    }
}
