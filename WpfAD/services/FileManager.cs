using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfAD.services
{
    internal interface IFileManager
    {
        public void Save(string filePath, string content);
        public string Read(string filePath);

    }
    internal class FileManager : IFileManager
    {
        public string Read(string filePath)
        {
            using var sr = new StreamReader(filePath);
            return sr.ReadToEnd();
        }

        public void Save(string filePath, string content)
        {
            using var sw = new StreamWriter(filePath);
            sw.Write(content);
        }
    }
}
