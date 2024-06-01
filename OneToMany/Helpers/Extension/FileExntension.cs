using System;
using System.IO;

namespace OneToMany.Helpers.Extension
{
	public static class FileExntension
	{
		public static bool CheckFileSize(this IFormFile file , int size)
		{
			return file.Length/1024 < size;
		}

		public static bool CheckFileType(this IFormFile file,string pattern)
		{
			return file.ContentType.Contains(pattern);
		}

		public async static Task SaveFileToLocalAsync(this IFormFile file , string path)
		{
            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
        }

		public static void DeleteFileFromLocal(this string path)
		{//file ozunu folderdende silir wwwrootdaki
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
	}
}

