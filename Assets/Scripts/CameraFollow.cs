using UnityEngine;
public class CameraFollow : MonoBehaviour
{
    // camera stays on top of player that spawns on these coords
    [SerializeField] private Vector3 offset = new Vector3(-1.67f, 3.6f, 0f);
    // controls smoothness of follow
    [SerializeField] private float damping = 0.15f;
    public Transform target;
    private Vector3 vel = Vector3.zero;

    // called for when player respawns, re-assign the target to it 
    public void SetTarget(Transform transform)
    {
        target = transform;
    }

    private void FixedUpdate()
    {
        if (target == null)
        {
            return;
        }
        
        // follow the player's movements 
        Vector3 targetPos = target.position + offset;
        targetPos.z = transform.position.z;

        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref vel, damping);
    }
}
