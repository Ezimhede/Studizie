using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Web;

namespace Studizie.Models
{
    public class ValidateFileAttribute : RequiredAttribute
    {
        public override bool IsValid(object value)
        {
            if (!(value is HttpPostedFileBase file))
            {
                return true;
            }

            if (file.ContentLength > 1 * 150 * 150)
            {
                return false;
            }


            using (var img = Image.FromStream(file.InputStream))
            {
                return img.RawFormat.Equals(img.RawFormat.Equals(ImageFormat.Png) ? ImageFormat.Png : ImageFormat.Jpeg);
            }

        }
    }
}