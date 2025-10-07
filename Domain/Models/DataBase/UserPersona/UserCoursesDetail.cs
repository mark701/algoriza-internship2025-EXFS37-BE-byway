using Domain.Models.DataBase.AdminPersona;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.DataBase.UserPersona
{
    public class UserCoursesDetail
    {
        public int UserCoursesDetailID { get; set; }
        public int UserCoursesHeaderID { get; set; }

        public int courseID { get; set; }

        public decimal coursePrice { get; set; }

        public UserCoursesHeader ?userCoursesHeader { get; set; }

        public Courses ? Courses { get; set; }



    }
}
