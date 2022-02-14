using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using Utilities.DataModel;

namespace Utilities.Tests
{
    [TestClass]
    public class FileManagerTests
    {
        [TestMethod]
        public void SaveNewModelTest()
        {
            UtilityDataModel model = new UtilityDataModel();

            FileManager.Save(model, "./test");

            Assert.IsTrue(File.Exists("./test/Utility.data.json"));
        }

        [TestMethod]
        public void SaveExistingModelTest()
        {
            UtilityDataModel model = new UtilityDataModel();

            FileManager.Save(model, "\\test");

            Assert.IsTrue(File.Exists("\\test\\Utility.data.json"));
            Assert.IsTrue(File.Exists("\\test\\backup\\Utility.data.json.backup"));
        }

        [TestMethod]
        public void LoadModelTest()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void LoadNotExistingFileTest()
        {
            Assert.Inconclusive();
        }
    }
}