using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Unity.Properties;
using UnityEngine;
using UnityEngine.UIElements;
using Utils;

namespace VoiceOver
{
    public class VoiceOverManager : MonoSingleton<VoiceOverManager>
    {
        [SerializeField] private string dialogText;
        private StringBuilder dialogTextBuilder;
        private Queue<VoiceOverAsset> voiceOverQueue;
        private VoiceOverAsset currentAsset;
        private VoiceOverDialogue currentDialogue;
        private UIDocument uiDocument;
        private Label dialogLabel;
        protected override void OnAwake()
        {
            voiceOverQueue = new Queue<VoiceOverAsset>();
            dialogTextBuilder = new StringBuilder();

            uiDocument = GetComponentInChildren<UIDocument>();
            dialogLabel = uiDocument.rootVisualElement.Q("dialogue") as Label;
            if (dialogLabel != null)
            {
                dialogLabel.SetBinding(nameof(TextField.value),
                    new DataBinding() { dataSource = this, dataSourcePath = new PropertyPath("dialogText") });
            }
        }

        public void AddVoiceOverToQueue(VoiceOverAsset asset)
        {
            if (asset.overrideQueue)
            {
                voiceOverQueue.Clear();
            }

            if (voiceOverQueue.Contains(asset))
                return;

            voiceOverQueue.Enqueue(asset);
            Debug.Log("Added new voice over asset to queue");
        }

        private void Update()
        {
            if (currentAsset == null && voiceOverQueue.Count > 0)
            {
                currentAsset = voiceOverQueue.Dequeue();
            }

            if (currentAsset != null && !currentAsset.script.Started)
            {
                currentDialogue = currentAsset.script.StartDialogue();
                PlayCurrentDialogue().DoNotAwait();
            }
        }

        private async Task PlayCurrentDialogue()
        {
            if (currentDialogue.overrideCurrentText)
            {
                dialogTextBuilder.Clear();
            }
            else
            {
                dialogTextBuilder.Append(currentDialogue.startEscapeSequence);
            }

            if (currentDialogue.startWritingDelay > 0)
            {
                await Task.Delay((int)currentDialogue.startWritingDelay * 1000);
            }


            var delay = Mathf.RoundToInt(currentDialogue.typingSpeed * 1000);

            for (var i = 0; i < currentDialogue.text.Length; i++)
            {
                dialogTextBuilder.Append(currentDialogue.text[i]);
                dialogText = dialogTextBuilder.ToString();
                Debug.Log(dialogText);
                await Task.Delay(delay);
            }

            if (!string.IsNullOrEmpty(currentDialogue.endEscapeSequence))
            {
                dialogTextBuilder.Append(currentDialogue.endEscapeSequence);
            }


            if (currentDialogue.onScreenTime > 0)
            {
                await Task.Delay(Mathf.RoundToInt(currentDialogue.onScreenTime * 1000));
            }

            currentDialogue = currentAsset.script.NextDialogue();
            if (currentDialogue != null)
            {
                PlayCurrentDialogue().DoNotAwait();
            }
            else
            {
                currentAsset = null;
            }
        }
    }
}