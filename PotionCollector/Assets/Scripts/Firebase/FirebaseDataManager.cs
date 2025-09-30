using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase.Auth;
using Firebase.Firestore;
using UnityEngine;

public class FirebaseDataManager : MonoBehaviour
{
    FirebaseFirestore db;
    FirebaseUser user;
    public static FirebaseDataManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        db = FirebaseFirestore.DefaultInstance;
        user = FirebaseAuth.DefaultInstance.CurrentUser;
    }

    public async Task SaveSessionData(int score, DateTime sessionStart, DateTime sessionEnd)
    {
        if (user == null)
        {
            Debug.LogError("User not signed in.");
            return;
        }

        DocumentReference docRef = db.Collection("users").Document(user.UserId);
        var sessionData = new Dictionary<string, object>
        {
            { "lastScore", score },
            { "sessionStart", sessionStart },
            { "sessionEnd", sessionEnd }
        };
        try
        {
            await docRef.SetAsync(sessionData, SetOptions.MergeAll);
            Debug.Log("Player session data saved successfully to Firestore.");
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to save session data: {e.Message}");
        }
    }
}