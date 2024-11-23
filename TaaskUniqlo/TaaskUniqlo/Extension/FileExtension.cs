namespace TaaskUniqlo.Extention;

public static class FileExtension
{
  public static bool IsValidType(this string Type)
    {

        if (Type.StartsWith("image") )
        {
            return true;
        }
        return false;
    }

    public static bool IsValidSize(this long kb)
    {
        if (kb< 2 * 1024)
        {
            return true;
        }
        return false;
    }
    public static string Upload(this IFormFile file, string path)
    {
        string fullPath = Path.Combine(path, Path.GetRandomFileName() + Path.GetExtension(file.FileName));
        using (Stream stream = File.Create(fullPath))
        {
            file.CopyTo(stream);
        }
        return Path.GetRandomFileName() + Path.GetExtension(file.FileName);
    }
}
