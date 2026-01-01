using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;

    public static UIManager Instance => _instance;
    [Header("GameText UI")]
    [SerializeField]
    private TextMeshProUGUI GameText;
    [SerializeField]
    private TextMeshProUGUI AlartText;
    [SerializeField]
    private TextMeshProUGUI NoticeText;

    [Header("Skill Cooldown UI")]
    [SerializeField]
    private Image _skillOneImg;
    [SerializeField]
    private Image _skillTwoImg;
    [SerializeField]
    private Image _skillThreeImg;

    [Header("Potion Cooldown UI")]
    [SerializeField]
    private Image _potionOneImg;
    [SerializeField]
    private Image _potionTwoImg;

    [Header("Player UI")]
    [SerializeField]
    private Image _playerHealthImg;
    [SerializeField]
    private Image _bossHealthImg;
    [SerializeField]
    private HealthComponent _playerHealth;
    [SerializeField]
    private GameObject _playerHealthObject;
    [SerializeField]
    private Animator _hitAnimator;
    [SerializeField]
    private Animator _warningAnimator;

    [Header("BossHealthBar UI")]
    [SerializeField]
    private GameObject _dragonHealthbar;
    [SerializeField]
    private HealthComponent _bossHealth;

    [Header("Ending UI")]
    [SerializeField]
    private GameObject _victoryUI;
    [SerializeField]
    private GameObject _defeatUI;



    //private Text
    
    private Dictionary<PlayerSkillType, Coroutine> _activeSkillCooldowns = new Dictionary<PlayerSkillType, Coroutine>();
    private Dictionary<PotionType, Coroutine> _activePotionCooldowns = new Dictionary<PotionType, Coroutine>();


    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        _instance = this;

        _playerHealth.OnHealthChange += UpdateHealthUI;
        _bossHealth.OnHealthChange += UpdateHealthUI;

      

    }
    private void OnDestroy()
    {
        _playerHealth.OnHealthChange -= UpdateHealthUI;
        _bossHealth.OnHealthChange -= UpdateHealthUI;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void UpdateHealthUI(HealthComponent sender, float amount)
    {
        if(sender == _playerHealth)
        {
            _playerHealthImg.fillAmount = amount;
        }
        else if(sender == _bossHealth)
        {
           _bossHealthImg.fillAmount = amount;
        }
    }
    public void FillPlayerHealth(float amount)
    {
        _playerHealthImg.fillAmount = amount;
    }
    public void SubscribeSkill(CooldownComponent component)
    {
        component.OnStartCooldownEvent += StartCooldownTracking;
    }
    public void SubscribePotion(PlayerInventoryComponent playerInventory)
    {
        playerInventory.OnStartPotionCooldown += StartCooldownTracking;
    }
    private void StartCooldownTracking(PlayerSkillType type,  float duration)
    {
        if (_activeSkillCooldowns.ContainsKey(type) && _activeSkillCooldowns[type] != null)
        {
            StopCoroutine(_activeSkillCooldowns[type]);
        }
        Image targetImage = null;
        switch (type)
        {
            case PlayerSkillType.Skill_One:
                targetImage = _skillOneImg;
                break;
            case PlayerSkillType.Skill_Two:
                targetImage = _skillTwoImg;
                break;
            case PlayerSkillType.Dash:
                targetImage = _skillThreeImg;
                break;
        }

        Coroutine newRoutine = StartCoroutine(SkillCooldownRoutine(targetImage, duration , type));
        _activeSkillCooldowns[type] = newRoutine;
    }
  
    private IEnumerator SkillCooldownRoutine(Image targetImage, float duration , PlayerSkillType type)
    {
        if(targetImage == null) yield break;

        float startTime = Time.time;
        float elapsedTime = 0f;
        targetImage.fillAmount = 1.0f;
        while (elapsedTime < duration)
        {
            yield return new WaitForSeconds(0.05f);

            elapsedTime = Time.time - startTime;
            float remainingRatio = 1.0f - (elapsedTime / duration);

            targetImage.fillAmount = remainingRatio;
        }
        targetImage.fillAmount = 0f;
        _activeSkillCooldowns.Remove(type);
    }

    private void StartCooldownTracking(PotionType type, float duration)
    {
        if (_activePotionCooldowns.ContainsKey(type) && _activePotionCooldowns[type] != null)
        {
            StopCoroutine(_activePotionCooldowns[type]);
        }
        Image targetImage = null;
        switch (type)
        {
            case PotionType.Healing:
                targetImage = _potionOneImg;
                break;
            case PotionType.Buff:
                targetImage = _potionTwoImg;
                break;
        }

        Coroutine newRoutine = StartCoroutine(PotionCooldownRoutine(targetImage, duration, type));
        _activePotionCooldowns[type] = newRoutine;
    }
    private IEnumerator PotionCooldownRoutine(Image targetImage, float duration, PotionType type)
    {
        if (targetImage == null) yield break;

        float startTime = Time.time;
        float elapsedTime = 0f;
        targetImage.fillAmount = 1.0f;
        while (elapsedTime < duration)
        {
            yield return new WaitForSeconds(0.05f);

            elapsedTime = Time.time - startTime;
            float remainingRatio = 1.0f - (elapsedTime / duration);

            targetImage.fillAmount = remainingRatio;
        }
        targetImage.fillAmount = 0f;
        _activePotionCooldowns.Remove(type);
    }
    public void PlayWarningAnimation(bool state)
    {
        _warningAnimator.SetBool("IsWarning", state);
    }
    public void ShowGameText()
    {
        GameText.gameObject.SetActive(true);
        SoundManager.Instance.PlaySFX("Impact");
    }
    public void AlartTextState(bool state)
    {
        AlartText.gameObject.SetActive(state);
    }
    public void SetNoticeTextState(bool state)
    {
        StartTextAnimation(state);
    }
    public void DisableNoticeText()
    {
        NoticeText.gameObject.SetActive(false);
    }
    public void ShowDragonHealthBar()
    {
        _dragonHealthbar.gameObject.SetActive(true);
    }
    private void StartTextAnimation(bool state)
    {
        var anim = NoticeText.gameObject.GetComponent<Animator>();
        if(anim != null)
        {
            if(state)
            {
                anim.SetTrigger("ShowText");
            
            }
            else
            {
                anim.SetTrigger("HideText");
            }
            
        }
    }
    public void PlayHitImageAnimation()
    {
        _hitAnimator.SetTrigger("Hit");
    }
    public void EnablePlayerUI()
    {
        _playerHealthObject.SetActive(true);
    }
    public void DisablePlayerUI() 
    {
        _playerHealthObject.SetActive(false);
    }
    public void EnableVictoryUI()
    {
        _victoryUI.SetActive(true);
    }
    public void EnableDefeatUI()
    {
        _defeatUI.SetActive(true);
    }
    public void MainButtonClick()
    {
        SceneManager.LoadScene("TitleScene");
    }
    public void ReStartButtonClick()
    {
        SceneManager.LoadScene("InGame");
    }
    public void ExitButtonClick()
    {
        Application.Quit();
    }
}
