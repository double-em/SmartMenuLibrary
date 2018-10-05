using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SmartMenuLibrary;
using System.IO;

namespace SmartMenuTest
{
    [TestClass]
    public class TestVersion1
    {
        private SmartMenu menu;


        [TestInitialize]
        public void SetupForTest()
        {
            menu = new SmartMenu();
            string path = "MenuTestEN.txt";
            menu.langSet = true;
            menu.LoadMenu(path);
        }
        [TestMethod]
        public void TestMenuIDload()
        {    
            Assert.AreEqual("DoThis", menu.MenuID(0));
        }
        [TestMethod]
        public void TestMenuListLoad()
        {
            Assert.AreEqual("Do this", menu.MenuList(0));
        }
        [TestMethod]
        public void TestMenuNameload()
        {
            Assert.AreEqual("My Fantastic Menu", menu.getmenuName());
        }
        [TestMethod]
        public void TestMenuDescriptionLoad()
        {
            Assert.AreEqual("(Press Menu number or 0 to exit)", menu.getmenuDescription());
        }
    }
}
