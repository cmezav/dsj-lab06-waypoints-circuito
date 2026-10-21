using UnityEngine;

public class FollowWP : MonoBehaviour
{
    public Transform[] waypoints;
    public float speed = 5f;
    public float turnSpeed = 4f;
    public float reachDistance = 2f;
    public float laneOffset = 0f;

    private int currentIndex = 0;

    void Update()
    {
        if (waypoints == null || waypoints.Length == 0)
            return;

        Transform target = waypoints[currentIndex];
        if (target == null)
            return;

        Vector3 targetPosition = target.position;

        if (waypoints.Length > 1)
        {
            Transform next = waypoints[(currentIndex + 1) % waypoints.Length];

            if (next != null)
            {
                Vector3 pathForward = next.position - target.position;
                pathForward.y = 0f;

                if (pathForward.sqrMagnitude > 0.001f)
                {
                    Vector3 right = Vector3.Cross(
                        Vector3.up,
                        pathForward.normalized
                    );

                    targetPosition += right * laneOffset;
                }
            }
        }

        Vector3 direction = targetPosition - transform.position;
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