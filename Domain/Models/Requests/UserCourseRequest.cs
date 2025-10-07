using Domain.Models.DataBase.UserPersona;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Models.Requests
{
    public class UserCourseRequest
    {
        public int UserCoursesHeaderID { get; set; }
        public int UserID { get; set; }

        public decimal Total { get; set; }

        public decimal Tax { get; set; }

        public decimal Discount { get; set; }

        public PaymentRequest Payment { get; set; }

        public ICollection<UserCoursesDetail> userCoursesDetails { get; set; }

    }
}
