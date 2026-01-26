using UnityEngine;
using UnityEngine.UI;

public class HealthImageComponent : MonoBehaviour
{
    [SerializeField]
    private Image _healthImage;

   
    public void SetHealthAmount(HealthComponent health, float healthPer)
    {
        _healthImage.fillAmount = healthPer;
    }
}
