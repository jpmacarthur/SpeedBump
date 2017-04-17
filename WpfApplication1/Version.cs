using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WpfApplication1
{
    class Version
    {
        private string type;
        private string name;
        private int[] version;
        private string verString;

        public Version()
        {
            type = "";
            name = "";
            version = new int[4];

        }
        public Version(int[] array)
        {
            type = "";
            name = "";
            version = array;

        }
        public Version(int[] version2, string type2, string name2, string verString2)
        {
            type = type2;
            name = name2;
            version = version2;
            verString = verString2;
        }
        public string getType() {;
            return type; }
        public string getName() {
            return name; }
        public string getVersion()
        {
            return verString;
        }
        public void setType(string newtype)
        { type = newtype; }

        public void setName(string newname)
        { name = newname; }
        public void setVersion(string ver)
        {
            string charVer = ver;
            verString = charVer;

        }
        public void bumpRewrite()
        {
            version[0] += 1;
        }
        public void bumpMajor()
        {
            version[1] += 1;
        }
        public void bumpMinor()
        {
            version[2] += 1;
        }
        public void bumpTrivial()
        {
            version[3] += 1;
        }
        public string toString()
        {
            string temp = "";
            foreach (int num in version)
            {
                object o = new object();
                o = num;
                temp = temp + num + '.';
            }
                
                
        
            return temp;
        }
        public int[] toArray()
        {
            int count = 0;
            for(int i=0; i < 8; i=i + 2, count++)
            {
                if (verString[i] != '.')
                {
                    version[count] = verString[i];
                }
            }
            return version;
        }
    }
}
