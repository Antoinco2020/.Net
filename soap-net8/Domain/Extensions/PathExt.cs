
namespace Domain.Extensions
{
    public static class PathExt
    {
        public static string CombinePath(params string[] paths)
        {
            return Path.Combine(paths);
        }

        public static string GetExtension(this string fileName)
        {
            return Path.GetExtension(fileName);
        }

        public static string GetExtUpper(this string fileName)
        {
            return Path.GetExtension(fileName).ToUpper();
        }
        public static string GetName(this string path)
        {
            return Path.GetFileName(path);
        }

        public static string GetNameWithoutExt(this string path)
        {
            return Path.GetFileNameWithoutExtension(path);
        }

        public static string GetDirectory(this string path)
        {
            return Path.GetDirectoryName(path);
        }

        public static string GetParentDirName(this string path)
        {
            return Directory.GetParent(path).Name;
        }
    }
}
