using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMovement : MonoBehaviour {
    [SerializeField] private float cycleDuration = default;
    [SerializeField] private float distance = default;
    [Range(0f, 1f)] [SerializeField] private float initPoint = 0.5f;
    [SerializeField] private Vector2 direction = Vector2.right;
    private Vector2 pointA;
    private Vector2 pointB;
    private float currentPoint;
    private float halfCycle;
    private bool goingToPointB;

    private void Awake() {
        Vector2 normalizeDirection = direction.normalized;
        currentPoint = initPoint;
        halfCycle = cycleDuration * 0.5f;
        pointA = (Vector2)transform.position + (normalizeDirection * distance * (1f - initPoint));
        pointB = (Vector2)transform.position - (normalizeDirection * distance * initPoint);
    }

    private void FixedUpdate() {
        currentPoint = goingToPointB ? Mathf.Min(halfCycle, currentPoint + Time.deltaTime) :
            Mathf.Max(0f, currentPoint - Time.deltaTime);

        transform.position = Vector2.Lerp(pointA, pointB, currentPoint / halfCycle);

        if (goingToPointB && currentPoint >= halfCycle) {
            goingToPointB = false;
        }
        if (!goingToPointB && currentPoint <= 0f) {
            goingToPointB = true;
        }
    }
}