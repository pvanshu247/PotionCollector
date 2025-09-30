using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private Sprite pauseSprite;
    [SerializeField] private Sprite playSprite;
    [SerializeField] private Image buttonImage;

    private bool _isPaused = false;

    private void OnEnable()
    {
        EventManager.OnScoreUpdated += UpdateScoreUI;
        EventManager.OnTimerUpdated += UpdateTimerUI;
    }

    private void UpdateScoreUI(int newScore, int delta)
    {
        scoreText.text = $"Score: {newScore}";
    }

    private void UpdateTimerUI(float timeRemaining)
    {
        if (timeRemaining <= 0) timeRemaining = 0;
        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);
        timerText.text = $"Timer: {minutes:00}:{seconds:00}";
    }

    public void TogglePause()
    {
        _isPaused = !_isPaused;

        if (_isPaused)
        {
            EventManager.RaiseGamePaused();
            buttonImage.sprite = playSprite;
        }
        else
        {
            EventManager.RaiseGameResumed();
            buttonImage.sprite = pauseSprite;
        }
    }

    private void OnDisable()
    {
        EventManager.OnScoreUpdated -= UpdateScoreUI;
        EventManager.OnTimerUpdated -= UpdateTimerUI;
    }
}