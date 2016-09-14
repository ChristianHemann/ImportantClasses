using System.Collections.Generic;
using System.Linq;
using ImportantClasses;
using ImportantClasses.Settings;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    [TestClass]
    public class TestSettings
    {
        [TestInitialize]
        public void Init()
        {
            Settings.Initialize(typeof(TestSettingsTestObjects));
        }

        [TestMethod]
        public void TestFindSettingAttribute()
        {
            Assert.IsTrue(Settings.IsInitialized);
            Assert.AreEqual(4, Settings.SettingItems.Count);
            Assert.AreEqual(1, Settings.GetSettings("").Count);
        }

        [TestMethod]
        public void TestSettingItem()
        {
            SettingItem item = Settings.GetSettings("").First();
            Assert.AreEqual(0f, item.Value);
            Assert.AreEqual(1f, item.DefaultValue);
            Assert.AreEqual(0d, item.MinValue);
            Assert.AreEqual(100d, item.MaxValue);
            Assert.AreEqual(2, item.DecimalPlaces);
            Assert.IsFalse(item.HasChanges);
            Assert.IsTrue(item.IsNumeric);
            Assert.IsTrue(item.IsPointNumber);
            Assert.AreEqual("", item.Path);
            Assert.AreEqual("set1", item.Name);

            item.ChangeValueTemporary(1.5f);
            Assert.AreEqual(1.5f, item.Value);
            Assert.IsTrue(item.HasChanges);

            item.DiscardTemporaryChange();
            Assert.AreEqual(0f, item.Value);
            Assert.IsFalse(item.HasChanges);

            item.RestoreDefaultValue();
            Assert.AreEqual(1f, item.Value);
            Assert.IsTrue(item.HasChanges);

            item.AdoptTemporaryChange();
            Assert.AreEqual(1f, item.Value);
            Assert.IsFalse(item.HasChanges);

            item.Value = 200; //bigger than MaxValue
            Assert.AreEqual(100f, item.Value);

            item.Value = -20; //smaller than MinValue
            Assert.AreEqual(0f, item.Value);

            Assert.AreEqual(item, item); //test equals function
        }

        [TestMethod]
        public void TestSettingsClass()
        {
            Assert.AreEqual(0, Settings.GetSettings("a").Count);
            Assert.AreEqual(1, Settings.GetSettings("a/b").Count);
            Assert.AreEqual(1, Settings.GetMenuItems("a/b").Count);

            List<string> menuItems = Settings.GetMenuItems("a");
            Assert.AreEqual(2, menuItems.Count);
            Assert.IsTrue(menuItems.Contains("c"));
            Assert.IsTrue(menuItems.Contains("b"));

            Assert.AreEqual(null,Settings.GetSetting("a/b","set1"));
            Assert.AreEqual(null,Settings.GetSetting("a/c","set2"));
            Assert.AreEqual(null,Settings.GetSetting("a","set2"));

            SettingItem item = Settings.GetSetting("a/b", "set2");
            Assert.AreEqual(0,item.Value);
            item.ChangeValueTemporary(1);
            Settings.AdoptAllTemporaryChanges();
            Assert.IsFalse(item.HasChanges);
            Assert.AreEqual(1, item.Value);
            Settings.RestoreAllDefaultValues();
            Assert.AreEqual(0, item.Value);
            Assert.IsTrue(item.HasChanges);
            Settings.DiscardAllTemporaryChanges();
            Assert.AreEqual(1, item.Value);
        }
    }

    public class TestSettingsTestObjects
    {
        [Setting("", "set1", 1f, 0, 100, 2)]
        public static float Set1 = 0f;

        [Setting("a/b", "set2",0)]
        public int Set2 = 0;

        [Setting("a/c", "set3")]
        public Message Set3 = new Message("set3");

        [Setting("a/b/c", "set4")]
        public bool Set4 = false;

        [ContainSettings]
        public static TestSettingsTestObjects Instance = new TestSettingsTestObjects();
    }
}
