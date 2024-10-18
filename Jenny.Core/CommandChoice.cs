using static Jenny.Core.DictationChoicesBuilder;

namespace Jenny.Core
{
    public abstract class CommandChoice
    {
        public Dictionary<string, SpeechAction> triggers;
    }
}
