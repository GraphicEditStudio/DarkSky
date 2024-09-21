using System.Collections.Generic;
using FMOD.Studio;
using UnityEngine;

namespace VoiceOver
{
    using FMODUnity;
    
    [System.Serializable]
    public class VoiceOverScript
    {
        public EventReference eventReference;
        public List<VoiceOverDialogue> dialogues;
        public bool Started { get; private set; }
        public bool Completed { get; private set; }
        public int CurrentDialogueIndex { get; private set; }

        private EventInstance audioEventInstance;
        public VoiceOverDialogue StartDialogue()
        {
            if (Started)
            {
                return null;
            }
            
            Started = true;
            audioEventInstance = FMODUnity.RuntimeManager.CreateInstance(eventReference);
            if (audioEventInstance.isValid())
            {
                audioEventInstance.start();
            }
            
            CurrentDialogueIndex = 0;
            return dialogues[CurrentDialogueIndex];
        }

        public VoiceOverDialogue NextDialogue()
        {
            CurrentDialogueIndex++;
            if (CurrentDialogueIndex < dialogues.Count) return dialogues[CurrentDialogueIndex];
            Completed = true;
            return null;
        }
        public void Reset()
        {
            Started = false;
            Completed = false;
            CurrentDialogueIndex = 0;
        }

        public void StopAudio()
        {
            if (audioEventInstance.isValid())
            {
                audioEventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            }
        }
    }
}