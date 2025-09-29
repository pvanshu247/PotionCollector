using UnityEngine;
using DG.Tweening;

public class Potion : MonoBehaviour
{
    private bool _isCollected = false;

    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            HandleRaycast();
        }
#else
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                HandleRaycast();
            }
        }
#endif
    }

    private void HandleRaycast()
    {
        if (_isCollected) return; 
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.transform == transform)
            {
                Collect();
            }
        }
    }

    void Collect()
    {
        if (_isCollected) return;
        _isCollected = true;
        UIManager.RaiseScoreChanged(1);
        transform.DOScale(Vector3.zero, 0.2f)
            .SetEase(Ease.InBack)
            .OnComplete(() => Destroy(gameObject));
    }
}
