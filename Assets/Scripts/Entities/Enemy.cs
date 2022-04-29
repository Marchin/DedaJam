using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    [SerializeField] private float cycleDuration = default;
    [SerializeField] private float distance = default;
    [Range(0f, 1f)] [SerializeField] private float initPoint = 0.5f;
    [Header("References")]
    [SerializeField] private Rigidbody2D rb = default;
    private Vector2 pointA;
    private Vector2 pointB;
    private float currentPoint;
    private bool goingToPointB;

    private void Awake() {
        currentPoint = initPoint;
        pointA = (Vector2)transform.position + (Vector2.right * distance * (1f - initPoint));
        pointB = (Vector2)transform.position - (Vector2.right * distance * initPoint);
    }

    private void FixedUpdate() {
        float newPoint = goingToPointB ? Mathf.Min(cycleDuration, currentPoint + Time.deltaTime) :
            Mathf.Max(0f, currentPoint - Time.deltaTime);

        rb.position = Vector2.Lerp(pointA, pointB, newPoint / cycleDuration);

        currentPoint = newPoint;

        if (goingToPointB && currentPoint >= cycleDuration) {
            goingToPointB = false;
        }
        if (!goingToPointB && currentPoint <= 0f) {
            goingToPointB = true;
        }
    }
}
