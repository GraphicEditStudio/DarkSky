namespace VoiceOver
{
    
    
    [System.Serializable]
    public class VoiceOverDialogue
    {
        public string text = "";
        public float typingSpeed = 0;
        public float onScreenTime = -1;
        public float startWritingDelay = 0.0f;
        public bool overrideCurrentText = true;
        public string startEscapeSequence = "\n";
        public string endEscapeSequence = " ";
    }
}