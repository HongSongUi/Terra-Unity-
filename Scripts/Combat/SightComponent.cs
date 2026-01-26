using System;
using UnityEngine;

public class SightComponent : MonoBehaviour
{
    [SerializeField]
    private Transform _eyePosition;
    [SerializeField]
    private float _viewAngle;
    [SerializeField]
    private float _viewDistance;
    [SerializeField]
    private float _timeToDetect = 1.5f;  // 인식까지 필요한 시간
    [SerializeField]
    private LayerMask _targetMask;
    [SerializeField]
    private LayerMask _obstacleMask;

    private float _currentDetectTime = 0f;

    private bool _isDetecting = false;
    public event Action<bool> OnPlayerDetected;
    bool playerInSight = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
         DetectTarget();
        
       
    }
    void DetectTarget()
    {
        if (_eyePosition == null)
        {
            Debug.Log("eye position is None");
            return;
        }

        playerInSight = false;
        Collider[] targetsInViewRadius = Physics.OverlapSphere(_eyePosition.position, _viewDistance, _targetMask);
        foreach (Collider target in targetsInViewRadius)
        {
            if (!target.CompareTag("Player")) continue;

            Vector3 dirToTarget = (target.transform.position - _eyePosition.position).normalized;
            float angleToTarget = Vector3.Angle(_eyePosition.forward, dirToTarget);

            if (angleToTarget < _viewAngle / 2f)
            {
                float distToTarget = Vector3.Distance(_eyePosition.position, target.transform.position);
                if (!Physics.Raycast(_eyePosition.position, dirToTarget, distToTarget, _obstacleMask))
                {
                    playerInSight = true;
                    break;
                }
            }
        }

        if (playerInSight)
        {
            // 감지 카운트업
            _currentDetectTime += Time.deltaTime;
            if (_currentDetectTime >= _timeToDetect && !_isDetecting)
            {
               
                _isDetecting = true;
                OnPlayerDetected?.Invoke(_isDetecting); // 감지됨
                _currentDetectTime = 0f;
            }
        }
        else
        {
            // 플레이어가 시야 밖 -> 초기화
            _currentDetectTime = 0f;

            if (_isDetecting) // 이미 감지 상태였다면 해제
            {
                _isDetecting = false;
                OnPlayerDetected?.Invoke(_isDetecting); // 감지 해제됨
            }
        }
    }
    private void OnDrawGizmosSelected()
    {
        if (_eyePosition == null)
        {
            Debug.Log("eye position is None");
            return;
        }
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_eyePosition.position, _viewDistance);

        Vector3 leftBoundary = DirFromAngle(-_viewAngle / 2);
        Vector3 rightBoundary = DirFromAngle(_viewAngle / 2);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(_eyePosition.position, _eyePosition.position + leftBoundary * _viewDistance);
        Gizmos.DrawLine(_eyePosition.position, _eyePosition.position + rightBoundary * _viewDistance);
    }
    Vector3 DirFromAngle(float angleInDegrees)
    {
        angleInDegrees += _eyePosition.eulerAngles.y;
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}
