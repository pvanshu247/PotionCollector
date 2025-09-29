using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class PotionSpawner : MonoBehaviour
{
    public static event Action<float> OnTimerUpdated;
    public List<GameObject> potionPrefabs;
    public float spawnInterval = 2f;

    [SerializeField] private BoxCollider spawnAreaCollider;
    [SerializeField] private float totalTime = 120f;

    private float _currentTime;
    private bool _isSpawning = true;

    private void Start()
    {
        _currentTime = totalTime;
        StartCoroutine(SpawnPotions());
        StartCoroutine(TimerCountdown());
    }

    private IEnumerator SpawnPotions()
    {
        while (_isSpawning && _currentTime > 0)
        {
            var spawnPos = GetRandomPositionInCameraView();
            var randomIndex = Random.Range(0, potionPrefabs.Count);
            var prefabToSpawn = potionPrefabs[randomIndex];
            var obj = Instantiate(prefabToSpawn, spawnPos, Quaternion.identity);
            StartCoroutine(DestroyObject(obj));
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private Vector3 GetRandomPositionInCameraView()
    {
        var localPos = new Vector3(
            Random.Range(-spawnAreaCollider.size.x / 2, spawnAreaCollider.size.x / 2),
            0f,
            Random.Range(-spawnAreaCollider.size.z / 2, spawnAreaCollider.size.z / 2)
        );

        // Convert to world space
        var worldPos = spawnAreaCollider.transform.TransformPoint(localPos);
        worldPos.y = 1.5f;
        return worldPos;
    }

    private IEnumerator TimerCountdown()
    {
        while (_currentTime > 0)
        {
            _currentTime -= Time.deltaTime;
            OnTimerUpdated?.Invoke(_currentTime);
            yield return null;
        }

        _isSpawning = false;
    }

    private IEnumerator DestroyObject(GameObject obj)
    {
        yield return new WaitForSeconds(2.5f);
        if (obj != null)
            obj.transform.DOScale(Vector3.zero, 0.2f)
                .SetEase(Ease.InBack)
                .OnComplete(() => Destroy(obj));
    }
}