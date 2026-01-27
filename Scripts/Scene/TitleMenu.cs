using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleMenu : MonoBehaviour
{

    public void OnStartGame()
    {
        SceneManager.LoadScene("LoadingScene");
    }
    public void OnQuitGame()
    {
        Application.Quit();
    }
}
