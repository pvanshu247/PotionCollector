using System;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI timerText;
    private int currentScore = 0;

    public static event Action<int> OnScoreChanged;

    private void OnEnable()
    {
        OnScoreChanged += IncreaseScore;
        PotionSpawner.OnTimerUpdated += UpdateTimerUI;
    }

    private void IncreaseScore(int amount)
    {
        currentScore += amount;
        scoreText.text = $"Score: {currentScore.ToString()}";
    }

    private void UpdateTimerUI(float timeRemaining)
    {
        if (timeRemaining <= 0) timeRemaining = 0;
        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);
        timerText.text = $"Timer: {minutes:00}:{seconds:00}";
    }

    public static void RaiseScoreChanged(int amount)
    {
        OnScoreChanged?.Invoke(amount);
    }

    private void OnDisable()
    {
        OnScoreChanged -= IncreaseScore;
        PotionSpawner.OnTimerUpdated -= UpdateTimerUI;
    }
}