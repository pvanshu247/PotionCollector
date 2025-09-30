using System.Collections.Generic;
using Firebase.Firestore;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardHandler : MonoBehaviour
{
    [SerializeField] private GameObject leaderboardItemPrefab;
    [SerializeField] private Transform leaderboardItemParent;
    [SerializeField] private Image loaderImg;

    private FirebaseFirestore db;

    private void Start()
    {
        db = FirebaseFirestore.DefaultInstance;
        FetchLeaderboard();
    }

    public async void FetchLeaderboard()
    {
        loaderImg.gameObject.SetActive(true);
        foreach (Transform child in leaderboardItemParent)
        {
            Destroy(child.gameObject);
        }

        Query leaderboardQuery = db.Collection("users")
            .OrderByDescending("lastScore")
            .Limit(5);

        QuerySnapshot snapshot = await leaderboardQuery.GetSnapshotAsync();

        int rank = 1;
        List<int> topScores = new List<int>();
        foreach (DocumentSnapshot doc in snapshot.Documents)
        {
            var data = doc.ToDictionary();
            string playerId = doc.Id;
            int score = data.ContainsKey("lastScore") ? int.Parse(data["lastScore"].ToString()) : 0;

            GameObject entry = Instantiate(leaderboardItemPrefab, leaderboardItemParent);

            TextMeshProUGUI[] texts = entry.GetComponentsInChildren<TextMeshProUGUI>();
            texts[0].text = rank.ToString() + ".";
            texts[1].text = playerId;
            texts[2].text = score.ToString();

            rank++;
            topScores.Add(score);
        }

        loaderImg.gameObject.SetActive(false);
        EventManager.RaiseLeaderboardLoaded(topScores.ToArray());
    }
    
    public void CloseLeaderboard()
    {
        Destroy(gameObject);
    }
}
