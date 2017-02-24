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
        private char[] verString;

        public Version()
        {
            type = "";
            name = "";
            version = new int[3];
            verString = new char[7];
        }
        public Version(int[] array)
        {
            type = "";
            name = "";
            version = array;
            verString = new char [7];
        }
        public Version(int[] version2,string type2, string name2,char[]verString2)
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
        public void setType(string newtype)
        { type = newtype; }

        public void setName(string newname)
        { name = newname; }
        public void setVersion(string ver)
        {
            char[] charVer = ver.ToCharArray();
            verString= charVer;

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
        public char[] toString()
        {
            int count = 0;
            foreach (int num in version)
            {
                if (count < 7)
                {
                    verString[count] = ((char)num);
                    verString[count + 1] = '.';
                }
                else verString[count] = ((char)num);
            }
            return verString;
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
