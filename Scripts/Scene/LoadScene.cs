using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadScene : MonoBehaviour
{
    [SerializeField]
    private Image _progressBar;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(LoadLevelAsync());
    }

    IEnumerator LoadLevelAsync()
    {
        AsyncOperation asyncOper = SceneManager.LoadSceneAsync("InGame");
        while (!asyncOper.isDone)
        {
            _progressBar.fillAmount = asyncOper.progress;
            // progressBar.fillAmount = asyncOper.progress;
            yield return new WaitForEndOfFrame();
        }
    }
}
