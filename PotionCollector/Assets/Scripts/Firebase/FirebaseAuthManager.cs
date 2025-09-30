using Firebase.Auth;
using UnityEngine;

public class FirebaseAuthManager : MonoBehaviour
{
    public FirebaseAuth auth;
    public FirebaseUser user;

    private void Awake()
    {
        InitializeFirebase();
    }

    void InitializeFirebase()
    {
        auth = FirebaseAuth.DefaultInstance;
        SignInAnonymously();
    }

    void SignInAnonymously()
    {
        auth.SignInAnonymouslyAsync().ContinueWith(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.LogError("Anonymous sign-in failed.");
                return;
            }
            user = task.Result.User;
            Debug.Log("Signed in anonymously with user ID: " + user.UserId);
        });
    }
}
