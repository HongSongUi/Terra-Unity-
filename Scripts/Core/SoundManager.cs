using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static SoundManager _instance;

    public static SoundManager Instance => _instance;

    [System.Serializable]
    public class Sound
    {
        public string name;
        public AudioClip clip;
        [Range(0f, 1f)] public float volume = 1f;
    }

    public List<Sound> Sounds;
    private Dictionary<string, Sound> _soundDict;

    [Header("Volume Settings")]
    [Range(0f, 1f)] public float BgmVolume = 1f;
    [Range(0f, 1f)] public float SfxVolume = 1f;

    private AudioSource _musicSource;
    private List<AudioSource> _sfxPool = new List<AudioSource>();
    public int SfxPoolSize = 10;

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
            return;
        }

        _soundDict = new Dictionary<string, Sound>();
        foreach (var s in Sounds)
        {
            _soundDict[s.name] = s;
        }


        // BGM용 AudioSource
        _musicSource = gameObject.AddComponent<AudioSource>();
        _musicSource.loop = true;
        _musicSource.volume = BgmVolume;

        // SFX 풀 초기화
        for (int i = 0; i < SfxPoolSize; i++)
        {
            _sfxPool.Add(gameObject.AddComponent<AudioSource>());
        }

    }

    private void Start()
    {
        PlayBGM("MainBGM");
    }
    private AudioSource GetAvailableSFXSource()
    {
        foreach (var source in _sfxPool)
        {
            if (!source.isPlaying)
                return source;
        }
        var newSource = gameObject.AddComponent<AudioSource>();
        _sfxPool.Add(newSource);
        return newSource;
    }

    #region BGM
    public void PlayBGM(string name, bool fade = false, float fadeTime = 1f)
    {
        if (!_soundDict.TryGetValue(name, out Sound sound))
        {
            Debug.LogWarning($"BGM {name} 없음!");
            return;
        }

        if (fade)
        {
            StartCoroutine(FadeBGM(sound.clip, fadeTime, sound.volume));
        }
        else
        {
            _musicSource.clip = sound.clip;
            _musicSource.volume = BgmVolume * sound.volume;
            _musicSource.Play();
        }
    }

    public void StopBGM(bool fade = false, float fadeTime = 1f)
    {
        if (fade)
            StartCoroutine(FadeOutBGM(fadeTime));
        else
            _musicSource.Stop();
    }

    private IEnumerator FadeBGM(AudioClip newClip, float duration, float volume)
    {
        float startVol = _musicSource.volume;

        // 페이드아웃
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            _musicSource.volume = Mathf.Lerp(startVol, 0f, t / duration);
            yield return null;
        }
        _musicSource.volume = 0f;

        _musicSource.clip = newClip;
        _musicSource.Play();

        // 페이드인
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            _musicSource.volume = Mathf.Lerp(0f, BgmVolume* volume, t / duration);
            yield return null;
        }
        _musicSource.volume = BgmVolume* volume;
    }

    private IEnumerator FadeOutBGM(float duration)
    {
        float startVol = _musicSource.volume;
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            _musicSource.volume = Mathf.Lerp(startVol, 0f, t / duration);
            yield return null;
        }
        _musicSource.Stop();
        _musicSource.volume = BgmVolume;
    }
    #endregion
    #region SFX
    public void PlaySFX(string name, float volumeMultiplier = 1f)
    {
        if (!_soundDict.TryGetValue(name, out Sound sound))
        {
            Debug.LogWarning($"SFX {name} 없음!");
            return;
        }

        var source = GetAvailableSFXSource();
        float finalVolume = sound.volume * SfxVolume * volumeMultiplier;
        source.PlayOneShot(sound.clip, finalVolume);
    }
    #endregion

    #region Volume Control
    public void SetBGMVolume(float volume)
    {
        BgmVolume = Mathf.Clamp01(volume);
        if (_musicSource.clip != null)
        {
            _musicSource.volume = BgmVolume * _soundDict[_musicSource.clip.name].volume;
        }
           
    }

    public void SetSFXVolume(float volume)
    {
        SfxVolume = Mathf.Clamp01(volume);
    }
    #endregion
}
