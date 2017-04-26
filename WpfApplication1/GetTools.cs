using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace SpeedBump
{
    public class GetTools
    {
        public string getSLNFile(string filename)
        {
            int last;
            string newfile = null;
            try
            {
                last = filename.LastIndexOf("\\");
                newfile = filename + filename.Substring(last) + ".sln";
            }
            catch (NullReferenceException help)
            {
                Console.WriteLine(help);

            }
            return newfile;
        }

        public Version getjsonVersion(myFile json)
        {

            Version ver = new Version();
            int count = 0;
            ver.setName(json.getFilename());

            string temp = "";
            string pattern = "[:][' ']\"[^\"]+\"";
            foreach (string line in json.getData())
            {

                if (line.Contains("version") && count < 2)
                {
                    ver = new Version();
                    ver.setType("Parent");
                    var match = Regex.Match(line, pattern);
                    temp = (match.Value);
                    temp = temp.Substring(3);
                    temp = temp.TrimEnd('\"');
                    ver.setVersion(temp);
                    return ver;

                }
                else count++;
            }
            return ver;
        }





        public myFile openJSON(string filename)
        {
            myFile json = new myFile();
            json.setFilename(filename);
            try
            {
                foreach (string line in File.ReadLines(filename + "\\manifest.json", Encoding.UTF8))
                {
                    json.add(line);
                }
            }
            catch (System.IO.FileNotFoundException) { Console.WriteLine(); }


            return json;
        }
        public Version getchildVersion(myFile file)
        {
            Version ver = new Version();

            string temp = "";
            string pattern = "\"[^\"]+\"";
            foreach (string line in file.getData())
            {

                if (line.Contains("[assembly: AssemblyVersion(") && !line.Contains("//"))
                {
                    ver = new Version();
                    ver.setType("Child");
                    ver.setName(file.getFilename());
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
        public myFile openAssemblyInfo(string filename)
        {
            myFile info = new myFile();
            info.setFilename(filename);
            string path = filename + "\\properties\\AssemblyInfo.cs";
            if (File.Exists(path))
            {
                foreach (string line in File.ReadLines(filename + "\\properties\\AssemblyInfo.cs", Encoding.UTF8))
                { info.add(line); }
            }
            return info;
        }
        public List<string> GetDependencies(string filename)
        {
            List<string> depen = new List<string>();
            List<string> depen2 = new List<string>();
            string newfile = getSLNFile(filename);
            int first;
            int last;
            string holder;
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
        public List<string> GetChildrenPath(string filename)
        {
            string newfile = getSLNFile(filename);
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
        public List<string> GetChildren(string filename)
        {
            string newfile = getSLNFile(filename);
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
        public List<string> GetDirectories(string filename)
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
            catch (ArgumentException e)
            {
                Console.WriteLine(e);
            }

            return temp;


        }
        public bool verify(myFile json)
        {
            bool matches = true;
            Version jsonversion = getjsonVersion(json);
            Dictionary<string, string> kids = getAllChildrenVersions(json.getFilename());

            foreach (KeyValuePair<string, string> entry in kids)
            {
                if (entry.Value != jsonversion.getVersion())
                {
                    matches = false;
                    Exception MismatchedVersion = new Exception(); throw MismatchedVersion;
                }
            }

            return matches;
        }

        public Dictionary<string, string> getAllChildrenVersions(string filename)
        {
            Dictionary<string, string> kids = new Dictionary<string, string>();
            List<string> filesdirec = GetDirectories(filename);
            myFile file = new myFile();
            foreach (string child in filesdirec)
            {
                file = openAssemblyInfo(child);
                {
                    if (file.getCount() != 0)
                    {
                        Version little = getchildVersion(file);
                        kids.Add(little.getName(), little.getVersion());
                    }
                }

            }

            return kids;
        }

        public Dictionary<string, string> bumpChildrenTrivial(string filename)
        {
            Dictionary<string, string> kids = new Dictionary<string, string>();
            List<string> files = GetDirectories(filename);
            myFile file = new myFile();
            foreach (string child in files)
            {
                file = openAssemblyInfo(child);
                Version little = getchildVersion(file);
                little.bumpTrivial();
                kids.Add(little.getName(), little.getVersion());

            }

            return kids;
        }
        public Dictionary<string, string> bumpChildrenMinor(string filename)
        {
            Dictionary<string, string> kids = new Dictionary<string, string>();
            List<string> files = GetDirectories(filename);
            myFile file = new myFile();
            foreach (string child in files)
            {
                file = openAssemblyInfo(child);
                Version little = getchildVersion(file);
                little.bumpMinor();
                kids.Add(little.getName(), little.getVersion());

            }

            return kids;
        }
        public Dictionary<string, string> bumpChildrenMajor(string filename)
        {
            Dictionary<string, string> kids = new Dictionary<string, string>();
            myFile file = new myFile();
            List<string> files = GetDirectories(filename);
            foreach (string child in files)
            {
                file = openAssemblyInfo(child);
                Version little = getchildVersion(file);
                little.bumpMajor();
                kids.Add(little.getName(), little.getVersion());

            }

            return kids;
        }
        public Dictionary<string, string> bumpChildrenRewrite(string filename)
        {
            Dictionary<string, string> kids = new Dictionary<string, string>();
            List<string> files = GetDirectories(filename);
            myFile file = new myFile();
            foreach (string child in files)
            {
                file = openAssemblyInfo(child);
                Version little = getchildVersion(file);
                little.bumpRewrite();
                kids.Add(little.getName(), little.getVersion());

            }

            return kids;
        }
        public bool writechildVersion(Dictionary<string, string> files)
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
                    {
                        temp[count] = Regex.Replace(thing, pattern, '"' + pair.Value + '"');
                        count = 0;
                        break;
                    }
                    count++;
                }

                File.WriteAllLines(pair.Key + "\\properties\\AssemblyInfo.cs", temp);

            }
            return worked;
        }
        public bool writejsonVersion(Version file)
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

        public string lastdirect()
        {
            string temp = "";
            string location = "C:\\Users\\Pat\\Desktop\\location.txt";


            if (File.Exists(location))
            {
                using (StreamReader file = new StreamReader(location))
                {
                    char[] badchar = { '\r', '\n' };
                    temp = file.ReadToEnd();
                    temp = temp.TrimEnd(badchar);
                    return temp;
                }
            }


            return temp;
        }
        public void writeDirec(string filename)
        {
            using (StreamWriter file = new StreamWriter("C:\\Users\\Pat\\Desktop\\location.txt"))
            {
                file.WriteLine(filename);
            }
        }
        public bool findOtherDep(myFile json)
        {
            Version ver = getjsonVersion(json);
            string tempstr;
            string pattern = "[:][' ']\"[^\"]+\"";
            foreach (string location in json.getData())
            {
                int last = json.getFilename().LastIndexOf('\\');
                tempstr = json.getFilename().Substring(last + 1);
                try
                {
                    string[] temp = File.ReadAllLines(location + "\\manifest.json");
                    for (int i = 0; i < temp.Count() - 1; i++)
                    {
                        if (temp[i].Contains(tempstr))
                        {
                            temp[i + 1] = Regex.Replace(temp[i + 1], pattern, ": \"" + ver.getVersion() + '"');
                        }
                    }
                    File.WriteAllLines(location + "\\manifest.json", temp);
                }
                catch (FileNotFoundException e) { Console.WriteLine(e); }

            }

            return true;
        }
        public void backupFile(string filename)
        {
            Dictionary<string, string> file = getAllChildrenVersions(filename);
            foreach (KeyValuePair<string, string> line in file)
            {
                StringBuilder str = new StringBuilder(line.Key);
                str.Append(@"\properties\BackUp_");
                str.Append(line.Value + @"\");
                Directory.CreateDirectory(str.ToString());
                File.Copy(line.Key + "\\properties\\AssemblyInfo.cs", str.ToString() + "\\AssemblyInfo.cs",true);



            }
        }

    }
}

