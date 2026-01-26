using UnityEngine;

public class TextBillboard : MonoBehaviour
{
    private Transform _cameraTransform;

    void Awake()
    {
        if (Camera.main != null)
        {
            _cameraTransform = Camera.main.transform;
        }
    }

    void LateUpdate()
    {
        if (_cameraTransform == null) return;

        Vector3 targetDirection = _cameraTransform.position - transform.position;


        targetDirection.y = 0;

        Vector3 finalDirection = -targetDirection.normalized;

        transform.rotation = Quaternion.LookRotation(finalDirection, Vector3.up);
    }
}
