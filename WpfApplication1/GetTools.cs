using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace WpfApplication1
{
    class GetTools
    {
        static public string getFileEnd(string filename) {
            int last;
            string newfile = null;
            try
            {
                last = filename.LastIndexOf("\\");
                newfile = filename + filename.Substring(last) + ".sln";
            }
            catch(NullReferenceException help)
            {
                Console.WriteLine(help);
                
            }
            return newfile;
        }
        static public string getjsonVersion(string filename)
        {
            string lines = "";
            Version ver = new Version();
            int count = 0;
            foreach (string line in File.ReadLines(filename, Encoding.UTF8))
            {
                if (line.Contains("Version:") && count < 2)
                {
                    line.TrimStart(' ');
                    lines = (line.Substring(12,7));
                    count++;
                }
            }


            return lines;
        }
        static public List<Version> getDepVersion(string filename)
        {
            List<Version> lines = new List<Version>();
            Version ver = new Version();
            int count = 0;
            string temp;
            foreach (string line in File.ReadLines(filename, Encoding.UTF8))
            {
                if (line.Contains("Project:"))
                {
                     
                    line.TrimStart(' ');
                    temp = line.Substring(12);
                    temp = temp.TrimEnd('\"');
                    ver.setName(temp);
                    
                }
                else if (line.Contains("Version:") && count > 2)
                {
                    line.TrimStart(' ');
                    temp = line.Substring(12, 7);
                    ver.setVersion(temp);
                    count++;
                }
                lines.Add(ver);
            }


            return lines;
        }

        static public List<string> GetDependencies(string filename)
        {
            List<string> depen = new List<string>();
            List<string> depen2 = new List<string>();
            string newfile = getFileEnd(filename);
            int first;
            int last;
            string holder;
            /*StreamReader streamReader = new StreamReader(filename);
            string line;
            while ((line = streamReader.ReadLine()) != null)
            {
                if (line.Contains("\"..\\"))
                {
                    depen.Add(line);
                }
            }*/
            try
            {
                foreach (string line in File.ReadLines(newfile, Encoding.UTF8))
                {
                    if (line.Contains("\"..\\"))
                    {
                        depen.Add(line);
                    }
                }
                foreach (string line in depen)
                {
                    first = line.IndexOf("=");
                    last = line.IndexOf(",");
                    if (last > 0)
                    {
                        holder = line.Substring(first + 3);
                        last = holder.IndexOf(",") - 1;
                        holder = holder.Substring(0, last);
                        depen2.Add(holder);
                    }
                }
            }
            catch(ArgumentNullException help)
            {
                Console.WriteLine(help);
            }
            return depen2;
        }

        static public List<string> GetChildren(string filename)
        {
            string newfile = getFileEnd(filename);
            List<string> depen = new List<string>();
            List<string> depen2 = new List<string>();
            int first;
            int last;
            string holder;
            try
            {
                foreach (string line in File.ReadLines(newfile, Encoding.UTF8))
                {
                    if (!line.Contains("\"..\\") && line.Contains(".csproj"))
                    {
                        depen.Add(line);
                    }
                }
                foreach (string line in depen)
                {
                    first = line.IndexOf("=");
                    last = line.LastIndexOf(",");
                    if (last > 0)
                    {
                        holder = line.Substring(first + 3);
                        last = holder.IndexOf(",") - 1;
                        holder = holder.Substring(0, last);
                        depen2.Add(holder);
                    }
                }
            }
            catch(ArgumentNullException help)
            {
                Console.WriteLine(help);
            }
            return depen2;
        }
        static public List<string> GetDirectories(string filename)
        {
            string[] dirs;
            List<string> temp = new List<string>();
            try
            {
                dirs = Directory.GetDirectories(filename);
                foreach (string dirt in dirs)
                {
                    temp.Add(dirt);
                }
            }
            catch(ArgumentException e)
            {
                Console.WriteLine(e);
            }
            
            /*foreach(string dirt in dirs)
            {
                temp.Add(dirt);
            }*/


            return temp;


        }
    }
}

