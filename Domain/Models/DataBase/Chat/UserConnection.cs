using Domain.Models.DataBase.UserPersona;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.DataBase.Chat
{
    public class UserConnection
    {
        public string ConnectionID { get; set; }
        public int UserID { get; set; }
        public DateTime ConnectedAt { get; set; }

        // Navigation property
        public Users User { get; set; }
    }

}
