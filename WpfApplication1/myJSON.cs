using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SpeedBump
{
    public class myFile
    {
        public List<string> data = new List<string>();
        private string filename;

        public myFile(){
        }

        public myFile(List<string> info)
        {
            data = info;
        }
        public myFile(string[] info)
        {
            List<string> data = new List<string>(info);
        }
        public void setData(List<string> info)
        {
            data = info;
        }
        public void add(string line)
        {
            data.Add(line);
        }
        public List<string> getData() {
            return data;
        }
        public void setFilename(string name)
        {
            if (Directory.Exists(name))
            {
                filename = name;
            }

        }
        public string getFilename()
        {
            return filename;
        }
        public int getCount()
        {
            return data.Count();
        }
    }
}
