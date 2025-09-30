using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private int _currentScore = 0;
    private string _sessionId;


    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void OnEnable()
    {
        EventManager.OnGamePaused += GamePaused;
        EventManager.OnGameResumed += GameResumed;
    }

    private void Start()
    {
        _sessionId = Guid.NewGuid().ToString();
        EventManager.RaiseGameStarted(_sessionId);
    }

    public void AddScore(int amount)
    {
        _currentScore += amount;
        EventManager.RaiseScoreUpdated(_currentScore, amount);
    }

    private void GameResumed(DateTime time)
    {
        Debug.Log($"Game Resumed at: {time}");
        Time.timeScale = 1f;
    }

    private void GamePaused(DateTime time)
    {
        Debug.Log($"Game Paused at: {time}");
        Time.timeScale = 0f;
    }

    private void OnDisable()
    {
        EventManager.OnGamePaused -= GamePaused;
        EventManager.OnGameResumed -= GameResumed;
    }

    public int CurrentScore => _currentScore;
}
