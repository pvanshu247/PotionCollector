using Firebase.Auth;
using TMPro;
using UnityEngine;

public class FirebaseAuthManager : MonoBehaviour
{
    [SerializeField] private GameObject loginPanel;
    [SerializeField] private TextMeshProUGUI errorText;
    public FirebaseAuth auth;
    public FirebaseUser user;

    private void Awake()
    {
        InitializeFirebase();
    }

    void InitializeFirebase()
    {
        auth = FirebaseAuth.DefaultInstance;
    }

    public void SignInAnonymously()
    {
        auth.SignInAnonymouslyAsync().ContinueWith(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                errorText.gameObject.SetActive(true);
                errorText.text = "Firebase authentication failed: " + task.Exception;
                Debug.LogError("Anonymous sign-in failed.");
                return;
            }
            user = task.Result.User;
            Debug.Log("Signed in anonymously with user ID: " + user.UserId);
        });
        loginPanel.SetActive(false);
    }
}
