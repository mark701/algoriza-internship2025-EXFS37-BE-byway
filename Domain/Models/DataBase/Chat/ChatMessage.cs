using Domain.Models.DataBase.UserPersona;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.DataBase.Chat
{
    public class ChatMessage
    {
        public int MessageID { get; set; }
        public int SenderID { get; set; }
        public int ReceiverID { get; set; }
        public string MessageText { get; set; }
        public bool IsRead { get; set; }
        public DateTime SentDateTime { get; set; }

        // Navigation properties
        public Users Sender { get; set; }
        public Users Receiver { get; set; }
    }
}
