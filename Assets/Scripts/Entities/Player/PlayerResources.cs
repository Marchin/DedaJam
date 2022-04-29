using System;
using UnityEngine;

public class PlayerResources : MonoBehaviour {
    [SerializeField] private int initialAmount = default;
    [SerializeField] private float shootInterval = default;
    [SerializeField] private int sadFaceValue = default;
    [SerializeField] private int happyFaceValue = default;
    [SerializeField] private Vector2 shootOffset = default;

    [Header("References")]
    [SerializeField] private BaseController controller = default;
    [SerializeField] private Movement movement = default;
    [SerializeField] private GameObject shot = default;
    [SerializeField] private AudioSource audioSource = default;
    [SerializeField] private AudioClip shotSound = default;
    [SerializeField] private GameObject[] petalos = default;
    [SerializeField] private AudioClip drinkSound = default;
    [SerializeField] private Sprite regularFace = default;
    [SerializeField] private Sprite hitFace = default;
    [SerializeField] private Sprite sadFace = default;
    [SerializeField] private Sprite happyFace = default;
    [SerializeField] private SpriteRenderer face = default;
    private int currAmount;
    private float timer;
    public event Action<int> OnResourcesChange;
    public event Action<int> OnVictory;
    public int CurrentAmount => currAmount;

    private void Awake() {
        currAmount = initialAmount;

        for (int i = 0; i < petalos.Length; i++) {
            petalos[i].gameObject.SetActive(i < initialAmount);
        }

        RefreshFace();
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
                petalos[currAmount - 1].gameObject.SetActive(false);
                RefreshFace();
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

    private void RefreshFace() {
        if (currAmount <= sadFaceValue) {
            face.sprite = sadFace;
        } else if (currAmount >= happyFaceValue) {
            face.sprite = happyFace;
        } else {
            face.sprite = regularFace;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Resource")) {
            audioSource.clip = drinkSound;
            audioSource.Play();
            Destroy(other.gameObject);
            ++currAmount;
            petalos[currAmount - 1].gameObject.SetActive(true);
            OnResourcesChange(currAmount);
                RefreshFace();
            if (currAmount <= sadFaceValue) {
                face.sprite = sadFace;
            }
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
        face.sprite = hitFace;
        OnResourcesChange(currAmount);
    }
}
