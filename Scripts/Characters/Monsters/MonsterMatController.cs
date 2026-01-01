using System.Collections;
using UnityEngine;

public class MonsterMatController : MonoBehaviour // 죽으면 서서히 사라지게
{

    [SerializeField]
    private MonsterAnimationEventHandler _animationEventHandler;
    [SerializeField] private SkinnedMeshRenderer _skinnedRenderer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnEnable()
    {
        _animationEventHandler.MonsterDesolve += DesolveEvent;
    }
    private void OnDisable()
    {
        _animationEventHandler.MonsterDesolve -= DesolveEvent;
    }


    public void DesolveEvent()
    {
        StartCoroutine(FadeOutAndDestroy(2.5f)); 
    }

    private IEnumerator FadeOutAndDestroy(float duration)
    {
        // 머티리얼 배열 가져오기 (SkinnedMeshRenderer용)
        Material[] mats = _skinnedRenderer.materials;

        // Transparent 모드 설정
        foreach (var mat in mats)
        {
            mat.SetFloat("_Surface", 1); // 0=Opaque, 1=Transparent
            mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            mat.SetInt("_ZWrite", 0);
            mat.DisableKeyword("_ALPHATEST_ON");
            mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            mat.EnableKeyword("_ALPHABLEND_ON");
            mat.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
        }

        float time = 0f;
        while (time < duration)
        {
            time += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, time / duration);

            foreach (var mat in mats)
            {
                if (mat.HasProperty("_BaseColor")) // URP Lit Shader Base Color
                {
                    Color c = mat.GetColor("_BaseColor");
                    mat.SetColor("_BaseColor", new Color(c.r, c.g, c.b, alpha));
                }
                else if (mat.HasProperty("_Color")) // fallback
                {
                    Color c = mat.color;
                    mat.color = new Color(c.r, c.g, c.b, alpha);
                }
            }

            yield return null;
        }

        Destroy(gameObject);
    }

}
