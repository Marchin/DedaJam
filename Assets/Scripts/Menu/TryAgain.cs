using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class TryAgain : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI _tryAgain = default;
    [SerializeField] private TextMeshProUGUI _victory = default;
    [SerializeField] private TextMeshProUGUI _score = default;

    private void Start() {
        transform.GetChild(0).gameObject.SetActive(false);
        var playerResources = FindObjectOfType<PlayerResources>();
        playerResources.OnResourcesChange += value => {
            if (value < 0) {
                transform.GetChild(0).gameObject.SetActive(true);
                _tryAgain.gameObject.SetActive(true);
                _victory.gameObject.SetActive(false);
                _score.gameObject.SetActive(false);
                Time.timeScale = 0f;
            }
        };
        playerResources.OnVictory += value => {
            transform.GetChild(0).gameObject.SetActive(true);
            _tryAgain.gameObject.SetActive(false);
            _victory.gameObject.SetActive(true);
            _score.gameObject.SetActive(true);
            _score.text = $"SCORE: {value}";
            Time.timeScale = 0f;
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
