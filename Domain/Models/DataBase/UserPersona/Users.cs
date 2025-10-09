using Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.DataBase.UserPersona
{
    public class Users : CreatedDate
    {
        public int UserID { get; set; }
        public string UserName { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string UserEmail { get; set; }

        public string Rule { get; set; }

        public string? PasswordHash { get; set; }
        public string? PasswordSlat { get; set; }

        public string? TypeSign { get; set; }

        public ICollection<UserCoursesHeader>? UserCourses { get; set; }

        public ICollection<Payment>? payments { get; set; }



    }
}
