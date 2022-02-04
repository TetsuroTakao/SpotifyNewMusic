using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotifyNewMusic.Models
{
    public class MessageModel
    {
        public DateTime EventOccours { get; set; }
        public string EventSource { get; set; }
        public string EventMessage { get; set; }
        public ErrorModel Error { get; set; }
        public string Memo { get; set; }
        public MessageType Type { get; set; }
    }
    public class ErrorModel
    {
        public DateTime ErrorOccours { get; set; }
        public string ErrorSource { get; set; }
        public string ErrorMessage { get; set; }
    }

    [Flags]
    public enum MessageType
    {
        Log = 1,
        Display = 2,
        Dialog = 4,
        Info = 8,
        Error = 16,
        Link = 32,
        ExternalLink=64,

        Default=Log,
        DisplayInfo = Display | Info,
        DisplayError = Display | Error
    }
}
