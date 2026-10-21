using UnityEngine;

public class FollowTracker : MonoBehaviour
{
    public Transform tracker;
    public float speed = 18f;
    public float turnSpeed = 4f;
    public float followDistance = 2.5f;

    void Update()
    {
        if (tracker == null) return;

        Vector3 direction = tracker.position - transform.position;
        direction.y = 0f;

        if (direction.magnitude <= followDistance) return;

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            Quaternion.LookRotation(direction),
            turnSpeed * Time.deltaTime
        );

        transform.position += transform.forward * speed * Time.deltaTime;
    }
}