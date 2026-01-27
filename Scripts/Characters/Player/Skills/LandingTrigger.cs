using UnityEngine;

public class LandingTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Ground"))
        {
            GetComponentInParent<SwordSkill>().OnHitGround();
        }
        
    }
}
