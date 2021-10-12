using Mountain.Core.Exceptions;
using Mountain.Core.Enums;
using System.Text.Json.Serialization;

namespace Mountain.Core.Chat
{
    public class InteractEvent
    {
        [JsonPropertyName("action")]
        public string ActionName { get; private set; }
        private InteractEventAction lastAction = InteractEventAction.Unset;
        [JsonIgnore]
        public InteractEventAction Action
        {
            get
            {
                return lastAction != InteractEventAction.Unset ? lastAction : ActionName switch
                {
                    "open_url" => InteractEventAction.OpenUrl,
                    "run_command" => InteractEventAction.RunCommand,
                    "suggest_command" => InteractEventAction.SuggestCommand,
                    "change_page" => InteractEventAction.ChangePage,
                    "show_text" => InteractEventAction.ShowText,
                    "show_item" => InteractEventAction.ShowItem,
                    "show_entity" => InteractEventAction.ShowEntity,
                    _ => InteractEventAction.Unset
                };
            }
            set
            {

                if (lastAction != value) ActionName = value switch
                {
                    InteractEventAction.OpenUrl => "open_url",
                    InteractEventAction.RunCommand => "run_command",
                    InteractEventAction.SuggestCommand => "suggest_command",
                    InteractEventAction.ChangePage => "change_page",
                    InteractEventAction.ShowText => "show_text",
                    InteractEventAction.ShowItem => "show_item",
                    InteractEventAction.ShowEntity => "show_entity",
                    _ => null
                };
                lastAction = value;
            }
        }
        [JsonPropertyName("value")]
        public string Value { get; set; }

        public void ValidateAsClickEvent()
        {
            switch (Action)
            {
                case InteractEventAction.OpenUrl:
                case InteractEventAction.RunCommand:
                case InteractEventAction.SuggestCommand:
                case InteractEventAction.ChangePage:
                    return;
                default:
                    throw new PropertyException("Inappropriate action provided for ClickEvent - " + Action + " is a HoverEvent");
            }
        }

        public void ValidateAsHoverEvent()
        {
            switch (Action)
            {
                case InteractEventAction.ShowText:
                case InteractEventAction.ShowItem:
                case InteractEventAction.ShowEntity:
                    return;
                default:
                    throw new PropertyException("Inappropriate action provided for HoverEvent - " + Action + " is a ClickEvent");
            }
        }

    }
}
