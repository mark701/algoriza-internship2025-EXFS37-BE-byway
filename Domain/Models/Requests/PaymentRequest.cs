using Domain.Models.DataBase.UserPersona;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Requests
{
    public class PaymentRequest
    {

        public int PaymentID { get; set; }
        public int UserID { get; set; }

        public string Country { get; set; }

        public string State { get; set; }
        public decimal Amount { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public string PaymentType {  get; set; }



        //credit  card      
        public string? CardName { get; set; }

        public string ?CardNumber { get; set; }

        public string? CVV { get; set; }

        public string ?ExpiryDate { get; set; }



        //paypal
        public string ?PaypalEmail { get; set; }


    }
}
