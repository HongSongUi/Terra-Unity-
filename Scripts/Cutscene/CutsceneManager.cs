using UnityEngine;
using UnityEngine.Playables;

public class CutsceneManager : MonoBehaviour
{
    private static CutsceneManager _instance;

    public static CutsceneManager Instance => _instance;

    [SerializeField]
    private PlayableDirector _doorOpenCutscene;
    [SerializeField]
    private PlayableDirector _dragonCutscene;
    [SerializeField]
    private PlayableDirector _winCutscene;
    private void Awake()
    {
        _instance = this;
  
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void PlayDoorOpenCutscene()
    {
        _doorOpenCutscene.Play();
    }
    public void PlayerDragonCutscene()
    {
        if(_dragonCutscene.state != PlayState.Playing)
        {
            SoundManager.Instance.PlayBGM("CutsceneBGM",true);
            _dragonCutscene.Play();
        }
      
    }
    public void PlayDragonWingsSound()
    {
        SoundManager.Instance.PlaySFX("DragonWing");
    }
    public void PlayWinCutscene()
    {
        _winCutscene.Play();
    }
}
