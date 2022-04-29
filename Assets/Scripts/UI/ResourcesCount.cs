using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResourcesCount : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI resourcesText = default;
    private PlayerResources playerResources;

    private void Start() {
        playerResources = FindObjectOfType<PlayerResources>();
        Debug.Assert(playerResources != null, "Player Resources not found");
        resourcesText.text = playerResources.CurrentAmount.ToString();
        playerResources.OnResourcesChange += value => resourcesText.text = Mathf.Max(0, value).ToString();
    }
}