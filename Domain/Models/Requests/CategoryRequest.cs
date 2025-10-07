using Domain.Enum;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Requests
{
    public class CategoryRequest
    {
        public int categoryID { get; set; }

        public string categoryName { get; set; }

        public string categoryDescription { get; set; }
        public IFormFile? Image { get; set; }
    }
}
