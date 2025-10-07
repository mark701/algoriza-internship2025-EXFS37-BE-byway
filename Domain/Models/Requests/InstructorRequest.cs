using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Requests
{
    public class InstructorRequest
    {
        public int instructorID { get; set; }

        public int jobTitleID { get; set; }

        public string instructorName { get; set; }
        public int courseRate { get; set; }

        public string instructorDescription { get; set; }


        public IFormFile? InstructorImage { get; set; }


    }
}
