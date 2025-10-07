using Domain.Enum;
using Domain.Models.DataBase.AdminPersona;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Requests
{
    public class CoursesRequest
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

        public ICollection<Content> content { get; set; }
        public IFormFile? CourseImage { get; set; }

    }
}
