using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Utils
{
    public static class XmlUtils
    {
        public static bool WriteToXml<T>(T obj, string folder, string fileName)
        {
            if (obj == null)
            {
                throw new ArgumentException("Object cannot be null!");
            }
            try
            {
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }
                var fullPath = Path.Combine(folder, fileName);
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                using (StreamWriter sw = new StreamWriter(fullPath))
                {
                    serializer.Serialize(sw, obj);
                }
                return true;
            }
            catch
            {
                throw;
            }
        }

        public static T ReadFromXml<T>(string filePath)
        {
            object? result = null;
            try
            {
                if (!File.Exists(filePath))
                {
                    throw new IOException("File not found!");
                }
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                using (StreamReader sr = new StreamReader(filePath))
                {
                    while (!sr.EndOfStream)
                    {
                        result = serializer.Deserialize(sr);
                    }
                }
                if (result == null)
                {
                    throw new IOException("Could not serialize data!");
                }
            }
            catch
            {
                throw;
            }
            return (T)result;
        }
    }
}