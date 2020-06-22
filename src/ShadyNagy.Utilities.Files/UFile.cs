using System.IO;

namespace ShadyNagy.Utilities.Files
{
    public class UFile
    {
        public static byte[] ReadBytes(string path, string fileName)
        {
            var fullPath = Path.Combine(path, fileName);

            using (MemoryStream ms = new MemoryStream())
            using (FileStream file = new FileStream(fullPath, FileMode.Open, FileAccess.Read))
            {
                var bytes = new byte[file.Length];
                file.Read(bytes, 0, (int)file.Length);
                ms.Write(bytes, 0, (int)file.Length);

                return ms.ToArray();
            }
        }

        public static void WriteBytes(string path, string fileName, byte[] data)
        {
            var fullPath = Path.Combine(path, fileName);

            using (var file = new FileStream(fullPath, FileMode.Create, FileAccess.ReadWrite))
            {
                file.Write(data, 0, (int)data.Length);

            }
        }
    }
}
