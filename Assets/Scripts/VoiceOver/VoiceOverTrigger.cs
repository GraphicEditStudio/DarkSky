using System;
using UnityEngine;

namespace VoiceOver
{
    public class VoiceOverTrigger : MonoBehaviour
    {
        [SerializeField] private VoiceOverAsset voiceOverAsset;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && voiceOverAsset != null)
            {
                VoiceOverManager.Instance.AddVoiceOverToQueue(voiceOverAsset);
            }
        }
    }
}