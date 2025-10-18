using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Requests.ChatDTO
{
    public class ChatHistoryDto
    {
        public int UserID { get; set; }
        //public string UserName { get; set; }
        public string FirstName { get; set; }  // ✅ Added
        public string LastName { get; set; }   // ✅ Added
        public string LastMessage { get; set; }
        public DateTime? LastMessageTime { get; set; }
        public int UnreadCount { get; set; }
        public bool IsOnline { get; set; }
    }
}
