using System;
using SixLabors.ImageSharp;
using System.IO;

namespace WebIdentityApi.Services
{
    public class ImageServices
    {
        public string CreateImgPath(string pathFor, string imagebase64)
        {
            byte[] imageBytes = Convert.FromBase64String(imagebase64);
            var uniqueFileName = Guid.NewGuid().ToString() + "_" + ".jpg";
            var filePath = Path.Combine($"wwwroot/images/{pathFor}", uniqueFileName);
            using (var img = Image.Load(imageBytes))
            {
                img.Save(filePath);
            }
            return filePath;
        }
    }
}