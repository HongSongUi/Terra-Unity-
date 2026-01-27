using UnityEngine;

public class Billboard : MonoBehaviour
{

    private Transform _cameraTransform;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _cameraTransform = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {

;   }
    private void LateUpdate()
    {
        Transform cameraTransform = _cameraTransform;
        Vector3 targetDirection = cameraTransform.position - transform.position;
        targetDirection.y = 0;
        transform.rotation = Quaternion.LookRotation(targetDirection);
    }
}
