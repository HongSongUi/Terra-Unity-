using System;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    public static GameManager Instance => _instance;

    [SerializeField]
    private GameObject _monsters;
    [SerializeField]
    private GameObject _dragon;
    [SerializeField]
    private TeleportTrigger _teleportTrigger;
    [SerializeField]
    private GameObject _dragonCutsceneTrigger;
    [SerializeField]
    private PlayerController _playerController;

    private int _monstersCount;

    private bool _isCutsceneEnd;
    private bool _cusorLocked = true;
    public bool IsCutsceneEnd => _isCutsceneEnd;

    private bool _isPlayerWin;
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _monstersCount = _monsters.transform.childCount;
        _dragonCutsceneTrigger.SetActive(false);
        ApplyCursorState();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void DecreaseMonsterCount()
    {
        _monstersCount--;
        if(_monstersCount <= 0)
        {
            CutsceneManager.Instance.PlayDoorOpenCutscene();
            _dragonCutsceneTrigger.SetActive(true);
        }
    }
    private void OnTeleportState()
    {
        _teleportTrigger.TeleportTriggerOn();
    }
    private void OnApplicationFocus(bool focus)
    {
        if(focus)
        {
            ApplyCursorState();
        }
    }
    
    private void ApplyCursorState()
    {
        Cursor.lockState = _cusorLocked ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !_cusorLocked;
    }
    public void ReceiveCutsceneEndSignal()
    {
        UIManager.Instance.EnablePlayerUI();
        UIManager.Instance.ShowDragonHealthBar();
        _playerController.EnablePlayerInput();
  
    }
    public void ReceiveCutsceneStartSignal()
    {
        UIManager.Instance.DisablePlayerUI();
        _playerController.DiablePlayerInput();
     
    }
    public void ReceiveVictoryCutsceneEnd()
    {
        UIManager.Instance.EnableVictoryUI();
        
    }
    public void GameEnd(bool playerVictory)
    {
        Cursor.lockState = CursorLockMode.None;
        _playerController.DiablePlayerInput();
        Cursor.visible = true;
        if (playerVictory)
        {
            CutsceneManager.Instance.PlayWinCutscene();
            SoundManager.Instance.PlayBGM("Victory");

        }
        else
        {
            SoundManager.Instance.PlaySFX("Defeat");
            UIManager.Instance.EnableDefeatUI();
       
        }
    }
}
