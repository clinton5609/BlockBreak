using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAim : MonoBehaviour
{

    private Rigidbody2D rb;
    [SerializeField] private float rotationOffset = 0f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        // find where mouse position is
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorld.z = 0f;

        // direction vector from player to mouse
        Vector2 direction = mouseWorld - transform.position;

        // rotate the player to face direction pointing towards mouse
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + rotationOffset;
        rb.MoveRotation(angle);
    }
}
