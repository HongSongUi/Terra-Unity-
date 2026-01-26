using UnityEngine;

public class CutsceneTrigger : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            
            CutsceneManager.Instance.PlayerDragonCutscene();
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CutsceneManager.Instance.PlayerDragonCutscene();
        }
    }
}
