using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[System.Serializable]
public class PatternTelegraph // 패턴과 패턴에 대응하는 경고 바닥
{
    public PhaseTwo patternName;          
    public FillWarningArea telegraph;
}

public class WarningAreaManager : MonoBehaviour
{
    [SerializeField]
    private List<PatternTelegraph> _patternTelegraphs;
    private Dictionary<PhaseTwo, FillWarningArea> _dict;

    public event Action OnFillCompleted;
    private void Awake()
    {
        _dict = new Dictionary<PhaseTwo, FillWarningArea>();
        foreach (var pt in _patternTelegraphs)
        {
            _dict[pt.patternName] = pt.telegraph;
            pt.telegraph.OnFilled += FillCompleted;
        }
    }

    private void OnDestroy()
    {
        foreach (var pt in _patternTelegraphs)
        {
            pt.telegraph.OnFilled -= FillCompleted;
        }
    }
    public void CallFilling(PhaseTwo currentPattern)
    {
        if (_dict.TryGetValue(currentPattern, out FillWarningArea telegraph))
        {
            telegraph.gameObject.SetActive(true);
            telegraph.StartFill();
            
        }

    }

    private void FillCompleted()
    {
        OnFillCompleted?.Invoke();
    }
}


