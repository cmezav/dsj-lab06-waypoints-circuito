using UnityEngine;

public class FollowTracker : MonoBehaviour
{
    public Transform tracker;
    public Transform[] otherCars;

    public float speed = 18f;
    public float turnSpeed = 4f;
    public float followDistance = 2.5f;
    public float laneOffset = 0f;

    public float avoidanceDistance = 5f;
    public float minimumDistance = 2.5f;
    public float avoidanceStrength = 2f;

    void Update()
    {
        if (tracker == null) return;

        Vector3 targetPosition = tracker.position + tracker.right * laneOffset;
        Vector3 direction = targetPosition - transform.position;
        direction.y = 0f;

        if (direction.magnitude <= followDistance) return;

        Vector3 steering = direction.normalized;

        foreach (Transform other in otherCars)
        {
            if (other == null || other == transform) continue;

            Vector3 away = transform.position - other.position;
            away.y = 0f;

            float distance = away.magnitude;
            if (distance > 0.01f && distance < avoidanceDistance)
            {
                float influence = 1f - distance / avoidanceDistance;
                steering += away.normalized * influence * avoidanceStrength;
            }
        }

        if (steering.sqrMagnitude > 0.001f)
        {
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(steering),
                turnSpeed * Time.deltaTime
            );
        }

        Vector3 nextPosition =
            transform.position + transform.forward * speed * Time.deltaTime;

        foreach (Transform other in otherCars)
        {
            if (other == null || other == transform) continue;

            Vector3 gap = nextPosition - other.position;
            gap.y = 0f;

            if (gap.magnitude < minimumDistance)
                return;
        }

        transform.position = nextPosition;
    }
}