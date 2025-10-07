using Domain.Models.DataBase.AdminPersona;
using Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Models.DataBase.UserPersona
{
    public class UserCoursesHeader : CreatedDate
    {
        public int UserCoursesHeaderID { get; set; }
        public int UserID { get; set; }

        public decimal Total { get; set; }

        public decimal Tax { get; set; }

        public decimal Discount { get; set; }

        [JsonIgnore]
        public Users users { get; set; }


        public ICollection<UserCoursesDetail> userCoursesDetails { get; set; }



    }
}
