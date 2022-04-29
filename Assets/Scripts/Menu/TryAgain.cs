using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class TryAgain : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI _score = default;
    [SerializeField] private Image _background = default;
    [SerializeField] private Sprite _victoryImage = default;
    [SerializeField] private Sprite _defeatImage = default;

    private void Start() {
        transform.GetChild(0).gameObject.SetActive(false);
        var playerResources = FindObjectOfType<PlayerResources>();
        playerResources.OnResourcesChange += value => {
            if (value < 0) {
                transform.GetChild(0).gameObject.SetActive(true);
                _score.gameObject.SetActive(false);
                _background.sprite = _defeatImage;
                Time.timeScale = 0f;
            }
        };
        playerResources.OnVictory += value => {
            transform.GetChild(0).gameObject.SetActive(true);
            _score.gameObject.SetActive(true);
            _background.sprite = _victoryImage;
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
