using Domain.Enum;
using Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.DataBase.AdminPersona
{
    public class categories
    {

        public int categoryID { get; set; }
        public string categoryName { get; set; }
        public string categoryDescription { get; set; }

        public string categoryImagePath { get; set; }


        public ICollection<Courses> courses { get; set; }

    }
}
