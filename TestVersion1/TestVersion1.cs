using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SmartMenuLibrary;
using System.IO;

namespace SmartMenuTest
{
    [TestClass]
    public class TestVersion1
    {
        [TestMethod]
        public void Test1()
        {
            SmartMenu m = new SmartMenu();
            string path = "MenuSpec";
            path = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"..\..\..\MenuErrors.txt"));
            m.LoadMenu(path);
        }
    }
}
