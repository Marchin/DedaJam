using UnityEngine;

public class Shot : MonoBehaviour {
    [SerializeField] private float speed = default;
    [SerializeField] private float maxDuration = default;
    [SerializeField] private Rigidbody2D rb = default;
    private float timer;
    public Vector2 Direction {
        set {
            rb.velocity = value * speed;
        }
    }

    private void Awake() {
        rb.velocity = Vector2.right * speed;
    }

    private void OnEnable() {
        timer = maxDuration;
    }

    private void Update() {
        timer -= Time.deltaTime;
        if (timer <= 0f) {
            Destroy(gameObject);
        }
    }
}