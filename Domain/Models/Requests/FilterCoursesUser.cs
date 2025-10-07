using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Requests
{
    public class FilterCoursesUser
    {
        public decimal courseRate { get; set; } 

        public int MinLecture { get; set; }
        
        public int MaxLecture { get; set; }
        
        public decimal MinPrice { get; set; } 
        public decimal MaxPrice { get; set; } 

        public string[]?Category { get; set; }
    }
}
