using System;
using Newtonsoft.Json;

namespace ScriptPlayer.Shared.Scripts
{
    public abstract class ScriptAction
    {
        [JsonIgnore]
        public TimeSpan TimeStamp;
        public TimeSpan MediaDuration;

        [JsonIgnore]
        public bool OriginalAction { get; set; }

        public abstract bool IsSameAction(ScriptAction action);
    }
}