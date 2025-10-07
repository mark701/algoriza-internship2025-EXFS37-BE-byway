using Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.DataBase.AdminPersona
{
    public class JobTitle
    {
        public int JobTilteID { get; set; }

        public string JobTilteName { get; set; }


        public ICollection<instructors>? instructors { get; set; }

    }
}
