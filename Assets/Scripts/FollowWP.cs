using UnityEngine;

public class FollowWP : MonoBehaviour
{
    public Transform[] waypoints;
    public float speed = 5f;
    public float turnSpeed = 4f;
    public float reachDistance = 2f;

    private int currentIndex = 0;

    void Update()
    {
        if (waypoints == null || waypoints.Length == 0)
            return;

        Transform target = waypoints[currentIndex];
        if (target == null)
            return;

        Vector3 direction = target.position - transform.position;
        direction.y = 0f;

        if (direction.magnitude <= reachDistance)
        {
            currentIndex = (currentIndex + 1) % waypoints.Length;
            return;
        }

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            Quaternion.LookRotation(direction),
            turnSpeed * Time.deltaTime
        );

        transform.position += transform.forward * speed * Time.deltaTime;
    }
}