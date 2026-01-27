using System;
using System.Collections;
using UnityEngine;

public class FillWarningArea : MonoBehaviour
{
    [SerializeField]
    private float _fillSpeed = 1f;
    [SerializeField]
    private string _progressName;
    private Renderer _rend;
    private float _fillAmount;
    private Material _mat;

    public event Action OnFilled;
    private void OnEnable()
    {
        if(string.IsNullOrEmpty(_progressName))
        {
            _progressName = "_Progress";
        }
        _rend = GetComponent<Renderer>();
        if(_rend != null)
        {
            _fillAmount = 0;
            _mat = _rend.material;

            _mat.SetFloat(_progressName, 0);
            
        }
    }
    private void OnDisable()
    {
        
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void StartFill()
    {
       StartCoroutine(FillingRoutine());
    }
    private IEnumerator FillingRoutine()
    {
        while (_fillAmount < 1f)
        {
            _fillAmount += Time.deltaTime * _fillSpeed;
            _mat.SetFloat(_progressName, _fillAmount);
            yield return null;
        }
        OnFilled?.Invoke();
        gameObject.SetActive(false);
    }
}
