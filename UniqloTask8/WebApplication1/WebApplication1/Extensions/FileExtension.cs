using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using WebApplication1.FileRegulations;

namespace AB108Uniqlo.Extensions
{
    public static class FileExtension
    {
        public static bool IsValidType(this IFormFile file)
            => file.ContentType.StartsWith(FileRegulation.TypeImage);
        public static bool IsValidSize(this IFormFile file)
            => file.Length <= FileRegulation.FileSize;
        public static async Task<string> UploadAsync(this IFormFile file,params string[] paths)
        {
            string errormessage = "";
            if (!IsValidSize(file)) errormessage += $"Faylın ölçüsü {FileRegulation.FileSize / (1024*1024)} mb - dan böyükdür. ";
            if (!IsValidSize(file)) errormessage += $"Faylın tipi '{FileRegulation.TypeImage}' olmalıdır. ";
            if (errormessage != "")
            {
                return errormessage;
            }
            string uploadPath = Path.Combine(paths);
            if (!Path.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }
            string FileName = "" + DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second + Path.GetExtension(file.FileName);

            using (Stream stream = File.Create(Path.Combine(uploadPath, FileName)))
            {
                await file.CopyToAsync(stream);
            }
            return FileName;
        }
    }
}
