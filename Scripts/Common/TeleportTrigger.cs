using System;
using TMPro;
using UnityEngine;

public class TeleportTrigger : MonoBehaviour
{
    [SerializeField]
    private Transform _nextTransform;
    [SerializeField]
    private bool _inOnTrigger;

    private void OnTriggerEnter(Collider other)
    {
       
   
        if (!_inOnTrigger)
        {
            UIManager.Instance.AlartTextState(true);
            return;
        }
        if(other.CompareTag("Player"))
        {
            UIManager.Instance.SetNoticeTextState(true);
            var controller = other.GetComponent<PlayerController>();
            if(controller != null)
            {
                controller.SetPlayerTP(_nextTransform.position, true);
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (!_inOnTrigger)
        {
            UIManager.Instance.AlartTextState(true);
            return;
        }
        else
        {
            UIManager.Instance.AlartTextState(false);
        }
        if (other.CompareTag("Player"))
        {
            var controller = other.GetComponent<PlayerController>();
            if (controller != null)
            {
                controller.SetPlayerTP(_nextTransform.position, true);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        UIManager.Instance.SetNoticeTextState(false);
        UIManager.Instance.AlartTextState(false);
        if (other.CompareTag("Player"))
        {
            var controller = other.GetComponent<PlayerController>();
            if (controller != null)
            {
                controller.SetPlayerTP(_nextTransform.position, false);
            }
        }
    }

    public void TeleportTriggerOn()
    {
        _inOnTrigger = true;
    }
}
