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

        static public Version getjsonVersion(string filename)
        {
            
            Version ver = new Version();
            int count = 0;
            ver.setType("Parent");
            foreach (string line in File.ReadLines(filename + "\\manifest.json", Encoding.UTF8))
            {
                if (line.Contains("version") && count < 2)
                {
                    line.TrimStart(' ');
                    ver.setVersion(line.Substring(13, 7));

                    count++;
                }
                else count++;
            }


            return ver;
        }
        static public Version getDepVersion(string filename)
        {
            Version ver = new Version();
            string newfile = getFileEnd(filename);
            string temp;
            ver.setName(filename);
            foreach (string line in File.ReadLines(filename + "\\properties\\AssemblyInfo.cs", Encoding.UTF8))
            {
                    
                if (line.Contains("[assembly: AssemblyVersion(\"1.0.0.0\")]"))
                {
                    line.TrimStart(' ');
                    temp = line.Substring(27, 7);
                    ver.setVersion(temp);

                }
            }


            return ver;
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

