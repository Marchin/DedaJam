using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TryAgain : MonoBehaviour {

    private void Start() {
        transform.GetChild(0).gameObject.SetActive(false);
        var playerResources = FindObjectOfType<PlayerResources>();
        playerResources.OnResourcesChange += value => {
            if (value < 0) {
                transform.GetChild(0).gameObject.SetActive(true);
                Time.timeScale = 0f;
            }
        };
    }
    
    public void ReloadLevel() {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToMainMenu() {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }
}
