using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Movement : MonoBehaviour {
    [Header("Walking")]
    [Tooltip("Speed in units per second")]
    [SerializeField] private float movementSpeed;

    [Header("Jump")]
    [SerializeField] private float jumpHeight;
    [SerializeField] private float peakJumpDistance;
    [SerializeField] private float timeToPeakJump = 0.5f;
    [SerializeField] private float fallMultiplier = 1f;

    [Header("References")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Collider2D col;
    [SerializeField] private Transform center;
    [SerializeField] private BaseController controller;
    [SerializeField] private Rigidbody2D rb;

    public bool facingRight = true;
    public bool isGrounded;
    private Vector2 acceleration;
    private Vector2 velocity = Vector2.zero;
    private float jumpSpeed;
    private float currSpeed;
    private float gravity;
    private float inputX;
    private float speedMultiplier = 1f;
    private bool jumpedToRight;
    private float jumpHorizontalSpeed;

    protected void OnValidate() {
        ComputeValues();
    }

    private void ComputeValues() {
        jumpHorizontalSpeed = peakJumpDistance / timeToPeakJump;
        jumpSpeed = (2f * jumpHeight * jumpHorizontalSpeed) / (peakJumpDistance);
        gravity = -(2f * jumpHeight * jumpHorizontalSpeed * jumpHorizontalSpeed) /
            (peakJumpDistance * peakJumpDistance);
    }

    public void Update() {
        if (isGrounded && controller.Jump()) {
            velocity.y = jumpSpeed;
            jumpedToRight = velocity.x > 0f;
            currSpeed = jumpHorizontalSpeed;
        } else {
            if (velocity.y > 0f && controller.ReleaseJump()) {
                velocity.y = 0f;
            }
        }
    }

    public void FixedUpdate() {
        VerticalMovement();
        HorizontalMovement();
        velocity = velocity + acceleration * Time.deltaTime;
        rb.velocity = velocity;
    }

    private void VerticalMovement() {
        Vector3 xOffset = col.bounds.extents.x * Vector3.right;

        RaycastHit2D rightHitDown = Physics2D.Raycast(transform.position + xOffset, Vector2.down, Utils.PhysicsDelta, groundLayer);
        RaycastHit2D leftHitDown = Physics2D.Raycast(transform.position - xOffset, Vector2.down, Utils.PhysicsDelta, groundLayer);

        bool wasGrounded = isGrounded;
        isGrounded = (rightHitDown.collider != null) || (leftHitDown.collider != null);

        if (isGrounded && !wasGrounded) {
            currSpeed = movementSpeed;
        }

        if (isGrounded && velocity.y <= 0f) {
            acceleration.y = 0f;
            velocity.y = 0f;
        } else {
            RaycastHit2D rightHitUp = Physics2D.Raycast(center.position + xOffset, Vector2.up, col.bounds.extents.y + 3f * Physics2D.defaultContactOffset, groundLayer);
            RaycastHit2D leftHitUp = Physics2D.Raycast(center.position - xOffset, Vector2.up, col.bounds.extents.y + 3f * Physics2D.defaultContactOffset, groundLayer);
            if (velocity.y > 0 && (rightHitUp.collider != null || leftHitUp.collider != null)) {
                velocity.y = 0f;
            }

            acceleration.y = velocity.y > 0f ? gravity : gravity * fallMultiplier;
        }
    }

    private void HorizontalMovement() {
        inputX = controller.Horizontal();

        if ((facingRight && (inputX < 0f)) || (!facingRight && (inputX > 0f))) {

            Flip();
        }

        velocity.x = (Utils.FloatsAreEqual(speedMultiplier, 1f)) ? currSpeed * inputX : speedMultiplier * currSpeed * inputX;
    }

    public void Flip() {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
        facingRight = !facingRight;
    }
}