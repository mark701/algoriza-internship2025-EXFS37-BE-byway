using Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.DataBase.AdminPersona
{
    public class Content : CreatedDate
    {
        public int contentID { get; set; }

        public int courseID { get; set; }
        public string contentName { get; set; }

        public int LecturesNumber { get; set; }
        public decimal contentHour { get; set; }


        public Courses? Courses { get; set; }


    }
}
