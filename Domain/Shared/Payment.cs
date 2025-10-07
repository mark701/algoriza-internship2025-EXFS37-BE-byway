using Domain.Models.DataBase.UserPersona;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Shared
{
    public abstract class Payment
    {
      
        public int PaymentID{ get; set; }
        public int UserID { get; set; }

        public string Country {  get; set; }
        
        public string State { get; set; }
        public decimal Amount { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public Users? Users { get; set; }
    }
}
