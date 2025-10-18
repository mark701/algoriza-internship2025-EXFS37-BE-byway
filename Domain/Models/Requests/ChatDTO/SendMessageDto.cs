using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Requests.ChatDTO
{
    public class SendMessageDto
    {
        public int ReceiverID { get; set; }
        public string MessageText { get; set; }
    }
}
