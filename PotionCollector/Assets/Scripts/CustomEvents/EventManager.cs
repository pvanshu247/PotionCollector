using System;
using UnityEngine;

public static class EventManager
{
    // Game lifecycle
    public static event Action<DateTime, string> OnGameStarted;
    public static event Action<DateTime> OnGamePaused;
    public static event Action<DateTime> OnGameResumed;
    public static event Action<DateTime, int> OnGameEnded;
    public static event Action<float> OnTimerUpdated;

    // Potion-related
    public static event Action<string, Vector3> OnPotionSpawned;
    public static event Action<string, int, DateTime> OnPotionCollected;

    // Score / Leaderboard
    public static event Action<int, int> OnScoreUpdated;
    public static event Action<int[]> OnLeaderboardLoaded;

    // Firebase Sync
    public static event Action<string> OnFirebaseSyncStarted;
    public static event Action<string, bool> OnFirebaseSyncCompleted;

    // ------------------ INVOKERS ------------------
    public static void RaiseGameStarted(string sessionId) =>
        OnGameStarted?.Invoke(DateTime.Now, sessionId);

    public static void RaiseGamePaused() =>
        OnGamePaused?.Invoke(DateTime.Now);

    public static void RaiseGameResumed() =>
        OnGameResumed?.Invoke(DateTime.Now);

    public static void RaiseGameEnded(int totalScore) =>
        OnGameEnded?.Invoke(DateTime.Now, totalScore);

    public static void RaiseTimerUpdate(float time) =>
        OnTimerUpdated?.Invoke(time);

    public static void RaisePotionSpawned(string potionType, Vector3 pos) =>
        OnPotionSpawned?.Invoke(potionType, pos);

    public static void RaisePotionCollected(string potionType, int value) =>
        OnPotionCollected?.Invoke(potionType, value, DateTime.Now);

    public static void RaiseScoreUpdated(int newScore, int delta) =>
        OnScoreUpdated?.Invoke(newScore, delta);

    public static void RaiseLeaderboardLoaded(int[] topScores) =>
        OnLeaderboardLoaded?.Invoke(topScores);

    public static void RaiseFirebaseSyncStarted(string operationType) =>
        OnFirebaseSyncStarted?.Invoke(operationType);

    public static void RaiseFirebaseSyncCompleted(string operationType, bool success) =>
        OnFirebaseSyncCompleted?.Invoke(operationType, success);
}
