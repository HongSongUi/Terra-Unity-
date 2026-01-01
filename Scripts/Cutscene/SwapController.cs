using UnityEngine;

public class SwapController : MonoBehaviour 
{
    [SerializeField]
    private GameObject _playerCharacter; // 컷신 종료 후 활성화될 플레이어
    [SerializeField]
    private GameObject _cutsceneCharacter; // 컷신 동안 움직이는 캐릭터

    [SerializeField]
    private GameObject _playerDragon; // 컷신 종료 후 활성화 될 드래곤
    [SerializeField]
    private GameObject _cutsceneDragon; // 컷신 동안 움직이는 드래곤

    private void TransferPositionAndAcivate(GameObject target, GameObject source)
    {
        if(source.activeInHierarchy)
        {
            Vector3 finalPos = source.transform.position;

            Quaternion finalRot = source.transform.rotation;

            target.transform.position = finalPos;
            target.transform.rotation = finalRot;

            source.SetActive(false);

            target.SetActive(true);

        }
    }

    public void TransferPositionPlayer()
    {
        TransferPositionAndAcivate(_playerCharacter, _cutsceneCharacter);
    }
    
    public void TransferPositionDragon()
    {
        TransferPositionAndAcivate(_playerDragon, _cutsceneDragon);
    }
    public void PlayerDisable()
    {
        _playerCharacter.SetActive(false);
    }
    public void DragonDisable()
    {
        _playerDragon.SetActive(false);
    }
}
