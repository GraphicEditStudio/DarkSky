
using System;
using UnityEngine;

namespace VoiceOver
{
    [CreateAssetMenu(menuName = "Voice Over/New Asset", fileName = "Voice Over Asset")]
    public class VoiceOverAsset : ScriptableObject
    {
        public VoiceOverScript script;
        public bool overrideQueue = false;

        private void OnEnable()
        {
            script.Reset();
        }
    }
}