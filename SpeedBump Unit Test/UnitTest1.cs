using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpeedBump;
using Newtonsoft.Json;
using System.IO;
using System.Collections.Generic;

namespace SpeedBump_Unit_Test
{
    [TestClass]
    public class UnitTest1
    {
        GetTools getTools = new GetTools();

        [TestMethod]
        public void TestGetSLNFile()
        {
            string project = "Task Scheduler";
            string filename = @"c:\My Projects\" + project;
            string result = getTools.getSLNFile(filename);
            string expected = filename + @"\" + project + ".sln";
            Assert.IsNotNull(expected);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestGetJSONVersion()
        {
            string filename = @"c:\My Projects\Task Scheduler";
            myFile json = getTools.openJSON(filename);
            SpeedBump.Version result = getTools.getjsonVersion(json);
            string expected = "1.0.0.0";
            Assert.IsNotNull(expected);
            Assert.AreEqual(expected, result.getVersion());
        }

        [TestMethod]
        public void TestOpenJson()
        {
            string filename = @"c:\My Projects\TT Gateway";
            myFile json = new myFile();
            json = getTools.openJSON(filename);
            Assert.IsNotNull(json.getData()); }
        [TestMethod]
        public void TestParseJson()
        {
            myFile json = new myFile();
            string expected = "1.0.0.0";
            SpeedBump.Version ver = new SpeedBump.Version();
            json.setFilename(@"c:\My Projects\TT Gateway");
            json.add("version : \"1.0.0.0\"");
            ver = getTools.getjsonVersion(json);

            Assert.AreEqual(expected, ver.getVersion());

        }

        [TestMethod]
        public void TestVerify()
        {
            string filename = @"c:\My Projects\Task Scheduler";
            myFile json = getTools.openJSON(filename);
            bool worked = false;
            try { worked = getTools.verify(json); Assert.IsTrue(worked); }
            catch(Exception MismatchedVersion) { Assert.IsFalse(worked); }
            filename = @"fasdfas";
            json.setFilename(filename);
            try { worked = getTools.verify(json); Assert.IsTrue(worked); }
            catch (Exception MismatchedVersion) { Assert.IsFalse(worked); }

        }
        [TestMethod]
        public void TestOpenAssembly()
        {
            string filename = @"c:\My Projects\TT Gateway";
            myFile assembly = getTools.openAssemblyInfo(filename);
            Assert.IsNotNull(assembly.getData());

        }

        [TestMethod]
        public void TestBackup()
        {
            string test = @"c:\My Projects\TT Gateway";
            getTools.backupFile(test);

            Assert.IsTrue(File.Exists(@"C:\My Projects\TT Gateway\TT Gateway Console\Properties\BackUp_1.0.0.3\AssemblyInfo.cs"));
            Assert.IsTrue(File.Exists(@"C:\My Projects\TT Gateway\TT Gateway DLL\Properties\BackUp_1.0.0.3\AssemblyInfo.cs"));
            Assert.IsTrue(File.Exists(@"C:\My Projects\TT Gateway\TT Gateway Windows Service\Properties\BackUp_1.0.0.3\AssemblyInfo.cs"));
        }
    }

    
}
