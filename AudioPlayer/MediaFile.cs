using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioPlayer
{
    public class MediaFile
    {
        public string FileName { get; set; }
        public string Path { get; set; }

        public MediaFile(string fileName, string path)
        {
            this.FileName = fileName;
            this.Path = path;
        }
        public MediaFile() : this("", "") { }
    }
}
