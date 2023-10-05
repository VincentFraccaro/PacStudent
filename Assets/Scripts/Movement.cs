using System.Collections;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private float speed = 3.0f;
    private Animator animator;

    private Vector3[] waypoints = 
    {
        new Vector3(0.5f, -0.5f, 0),
        new Vector3(0.5f, -4.5f, 0),
        new Vector3(5.5f, -4.5f, 0),
        new Vector3(5.5f, -0.5f, 0)
    };
    private int currentWaypoint = 0;

    private void Start()
    {
        animator = GetComponent<Animator>();
        StartCoroutine(MoveToWaypoint());
    }

    IEnumerator MoveToWaypoint()
    {
        while (true)
        {
            Vector3 start = transform.position;
            Vector3 end = waypoints[currentWaypoint];
            Vector3 direction = (end - start).normalized;
            SetAnimationDirection(direction);
            float journeyLength = Vector3.Distance(start, end);
            float startTime = Time.time;

            float distanceCovered = 0;
            while (distanceCovered < journeyLength)
            {
                float t = (Time.time - startTime) * speed / journeyLength;
                transform.position = Vector3.Lerp(start, end, t);
                distanceCovered = (transform.position - start).magnitude;
                yield return null;
            }

            transform.position = waypoints[currentWaypoint];

            currentWaypoint = (currentWaypoint + 1);
            if (currentWaypoint > 3)
            {
                currentWaypoint = 0;
            }
            
        }
    }

    void SetAnimationDirection(Vector3 direction)
    {
        if (direction == Vector3.up)
            animator.SetInteger("Direction", 1);
        else if (direction == Vector3.down)
            animator.SetInteger("Direction", 3);
        else if (direction == Vector3.left)
            animator.SetInteger("Direction", 2);
        else if (direction == Vector3.right)
            animator.SetInteger("Direction", 0);
    }
}