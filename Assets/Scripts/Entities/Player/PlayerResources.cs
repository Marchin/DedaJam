using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerResources : MonoBehaviour {
    [SerializeField] private int initialAmount = default;

    [Header("References")]
    [SerializeField] private BaseController controller;
    private int currAmount;

    private void Awake() {
        currAmount = initialAmount;
    }

    private void Update() {
        if (controller.Fire()) {
            if (currAmount > 0) {
                --currAmount;
                Debug.Log($"Shoot - ({currAmount} shots left)");
            } else {
                Debug.Log("No ammo");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Resource")) {
            ++currAmount;
            Destroy(other.gameObject);
            Debug.Log($"Pick - ({currAmount} shots left)");
        }
    }
}
