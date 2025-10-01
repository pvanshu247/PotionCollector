using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;

public class PotionSpawner : MonoBehaviour
{
    //public List<AssetReferenceGameObject> potionPrefabs;
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
            
            EventManager.RaisePotionSpawned(prefabToSpawn.name, spawnPos);
            
            StartCoroutine(DestroyObject(obj));
            
            // var handle = prefabToSpawn.InstantiateAsync(spawnPos, Quaternion.identity);
            //
            // if (handle.Status == AsyncOperationStatus.Succeeded)
            // {
            //     GameObject obj = handle.Result;
            //
            //     EventManager.RaisePotionSpawned(prefabToSpawn.RuntimeKey.ToString(), spawnPos);
            //
            //     StartCoroutine(DestroyObject(obj));
            // }

            yield return new WaitForSeconds(Random.Range(0.5f, 2f));
        }
    }

    private Vector3 GetRandomPositionInCameraView()
    {
        var localPos = new Vector3(
            Random.Range(-spawnAreaCollider.size.x / 2, spawnAreaCollider.size.x / 2),
            0f,
            Random.Range(-spawnAreaCollider.size.z / 2, spawnAreaCollider.size.z / 2)
        );

        var worldPos = spawnAreaCollider.transform.TransformPoint(localPos);
        worldPos.y = 1.5f;
        return worldPos;
    }

    private IEnumerator TimerCountdown()
    {
        while (_currentTime > 0)
        {
            _currentTime -= Time.deltaTime;
            EventManager.RaiseTimerUpdate(_currentTime);
            yield return null;
        }
        EventManager.RaiseGameEnded(GameManager.Instance.CurrentScore);
        _isSpawning = false;
    }

    private IEnumerator DestroyObject(GameObject obj)
    {
        yield return new WaitForSeconds(Random.Range(0.7f, 2.2f));
        if (obj != null)
            obj.transform.DOScale(Vector3.zero, 0.2f)
                .SetEase(Ease.InBack)
                .OnComplete(() => Destroy(obj));
    }
}