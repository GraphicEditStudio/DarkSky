using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
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
        private CancellationTokenSource playDialogueCancellationTokenSource;
        private CancellationToken playDialogueCancellationToken;
        private CancellationTokenSource clearScreenCancellationTokenSource;
        private CancellationToken clearScreenCancellationToken;
        protected override void OnAwake()
        {
            voiceOverQueue = new Queue<VoiceOverAsset>();
            dialogTextBuilder = new StringBuilder();
            UISetup();
        }

        private void UISetup()
        {
            uiDocument = GetComponentInChildren<UIDocument>(includeInactive: true);
            dialogLabel = uiDocument.rootVisualElement?.Q("dialogue") as Label;
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
                currentAsset.script.StopAudio();
                if (playDialogueCancellationToken.CanBeCanceled)
                {
                    playDialogueCancellationTokenSource.Cancel();
                    playDialogueCancellationTokenSource.Dispose();
                }
                currentAsset = null;
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
                playDialogueCancellationTokenSource = new CancellationTokenSource();
                playDialogueCancellationToken = playDialogueCancellationTokenSource.Token;
                PlayCurrentDialogue(playDialogueCancellationToken).DoNotAwait();
            }
        }

        private async Task PlayCurrentDialogue(CancellationToken cancellationToken)
        {
            if (clearScreenCancellationToken.CanBeCanceled)
            {
                clearScreenCancellationTokenSource.Cancel();
                clearScreenCancellationTokenSource.Dispose();
            }
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
                await Task.Delay((int)currentDialogue.startWritingDelay * 1000, cancellationToken);
                if (cancellationToken.IsCancellationRequested)
                {
                    return;
                }
            }


            var delay = Mathf.RoundToInt(currentDialogue.typingSpeed * 1000);

            for (var i = 0; i < currentDialogue.text.Length; i++)
            {
                dialogTextBuilder.Append(currentDialogue.text[i]);
                dialogText = dialogTextBuilder.ToString();
                await Task.Delay(delay, cancellationToken);
                if (cancellationToken.IsCancellationRequested)
                {
                    return;
                }
            }

            if (!string.IsNullOrEmpty(currentDialogue.endEscapeSequence))
            {
                dialogTextBuilder.Append(currentDialogue.endEscapeSequence);
            }


            if (currentDialogue.onScreenTime > 0)
            {
                await Task.Delay(Mathf.RoundToInt(currentDialogue.onScreenTime * 1000), cancellationToken);
            }

            currentDialogue = currentAsset.script.NextDialogue();
            if (currentDialogue != null)
            {
                playDialogueCancellationTokenSource = new CancellationTokenSource();
                playDialogueCancellationToken = playDialogueCancellationTokenSource.Token;
                PlayCurrentDialogue(playDialogueCancellationToken).DoNotAwait();
            }
            else
            {
                currentAsset = null;
                if (voiceOverQueue.Count == 0)
                {
                    clearScreenCancellationTokenSource = new CancellationTokenSource();
                    clearScreenCancellationToken = clearScreenCancellationTokenSource.Token;
                    ClearDialogText().DoNotAwait();
                }
            }
        }

        private async Task ClearDialogText()
        {
            await Task.Delay(1000, clearScreenCancellationToken);
            dialogTextBuilder.Clear();
            dialogText = dialogTextBuilder.ToString();
        }
    }
}