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
                    ver = new Version();
                    var match = Regex.Match(line, pattern);
                    temp =(match.Value);
                    temp = temp.Substring(3);
                    temp = temp.TrimEnd('\"');
                    ver.setVersion(temp);
                    ver.setName(filename);
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
            foreach (string line in File.ReadLines(filename + "\\properties\\AssemblyInfo.cs", Encoding.UTF8))
            {

                if (line.Contains("[assembly: AssemblyVersion(") && !line.Contains("//"))
                {
                    ver = new Version();
                    ver.setName(filename);
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

    static public Dictionary<string, string> bumpChildrenTrivial(string filename)
    {
        Dictionary<string, string> kids = new Dictionary<string, string>();
        List<string> files = GetDirectories(filename);
        foreach (string child in files)
        {
            Version little = getchildVersion(child);
            little.bumpTrivial();
            kids.Add(little.getName(), little.getVersion());

        }

        return kids;
    }
     static public Dictionary<string, string> bumpChildrenMinor(string filename)
        {
            Dictionary<string, string> kids = new Dictionary<string, string>();
            List<string> files = GetDirectories(filename);
            foreach (string child in files)
            {
                Version little = getchildVersion(child);
                little.bumpMinor();
                kids.Add(little.getName(), little.getVersion());

            }

            return kids;
        }
     static public Dictionary<string, string> bumpChildrenMajor(string filename)
        {
            Dictionary<string, string> kids = new Dictionary<string, string>();
            List<string> files = GetDirectories(filename);
            foreach (string child in files)
            {
                Version little = getchildVersion(child);
                little.bumpMajor();
                kids.Add(little.getName(), little.getVersion());

            }

            return kids;
        }
     static public Dictionary<string, string> bumpChildrenRewrite(string filename)
        {
            Dictionary<string, string> kids = new Dictionary<string, string>();
            List<string> files = GetDirectories(filename);
            foreach (string child in files)
            {
                Version little = getchildVersion(child);
                little.bumpRewrite();
                kids.Add(little.getName(), little.getVersion());

            }

            return kids;
        }
      static public bool writechildVersion(Dictionary<string, string> files)
        {
            bool worked = false;
            string pattern = "\"[^\"]+\"";
            int count = 0;
            foreach (KeyValuePair<string, string> pair in files)
            {
                string[] temp = File.ReadAllLines(pair.Key + "\\properties\\AssemblyInfo.cs");
                foreach (string thing in temp)
                {
                    if (thing.Contains("[assembly: AssemblyVersion(") && !thing.Contains("//"))
                    {temp[count] = Regex.Replace(thing, pattern, '"' + pair.Value + '"');
                    count = 0;
                    break;
                    }
                    count++;
                }
                
                File.WriteAllLines(pair.Key + "\\properties\\AssemblyInfo.cs", temp);
                
            }
            return worked;
        }
        static public bool writejsonVersion(Version file)
        {
            bool worked = false;
            string pattern = "[:][' ']\"[^\"]+\"";
            int count = 0;
            string[] temp = File.ReadAllLines(file.getName() + "\\manifest.json");
            foreach (string thing in temp)
                {
                    if (thing.Contains("version") && count < 2)
                    {
                        temp[count] = Regex.Replace(thing, pattern, ": \"" + file.getVersion() + '"');
                    worked = true;
                        break;
                    }
                    count++;
                }

                File.WriteAllLines(file.getName() + "\\manifest.json", temp);
            return worked;
            }
           
        static public string lastdirect()
        {
            string temp = "";
            string location = "C:\\Users\\Pat\\Desktop\\location.txt";


            if (File.Exists(location)){
                using (StreamReader file = new StreamReader(location)) 
                {
                    char[] badchar = { '\r', '\n' };
                    temp = file.ReadToEnd();
                    temp = temp.TrimEnd(badchar);
                    return temp; }
            }


            return temp;
        }
    static public void writeDirec(string filename)
        {
            using (StreamWriter file = new StreamWriter("C:\\Users\\Pat\\Desktop\\location.txt"))
            {
                file.WriteLine(filename);
            }
        }
    }

}

