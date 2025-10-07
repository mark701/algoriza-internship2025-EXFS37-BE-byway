using Domain.Enum;
using Domain.Models.DataBase.UserPersona;
using Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Models.DataBase.AdminPersona
{
    public class Courses : CreatedDate
    {
        public int courseID { get; set; }
        public string courseName { get; set; }

        public int categoryID { get; set; }
        public int instructorID { get; set; }

        public CourseLevelType courseLevel { get; set; }

        public int courseRate { get; set; }

        public decimal courseHours { get; set; }

        public decimal CoursePrice { get; set; }

        public string CourseDescription { get; set; }

        public string CourseCertification { get; set; }

        public string CourseImagePath { get; set; }



        public categories? categories { get; set; }

        public instructors? instructors { get; set; }


        public ICollection<Content>? content { get; set; }

        public ICollection<UserCoursesDetail>? userCoursesDetails { get; set; }





    }
}
