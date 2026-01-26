using Unity.Cinemachine;
using UnityEngine;
using static Unity.Cinemachine.CinemachineImpulseDefinition;

public class CameraManager : MonoBehaviour
{
    private static CameraManager _instance;

    public static CameraManager Instance => _instance;

    private CinemachineImpulseSource _impulseSource;
    private void Awake()
    {
        _instance = this;

        _impulseSource = GetComponent<CinemachineImpulseSource>();

    }
    public void ShakeCamera(Vector3 impulseDir,float strength, float duration = 0.5f)
    {

        _impulseSource.ImpulseDefinition.ImpulseDuration = duration;
        _impulseSource.GenerateImpulse(impulseDir);
    }
    public void ShakeCameraOnExplosion(float strength, float duration = 0.5f)
    {
        if (_impulseSource == null) return;
        _impulseSource.ImpulseDefinition.ImpulseShape = ImpulseShapes.Rumble;
        Vector3 impulseForce = (Vector3.up+Vector3.left+Vector3.back) * strength;
        ShakeCamera(impulseForce, strength, duration);
    }
    public void ShakeCameraOnHit(float strength, float duration = 0.5f)
    {
        if (_impulseSource == null) return;
        _impulseSource.ImpulseDefinition.ImpulseShape = ImpulseShapes.Recoil;
        Vector3 impulseForce = Vector3.up* strength;
        ShakeCamera(impulseForce, strength, duration);
    }
}
