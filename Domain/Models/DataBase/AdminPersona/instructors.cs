using Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Models.DataBase.AdminPersona
{
    public class instructors : CreatedDate
    {
        public int instructorID { get; set; }

        public int jobTitleID { get; set; }
        public string instructorName { get; set; }

        public int courseRate { get; set; }

        public string instructorDescription { get; set; }

        public string instructorImagePath { get; set; }


        public ICollection<Courses> courses { get; set; }


        [JsonIgnore]
        public JobTitle jobTitle { get; set; }

    }
}
