using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Requests.ChatDTO
{
    public class MessageDto
    {
        public int MessageID { get; set; }
        public int SenderID { get; set; }
        public string SenderName { get; set; }
        public int ReceiverID { get; set; }
        public string ReceiverName { get; set; }
        public string MessageText { get; set; }
        public bool IsRead { get; set; }
        public DateTime SentDateTime { get; set; }
    }
}
