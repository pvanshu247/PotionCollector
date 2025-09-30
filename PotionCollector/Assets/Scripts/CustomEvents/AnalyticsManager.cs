using System;
using UnityEngine;

public class AnalyticsManager : MonoBehaviour
{
    private DateTime _sessionStartTime;
    private void OnEnable()
    {
        EventManager.OnGameStarted += GameStarted;
        EventManager.OnGameEnded += GameEnded;
    }

    private void GameStarted(DateTime time, string sessionId)
    {
        _sessionStartTime = time;
        Debug.Log($"Analytics: Game Started with Session ID: {sessionId} at {time}");
    }

    private async void GameEnded(DateTime time, int score)
    {
        Debug.Log($"Analytics: Game Ended with score: {score} at {time}");
        await FirebaseDataManager.Instance.SaveSessionData(score, _sessionStartTime, time);
    }

    private void OnDisable()
    {
        EventManager.OnGameStarted -= GameStarted;
        EventManager.OnGameEnded -= GameEnded;
    }
}
