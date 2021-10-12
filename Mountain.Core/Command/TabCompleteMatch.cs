using Mountain.Core.Chat;

namespace Mountain.Core.Command
{
    public class TabCompleteMatch
    {
        public string Insert { get; set; }
        public BaseChatMessage Tooltip { get; set; }

        public bool HasTooltip => Tooltip != null;
    }
}
