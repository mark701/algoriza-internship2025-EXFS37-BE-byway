using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Extentions
{
    public static class CustomImages
    {
        public static bool CheckWidthHeightImage(IFormFile formFile,int Width, int Height)
        {
            using (var stream = formFile.OpenReadStream())
            using (var image = Image.FromStream(stream))
            {
                // Validate dimensions
                if (image.Width > Width || image.Height > Height)
                {
                    return false;
                }
                else return true;
            }

            

        }
    }
}
