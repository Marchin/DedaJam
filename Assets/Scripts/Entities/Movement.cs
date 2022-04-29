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
    [SerializeField] private Animator animator;

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
    private List<PlatformMovement> inContactPlatforms = new List<PlatformMovement>(3);

    private void Awake() {
        ComputeValues();
    }

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

        animator.SetBool("Walking", Mathf.Abs(velocity.x) > 0.1f);
    }

    private void VerticalMovement() {
        Vector3 xOffset = col.bounds.extents.x * Vector3.right;

        RaycastHit2D rightHitDown = Physics2D.Raycast(transform.position + xOffset, Vector2.down, Utils.PhysicsDelta, groundLayer);
        RaycastHit2D leftHitDown = Physics2D.Raycast(transform.position - xOffset, Vector2.down, Utils.PhysicsDelta, groundLayer);

        bool wasGrounded = isGrounded;
        isGrounded = (rightHitDown.collider != null) || (leftHitDown.collider != null);

        if (isGrounded && !wasGrounded) {
            currSpeed = movementSpeed;
            Vector3 newPos = transform.position;
            float maxSurface = (rightHitDown.collider != null) ? rightHitDown.collider.bounds.max.y : leftHitDown.collider.bounds.max.y;
            newPos.y += Mathf.Max(Utils.PhysicsDelta, maxSurface - newPos.y);
            transform.position = newPos;
        }

        if (isGrounded && velocity.y <= 0f) {
            acceleration.y = 0f;
            velocity.y = inContactPlatforms.Count > 0 ? inContactPlatforms[0].Speed.y : 0f;
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

        if (inContactPlatforms.Count > 0) {
            velocity.x += inContactPlatforms[0].Speed.x;
        }
    }

    public void Flip() {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
        facingRight = !facingRight;
    }
    
    private void OnCollisionEnter2D(Collision2D col) {
        PlatformMovement movement = col.gameObject.GetComponent<PlatformMovement>();

        if (movement != null) {
            inContactPlatforms.Add(movement);
        }
    }

    private void OnCollisionExit2D(Collision2D col) {
        PlatformMovement movement = inContactPlatforms.Find(m => m.gameObject == col.gameObject);

        if (movement != null) {
            inContactPlatforms.Remove(movement);
        }
    }
}