using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnnotateYoloImage
{
    public static class IoUtils
    {
        public static List<string> File2List(string fileName)
        {
            var res = new List<string>();
            using (var stream = new FileStream(fileName, FileMode.Open))
            {
                using (var reader = new StreamReader(stream))
                {
                    while (true)
                    {
                        var line = reader.ReadLine();
                        if (string.IsNullOrEmpty(line))
                            break;
                        res.Add(line);
                    }
                }
            }
            return res;
        }

        public static void String2File(string filePath, string value)
        {
            try
            {
                if (File.Exists(filePath))
                    File.Delete(filePath);
                using (StreamWriter sr = new StreamWriter(filePath))
                {
                    sr.Write(value);
                    sr.Flush();
                }
            }
            catch (Exception e)
            {
            }
        }

        public static void List2File(string filePath, List<string> list)
        {
            try
            {
                if (File.Exists(filePath))
                    File.Delete(filePath);
                using (StreamWriter sr = new StreamWriter(filePath))
                {
                    foreach (var line in list)
                    {
                        sr.WriteLine(line);
                    }
                    sr.Flush();
                }
            }
            catch (Exception e)
            {
            }
        }
    }
}
