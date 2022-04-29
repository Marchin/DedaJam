using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMovement : MonoBehaviour {
    [SerializeField] private float cycleDuration = default;
    [SerializeField] private float distance = default;
    [Range(0f, 1f)] [SerializeField] private float initPoint = 0.5f;
    [SerializeField] private Vector2 direction = Vector2.right;
    public Vector2 Speed { get; private set; }
    private Vector2 pointA;
    private Vector2 pointB;
    private Vector2 normalizedDirection;
    private float currentPoint;
    private bool goingToPointB;

    private void Awake() {
        normalizedDirection = direction.normalized;
        currentPoint = initPoint;
        pointA = (Vector2)transform.position + (normalizedDirection * distance * (1f - initPoint));
        pointB = (Vector2)transform.position - (normalizedDirection * distance * initPoint);
    }

    private void FixedUpdate() {
        float newPoint = goingToPointB ? Mathf.Min(cycleDuration, currentPoint + Time.deltaTime) :
            Mathf.Max(0f, currentPoint - Time.deltaTime);

        transform.position = Vector2.Lerp(pointA, pointB, newPoint / cycleDuration);

        Speed = (goingToPointB ? -normalizedDirection : normalizedDirection) * (distance / cycleDuration);

        currentPoint = newPoint;

        if (goingToPointB && currentPoint >= cycleDuration) {
            goingToPointB = false;
        }
        if (!goingToPointB && currentPoint <= 0f) {
            goingToPointB = true;
        }
    }
}