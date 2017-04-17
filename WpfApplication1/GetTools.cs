using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

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
            string temp = "";
            string pattern = "[:][' ']\"[^\"]+\"";
            foreach (string line in File.ReadLines(filename + "\\manifest.json", Encoding.UTF8))
            {
                
                if (line.Contains("version") && count <2)
                {
                    var match = Regex.Match(line, pattern);
                    temp =(match.Value);
                    temp = temp.Substring(3);
                    temp = temp.TrimEnd('\"');
                    ver.setVersion(temp);
                    return ver;
                    
                }
                else count++;
            }


            return ver;
        }
        static public Version getchildVersion(string filename)
        {
            Version ver = new Version();
            ver.setType("Child");
            string temp = "";
            string pattern = "\"[^\"]+\"";
            ver.setName(filename);
            foreach (string line in File.ReadLines(filename + "\\properties\\AssemblyInfo.cs", Encoding.UTF8))
            {
                    
                if (line.Contains("[assembly: AssemblyVersion(\"1.0.0.0\")]"))
                {
                    var match = Regex.Match(line, pattern);
                    temp = (match.Value);
                    temp = temp.Substring(1);
                    temp = temp.TrimEnd('\"');
                    ver.setVersion(temp);
                    
                    return ver;

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
            catch (Exception ex)
            {
                if (ex is ArgumentNullException || ex is FileNotFoundException)
                {
                    Console.WriteLine("There was a slight issue");
                }
            }
            return depen2;
        }
        static public List<string> GetChildrenPath(string filename)
        {
            string newfile = getFileEnd(filename);
            List<string> depen = new List<string>();
            try
            {
                foreach (string line in File.ReadLines(newfile, Encoding.UTF8))
                {
                    if (!line.Contains("\"..\\") && line.Contains(".csproj"))
                    {
                        depen.Add(line);
                    }
                }
      
            }
            catch (Exception ex)
            {
                if (ex is ArgumentNullException || ex is FileNotFoundException)
                {
                    Console.WriteLine("There was a slight issue");
                }
            }
            return depen;
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
            catch (Exception ex)
            {
                if (ex is ArgumentNullException || ex is FileNotFoundException)
                {
                    Console.WriteLine("There was a slight issue");
                }
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
        static public bool verify(string filename) {
            bool matches = true;
            Version json = getjsonVersion(filename);
            Exception MismatchedVersion = new Exception();
            Dictionary<string, string> kids = getAllChildrenVersions(filename);

            foreach(KeyValuePair<string, string> entry in kids)
            {
                if (entry.Value != json.getVersion()) throw MismatchedVersion; matches = false;
            }

            return matches;
        }
    
    static public Dictionary<string, string> getAllChildrenVersions(string filename)
    {
            Dictionary<string, string> kids = new Dictionary<string, string>();
            List<string> files = GetDirectories(filename);
            foreach(string child in files)
            {
                Version little = getchildVersion(child);
                kids.Add(little.getName(), little.getVersion());

            }

        return kids;
    }
}

}

