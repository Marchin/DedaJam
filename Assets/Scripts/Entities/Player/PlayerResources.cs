using System;
using UnityEngine;

public class PlayerResources : MonoBehaviour {
    [SerializeField] private int initialAmount = default;
    [SerializeField] private float shootInterval = default;
    [SerializeField] private Vector2 shootOffset = default;

    [Header("References")]
    [SerializeField] private BaseController controller = default;
    [SerializeField] private Movement movement = default;
    [SerializeField] private GameObject shot = default;
    [SerializeField] private AudioSource audioSource = default;
    [SerializeField] private AudioClip shotSound = default;
    [SerializeField] private AudioClip drinkSound = default;
    private int currAmount;
    private float timer;
    public event Action<int> OnResourcesChange;
    public event Action<int> OnVictory;
    public int CurrentAmount => currAmount;

    private void Awake() {
        currAmount = initialAmount;
    }

    private void Update() {
        if (controller.Fire() && timer <= 0f) {
            if (currAmount > 0) {
                audioSource.clip = shotSound;
                audioSource.Play();
                Vector3 offset = shootOffset;
                if  (!movement.facingRight) {
                    offset.x = -offset.x;
                }
                var go = Instantiate(shot, transform.position + offset, Quaternion.identity);
                go.GetComponent<Shot>().Direction = movement.facingRight ? Vector2.right : Vector2.left;
                go.SetActive(true);
            }
                
            --currAmount;
            OnResourcesChange(currAmount);
            timer = shootInterval;

            if (currAmount < 0) {
                enabled = false;
            }
        }
        
        if (timer >= 0f) {
            timer -= Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Resource")) {
            audioSource.clip = drinkSound;
            audioSource.Play();
            Destroy(other.gameObject);
            ++currAmount;
            OnResourcesChange(currAmount);
        } else if (other.CompareTag("DeathZone")) {
            Die();
        } else if (other.CompareTag("Victory")) {
            OnVictory(currAmount);
        } else if (other.CompareTag("Enemy")) {
            Die();
        }
    }

    private void Die() {
        currAmount = -1;
        OnResourcesChange(currAmount);
    }
}
